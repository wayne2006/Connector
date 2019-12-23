using Connector.DotNettySocket;
using DotNetty.Buffers;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ark.Connector.Base
{
    internal class TcpServerChannelHandler : CommonChannelHandler
    {
        private static int TRY_TIMES = 3;
        private int currentTime = 0;
        public TcpServerChannelHandler(IChannelEvent channelEvent) : base(channelEvent)
        {

        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, object msg)
        {
            var buffer = msg as IByteBuffer;
            if (buffer != null)
            {
                var message = buffer.ToString(Encoding.UTF8);
                if (string.Equals("heartbeat", message))
                {
                    byte[] messageBytes = Encoding.UTF8.GetBytes("reply");
                    var msgbuffer = ctx.Allocator.Buffer(messageBytes.Length).WriteBytes(messageBytes);
                    ctx.WriteAndFlushAsync(msgbuffer);
                }
                else
                {
                    base.ChannelRead0(ctx, msg);
                }
            }  
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            base.ChannelActive(context);
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            base.ChannelInactive(context);
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            base.ExceptionCaught(context,exception);
        }

        public override void UserEventTriggered(IChannelHandlerContext context, object evt)
        {
            if (evt is IdleStateEvent)
            {
                var eventState = evt as IdleStateEvent;
                if (eventState != null && eventState.State == IdleState.ReaderIdle)
                {
                    if (currentTime < TRY_TIMES)
                    {
                        currentTime++;
                        logger.LogDebug(context.Channel.RemoteAddress + "丢失第 " + currentTime + " 个心跳包");
                    }
                    else
                    {
                        //如果3次心跳包都没有回应，则断开链接
                        context.Channel.CloseAsync();
                        logger.LogError($"没有心跳响应 {context.Channel},断开连接");
                    }
                }
            }
        }
    }
}
