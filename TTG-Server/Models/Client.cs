using System.Net;
using System.Net.Sockets;
using TTG_Shared.Models;
using System;

namespace TTG_Server.Models; 

public class Client {

    public readonly Guid ID = Guid.NewGuid();
    private readonly IPEndPoint _tcpIpEndPoint;
    private readonly IPEndPoint _udpIpEndPoint;

    private TcpClient _tcpClient;

    private readonly CancellationTokenSource _taskCancellationTokenSource = new();
    private Task _task;

    public Client(TcpClient tcpClient, IPEndPoint udpIPEndPoint) {
        this._tcpClient = tcpClient;
        this._tcpIpEndPoint = (IPEndPoint) this._tcpClient.Client.RemoteEndPoint!;
        this._udpIpEndPoint = udpIPEndPoint;

        TTGServer.Instance.Clients.Add(this._tcpIpEndPoint, this);
        TTGServer.Instance.Clients.Add(this._udpIpEndPoint, this);

        this._task = Task.Run(this.TCPDataReceivedCallback, this._taskCancellationTokenSource.Token);
    }

    public void SendPacket(Packet packet) {
        var packetBytes = packet.ToBytes();
        this._tcpClient.GetStream().Write(packetBytes, 0, packetBytes.Length);
    }

    public void ProcessPacket(ProtocolType protocol, Packet packet) {
        Console.WriteLine(
            "[{0}] Packet received from {1} : {2}",
            protocol == ProtocolType.Tcp ? "TCP" : "UDP",
            protocol == ProtocolType.Tcp ? this._tcpIpEndPoint : this._udpIpEndPoint,
            packet
        );
    }

    public void Shutdown() {
        if (this._taskCancellationTokenSource.IsCancellationRequested) return;

        Console.WriteLine("The TCP client disconnected: {0}", this._tcpClient.Client.RemoteEndPoint);

        if (!this._task.IsCompleted)
            this._taskCancellationTokenSource.Cancel();

        if (TTGServer.Instance.Clients.Remove(this._tcpIpEndPoint))
            this._tcpClient.Close();
    }

    private async Task TCPDataReceivedCallback() {
        try {
            // Start reading data from the client
            var buffer = new byte[1024];
            var stream = this._tcpClient.GetStream();

            while (true) {
                var bytesRead = await stream.ReadAsync(buffer, this._taskCancellationTokenSource.Token);

                if (bytesRead <= 0)
                    break; // Connection closed

                this.ProcessPacket(ProtocolType.Tcp, Packet.FromBytes(buffer[..bytesRead]));
            }
        } catch (Exception ex) {
            Console.WriteLine("Error occurred while handling TCP client: " + ex.Message);
        } finally {
            this.Shutdown();
        }
    }

}