using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Net.Sockets;
using System.Numerics;
using TTG_Shared.Packets;

namespace TTG_Server.Models; 

public class Room {

    public readonly Guid ID = Guid.NewGuid();
    public string Name { get; }
    public ushort MaxPlayers { get; }
    public ushort MaxTraitors { get; }

    private readonly List<Player> _players;

    private readonly Color[] _colors = new Color[16] {
        Color.Red,
        Color.Green,
        Color.Blue,
        Color.Yellow,
        Color.Orange,
        Color.Purple,
        Color.Cyan,
        Color.Magenta,
        Color.Brown,
        Color.Gray,
        Color.SlateBlue,
        Color.White,
        Color.LightBlue,
        Color.LightGreen,
        Color.LightPink,
        Color.LightGray
    };

    public IReadOnlyList<Player> Players => this._players;

    public Room(Player owner, string name, ushort maxPlayers, ushort maxTraitors) {
        owner.Color = this.GetRandomColor();
        this._players = new List<Player>(maxPlayers) { owner };

        this.Name = name;
        this.MaxPlayers = maxPlayers;
        this.MaxTraitors = maxTraitors;

        TTGServer.Instance.Rooms.Add(this.ID, this);
        Console.WriteLine($"Created a room with ID: {this.ID}");
    }

    public Room(Client owner, CreateRoomPacket packet) : this(new Player(owner, packet.Nickname), packet.Name, packet.MaxPlayers, packet.MaxTraitors) {}

    public bool GetPlayerByClient(Client client, [MaybeNullWhen(false)] out Player player) {
        player = this._players.FirstOrDefault(player => player.Client.Equals(client));
        return player != null;
    }

    private Color GetRandomColor() {
        var random = new Random();
        int index;

        do {
            index = random.Next(0, this._colors.Length);
        } while (this._colors[index] == Color.Empty);

        var selectedColor = this._colors[index];
        this._colors[index] = Color.Empty;

        return selectedColor;
    }

    private void FreeColor(Color color) {
        int index;
        for (index = 0; this._colors[index] != Color.Empty; index++) {}
        this._colors[index] = color;
    }

    public void UpdatePosition(Client client, PlayerMovementPacket packet) {
        if (!this.GetPlayerByClient(client, out var cPlayer)) return;
        cPlayer.Position = packet.Position;

        var movementPacket = new PlayerMovementPacket(cPlayer.Position, packet.Texture, packet.Direction, cPlayer.Client.ID);
        foreach (var player in this._players)
            if (!player.Client.Equals(cPlayer.Client))
                player.Client.SendPacket(ProtocolType.Udp, movementPacket);
    }

    public void AddPlayer(Client client, string nickname) {
        var newPlayer = new Player(client, nickname, this.GetRandomColor());
        client.SendPacket(ProtocolType.Tcp, new JoinRoomResultPacket(true, "", this.ID, this.MaxPlayers, newPlayer.Color));

        var connectPacket = new ConnectRoomPacket(client.ID, newPlayer.Nickname, newPlayer.Color, newPlayer.Position);
        foreach (var player in this._players)
            player.Client.SendPacket(ProtocolType.Udp, connectPacket);

        this._players.Add(newPlayer);
    }

    public void RemovePlayer(Client client) {
        if (!this.GetPlayerByClient(client, out var cPlayer) || !this._players.Remove(cPlayer)) return;

        this.FreeColor(cPlayer.Color);

        var disconnectPacket = new DisconnectRoomPacket(client.ID);
        foreach (var player in this._players)
            player.Client.SendPacket(ProtocolType.Udp, disconnectPacket);

        if (this._players.Count <= 0)
            this.Shutdown();
    }

    public void Shutdown() {
        if (!TTGServer.Instance.Rooms.Remove(this.ID)) return;

        var removePacket = new RemoveRoomPacket(this.ID);
        foreach (var client in TTGServer.Instance.Clients)
            if (client.Key.Equals(client.Value.TCPIPEndPoint))
                client.Value.SendPacket(ProtocolType.Tcp, removePacket);

        Console.WriteLine($"Deleted the room with ID: {this.ID}");
    }

}