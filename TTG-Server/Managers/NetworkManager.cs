using System.Net;
using System.Net.Sockets;
using TTG_Server.Models;
using TTG_Shared.Models;
using TTG_Shared.Packets;
using TTG_Shared.Utils;

namespace TTG_Server.Managers; 

public class NetworkManager : IDisposable {

    public readonly IPAddress IP;
    public readonly ushort Port;
    public readonly IPEndPoint EndPoint;

    private readonly TcpListener _tcpListener;
    private readonly UdpClient _udpListener;

    public NetworkManager(string ip, ushort port) {
        this.IP = IPAddress.Parse(ip);
        this.Port = port;
        this.EndPoint = new IPEndPoint(ConverterUtil.ConvertIpToBigEndian(this.IP), this.Port);

        this._tcpListener = new TcpListener(this.EndPoint);
        this._udpListener = new UdpClient(this.EndPoint);
    }

    public bool Initialize() {
        try {
            this._tcpListener.Start();

            this._tcpListener.BeginAcceptTcpClient(this.TCPClientConnectedCallback, null);
            this._udpListener.BeginReceive(this.UDPMessageReceivedCallback, null);
            
            return true;
        } catch (SocketException e) {
            Console.WriteLine(e);
            return false;
        }
    }

    private void TCPClientConnectedCallback(IAsyncResult ar) {
        if (TTGServer.Instance.TaskManager.IsNetworkCancelled) return;

        var tcpClient = this._tcpListener.EndAcceptTcpClient(ar);

        Console.WriteLine("New TCP client connected: {0}", tcpClient.Client.RemoteEndPoint);

        // Send handshake
        var handshakePacket = new HandshakePacket(DateTime.Now, Guid.NewGuid());
        var packetBytes = handshakePacket.ToBytes();
        tcpClient.GetStream().Write(packetBytes, 0, packetBytes.Length);
        TTGServer.Instance.ClientsHandshaking.Add(handshakePacket.Code, tcpClient);

        // Continue accepting new TCP clients
        if (!TTGServer.Instance.TaskManager.IsNetworkCancelled)
            this._tcpListener.BeginAcceptTcpClient(this.TCPClientConnectedCallback, null);
    }

    private void UDPMessageReceivedCallback(IAsyncResult ar) {
        if (TTGServer.Instance.TaskManager.IsNetworkCancelled) return;

        IPEndPoint? clientEndPoint = null;
        var receivedBytes = this._udpListener.EndReceive(ar, ref clientEndPoint);

        if (clientEndPoint != null) {
            var packet = Packet.FromBytes(receivedBytes);
            if (packet is HandshakePacket hp && TTGServer.Instance.ClientsHandshaking.TryGetValue(hp.Code, out var tcpClient)) {
                new Client(tcpClient, clientEndPoint);
                TTGServer.Instance.ClientsHandshaking.Remove(hp.Code);
            } else if (TTGServer.Instance.Clients.TryGetValue(clientEndPoint, out var client))
                client.ProcessPacket(ProtocolType.Udp, packet);
        }

        // Continue receiving UDP messages
        if (!TTGServer.Instance.TaskManager.IsNetworkCancelled)
            this._udpListener.BeginReceive(this.UDPMessageReceivedCallback, null);
    }

    public void Dispose() {
        this._tcpListener.Stop();
        this._udpListener.Close();
        GC.SuppressFinalize(this);
    }

}