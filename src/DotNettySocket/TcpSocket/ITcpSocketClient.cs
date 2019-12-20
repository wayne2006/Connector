namespace Coldairarrow.DotNettySocket
{
    /// <summary>
    /// TcpSocket客户端
    /// </summary>
    public interface ITcpSocketClient : IBaseTcpSocketClient, ISendBytes, ISendString
    {
        /// <summary>
        /// 获取连接状态
        /// </summary>
        /// <returns></returns>
        bool GetStatus();
    }
}