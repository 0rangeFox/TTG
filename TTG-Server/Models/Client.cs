using System.Net;
using System.Net.Sockets;
using TTG_Shared.Models;
using TTG_Shared.Packets;

namespace TTG_Server.Models; 

public class Client {

    public readonly Guid ID;
    public readonly IPEndPoint TCPIPEndPoint;
    public readonly IPEndPoint UDPIPEndPoint;

    private TcpClient _tcpClient;

    private readonly CancellationTokenSource _taskCancellationTokenSource = new();
    private Task _task;

    public Room? Room => TTGServer.Instance.Rooms.Values.FirstOrDefault(room => room.GetPlayerByClient(this, out _));

    public Client(Guid clientID, TcpClient tcpClient, IPEndPoint udpipEndPoint) {
        this.ID = clientID;
        this._tcpClient = tcpClient;
        this.TCPIPEndPoint = (IPEndPoint) this._tcpClient.Client.RemoteEndPoint!;
        this.UDPIPEndPoint = udpipEndPoint;

        TTGServer.Instance.Clients.Add(this.TCPIPEndPoint, this);
        TTGServer.Instance.Clients.Add(this.UDPIPEndPoint, this);

        this._task = Task.Run(this.TCPDataReceivedCallback, this._taskCancellationTokenSource.Token);
    }

    public void SendPacket(ProtocolType protocol, Packet packet) {
        Console.WriteLine(
            "[{0} | {1}] Packet sent for {2} : {3}",
            DateTime.Now,
            protocol == ProtocolType.Tcp ? "TCP" : "UDP",
            protocol == ProtocolType.Tcp ? this.TCPIPEndPoint : this.UDPIPEndPoint,
            packet
        );

        switch (protocol) {
            case ProtocolType.Tcp:
                this._tcpClient.GetStream().Write(packet.ToBytes());
                break;
            case ProtocolType.Udp:
                TTGServer.Instance.NetworkManager.UDPClient.Send(packet.ToBytes(), this.UDPIPEndPoint);
                break;
            default:
                throw new NotImplementedException();
        }
    }

    public void ProcessPacket(ProtocolType protocol, Packet packet) {
        Console.WriteLine(
            "[{0} | {1}] Packet received from {2} : {3}",
            DateTime.Now,
            protocol == ProtocolType.Tcp ? "TCP" : "UDP",
            protocol == ProtocolType.Tcp ? this.TCPIPEndPoint : this.UDPIPEndPoint,
            packet
        );

        switch (packet) {
            case RequestRoomsPacket:
                foreach (var room in TTGServer.Instance.Rooms.Values.ToList())
                    this.SendPacket(ProtocolType.Tcp, new AddRoomPacket(room.ID, room.Name, room.MaxPlayers, room.MaxTraitors));
                break;
            case RequestRoomPlayersPacket:
                foreach (var player in this.Room?.Players ?? Array.Empty<Player>())
                    this.SendPacket(ProtocolType.Udp, new ConnectRoomPacket(player.Client.ID, player.Nickname, player.Color, player.Position));
                break;
            case CreateRoomPacket crp:
                var newRoom = new Room(this, crp);
                this.SendPacket(ProtocolType.Tcp, new CreatedRoomPacket(true, string.Empty, newRoom.ID, newRoom.Players[0].Color));

                var addPacket = new AddRoomPacket(newRoom.ID, newRoom.Name, newRoom.MaxPlayers, newRoom.MaxTraitors);
                foreach (var client in TTGServer.Instance.Clients)
                    if (!client.Value.TCPIPEndPoint.Equals(this.TCPIPEndPoint) && client.Key.Equals(client.Value.TCPIPEndPoint))
                        client.Value.SendPacket(ProtocolType.Tcp, addPacket);

                break;
            case StartRoomPacket:
                this.Room?.Start();
                break;
            case JoinRoomPacket jrp:
                if (TTGServer.Instance.Rooms.TryGetValue(jrp.ID, out var joinRoom)) {
                    if (joinRoom.Players.FirstOrDefault(player => string.Equals(player.Nickname, jrp.Nickname, StringComparison.OrdinalIgnoreCase)) == null)
                        joinRoom.AddPlayer(this, jrp.Nickname);
                    else
                        this.SendPacket(ProtocolType.Tcp, new JoinRoomResultPacket(false, "This nickname is already in-game."));
                } else
                    this.SendPacket(ProtocolType.Tcp, new JoinRoomResultPacket(false, "Can't connect into the server."));
                break;
            case LeaveRoomPacket:
                this.Room?.RemovePlayer(this);
                break;
            case ReadyRoomPacket:
                this.Room?.SetReadyPlayer(this);
                break;
            case PlayerMovementPacket pmp:
                this.Room?.UpdatePosition(this, pmp);
                break;
        }
    }

    public void Shutdown() {
        if (this._taskCancellationTokenSource.IsCancellationRequested) return;

        Console.WriteLine("The TCP client disconnected: {0}", this._tcpClient.Client.RemoteEndPoint);

        this.Room?.RemovePlayer(this);

        if (!this._task.IsCompleted)
            this._taskCancellationTokenSource.Cancel();

        if (TTGServer.Instance.Clients.Remove(this.TCPIPEndPoint))
            this._tcpClient.Close();
    }

    private async Task TCPDataReceivedCallback() {
        Packet? packet = null;
        try {
            // Start reading data from the client
            var buffer = new byte[1024];
            var stream = this._tcpClient.GetStream();

            while (true) {
                var bytesRead = await stream.ReadAsync(buffer, this._taskCancellationTokenSource.Token);

                if (bytesRead <= 0)
                    break; // Connection closed

                // TODO Change this for TCP Buffer Coalescing
                packet = Packet.FromBytes(buffer[..bytesRead]);
                this.ProcessPacket(ProtocolType.Tcp, packet);
            }
        } catch (Exception ex) {
            Console.WriteLine($"Error occurred while handling TCP client ({packet}): {ex.Message}");
        } finally {
            this.Shutdown();
        }
    }

}