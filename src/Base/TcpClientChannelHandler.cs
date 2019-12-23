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
    internal class TcpClientChannelHandler : CommonChannelHandler
    {
        private static int TRY_TIMES = 3;
        private int currentTime = 0;
        public TcpClientChannelHandler(IChannelEvent channelEvent) : base(channelEvent)
        {

        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, object msg)
        {
            var buffer = msg as IByteBuffer;
            if (buffer != null)
            {
                var message = buffer.ToString(Encoding.UTF8);
                if (!string.Equals("reply", message))
                {
                    base.ChannelRead0(ctx, msg);
                }
                else
                {
                    //收到心跳响应
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
                if (eventState != null && eventState.State == IdleState.WriterIdle)
                {
                    byte[] messageBytes = Encoding.UTF8.GetBytes("heartbeat");
                    try
                    {
                        var msg = context.Allocator.Buffer(messageBytes.Length).WriteBytes(messageBytes);
                        context.WriteAndFlushAsync(msg);
                        logger.LogError($"发送心跳检测 {context.Channel}");

                    }
                    catch (Exception ex) 
                    {
                        logger.LogError(ex.Message);
                    }
                    
                }
            }
        }
    }
}
