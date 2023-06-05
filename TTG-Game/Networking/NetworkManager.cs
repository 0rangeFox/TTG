using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using TTG_Shared.Models;
using TTG_Shared.Packets;
using TTG_Shared.Utils;

namespace TTG_Game.Networking; 

public class NetworkManager : IDisposable {

    private readonly TcpClient _tcpClient = new();
    private readonly UdpClient _udpClient = new();

    private HandshakePacket? _handshakePacket;

    private readonly CancellationTokenSource _taskCancellationTokenSource = new();
    private readonly Task _task;

    public NetworkManager(string ip, ushort port) {
        var endPoint = new IPEndPoint(ConverterUtil.ConvertIpToBigEndian(ip), port);

        try {
            this._tcpClient.Connect(endPoint);
            this._task = Task.Run(this.TCPDataReceivedCallback, this._taskCancellationTokenSource.Token);
            Console.WriteLine("Connected to the server via TCP and waiting for handshake.");

            while (this._handshakePacket == null)
                Task.Delay(1000);

            Console.WriteLine($"Received Handshake Packet with code {this._handshakePacket.Code} and initial handshake time {this._handshakePacket.Time}.");

            this._udpClient.Connect(endPoint);
            Console.WriteLine("Connected to the server via UDP and sending for handshake.");
            this.SendPacketUDP(this._handshakePacket);
            this._udpClient.BeginReceive(this.UDPMessageReceivedCallback, null);

            this.SendPacketTCP(new TestPacket());
            this.SendPacketUDP(new TestPacket());
        } catch (Exception ex) {
            Console.WriteLine("Error occurred: " + ex.Message);
        }
    }

    public void SendPacketTCP(Packet packet) {
        var packetBytes = packet.ToBytes();
        this._tcpClient.GetStream().Write(packetBytes, 0, packetBytes.Length);
    }

    public void SendPacketUDP(Packet packet) {
        var packetBytes = packet.ToBytes();
        this._udpClient.Send(packetBytes, packetBytes.Length);
    }

    private async Task TCPDataReceivedCallback() {
        try {
            var buffer = new byte[1024];
            var stream = this._tcpClient.GetStream();

            while (true) {
                var bytesRead = await stream.ReadAsync(buffer, this._taskCancellationTokenSource.Token);
            
                if (bytesRead <= 0)
                    break; // Connection closed by the server

                var packet = Packet.FromBytes(buffer[..bytesRead]);

                if (packet is HandshakePacket hp)
                    this._handshakePacket = hp;
            }
        } catch (Exception ex) {
            Console.WriteLine("Error occurred while handling TCP message: " + ex.Message);
        }
    }

    private void UDPMessageReceivedCallback(IAsyncResult ar) {
        IPEndPoint? serverEndPoint = null;
        var receivedBytes = this._udpClient.EndReceive(ar, ref serverEndPoint);

        var packet = Packet.FromBytes(receivedBytes);

        // Continue receiving UDP messages
        this._udpClient.BeginReceive(this.UDPMessageReceivedCallback, null);
    }

    public void Dispose() {
        if (!this._task.IsCompleted)
            this._taskCancellationTokenSource.Cancel();

        this._tcpClient.Close();
        this._udpClient.Close();
        GC.SuppressFinalize(this);
    }

}
