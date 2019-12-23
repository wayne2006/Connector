using DotNetty.Common.Internal.Logging;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using System;

namespace Connector.DotNettySocket
{
    class CommonChannelHandler : SimpleChannelInboundHandler<object>
    {
        protected ILogger logger;
        IChannelEvent _channelEvent { get; }
        public CommonChannelHandler(IChannelEvent channelEvent)
        {
            _channelEvent = channelEvent;
            logger = InternalLoggerFactory.DefaultFactory.CreateLogger("Netty");
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, object msg)
        {
            _channelEvent.OnChannelReceive(ctx, msg);
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            _channelEvent.OnChannelActive(context);
        }

        public override void ChannelReadComplete(IChannelHandlerContext context)
        {
            context.Flush();
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            _channelEvent.OnChannelInactive(context.Channel);
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            _channelEvent.OnException(context.Channel, exception);
        }
    }
}