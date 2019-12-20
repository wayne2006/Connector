namespace Connector.DotNettySocket
{
    /// <summary>
    /// TcpSocket连接
    /// </summary>
    public interface ITcpSocketConnection : IBaseSocketConnection, ISendBytes, ISendString
    {

    }
}