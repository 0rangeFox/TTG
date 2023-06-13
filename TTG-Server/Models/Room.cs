using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Net.Sockets;
using System.Numerics;
using TTG_Shared.Models;
using TTG_Shared.Packets;

namespace TTG_Server.Models; 

public class Room {

    public enum RoomStatus : byte {

        Waiting,
        Starting,
        Running,
        Ending

    }

    public readonly Guid ID = Guid.NewGuid();
    public string Name { get; }
    public ushort MaxPlayers { get; }
    public ushort MaxTraitors { get; }
    public RoomStatus Status { get; private set; } = RoomStatus.Waiting;

    private readonly List<Player> _players;
    private readonly int _minPlayers;
    private readonly int _maxTraitors;
    private readonly HashSet<Guid> _readyPlayers = new();

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
        this._minPlayers = 4 * this.MaxTraitors + 1 - 4;
        this._maxTraitors = (this.MaxPlayers - 1) / 4 + 1;

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
        if (!this.GetPlayerByClient(client, out var cPlayer) || cPlayer.IsDead) return;
        cPlayer.Position = packet.Position;

        var movementPacket = new PlayerMovementPacket(cPlayer.Position, packet.Texture, packet.Direction, cPlayer.Client.ID);
        foreach (var player in this._players)
            if (!player.Client.Equals(cPlayer.Client))
                player.Client.SendPacket(movementPacket);
    }

    public void ExecuteAction(Client client, ExecuteActionPacket packet) {
        if (!this.GetPlayerByClient(client, out var cPlayer)) return;

        switch (packet.Action) {
            case Actions.Kill:
                if (cPlayer.Role != Roles.Traitor || cPlayer.IsDead) break;
                var victim = this._players.Find(victim => victim.Client.ID.Equals(packet.To));
                if (victim == null || victim.IsDead) break;

                victim.IsDead = true;

                var deadPacket = new PlayerMovementPacket(victim.Position, 12, false, victim.Client.ID);
                foreach (var player in this._players) {
                    player.Client.SendPacket(deadPacket);
                    player.Client.SendPacket(packet);
                }

                break;
        }
    }

    public void AddPlayer(Client client, string nickname) {
        var newPlayer = new Player(client, nickname, this.GetRandomColor());
        client.SendPacket(new JoinRoomResultPacket(true, "", this.ID, this.MaxPlayers, newPlayer.Color));

        var connectPacket = new ConnectRoomPacket(client.ID, newPlayer.Nickname, newPlayer.Color, newPlayer.Position);
        foreach (var player in this._players)
            player.Client.SendPacket(connectPacket);

        this._players.Add(newPlayer);
    }

    public void RemovePlayer(Client client) {
        if (!this.GetPlayerByClient(client, out var cPlayer) || !this._players.Remove(cPlayer)) return;

        this.FreeColor(cPlayer.Color);

        var disconnectPacket = new DisconnectRoomPacket(client.ID);
        foreach (var player in this._players)
            player.Client.SendPacket(disconnectPacket);

        if (this._players.Count <= 0)
            this.Shutdown();
    }

    public void SetReadyPlayer(Client client) {
        this._readyPlayers.Add(client.ID);

        if (this._readyPlayers.Count == this._players.Count)
            this.Run();
    }

    private void ShufflePlayerRoles() {
        var random = new Random();
        var traitorsToAdd = this._maxTraitors;

        while (traitorsToAdd > 0) {
            var randomPlayer = this._players[random.Next(this._players.Count)];

            randomPlayer.Role = Roles.Traitor;
            traitorsToAdd--;
        }
    }

    public void Start() {
        if (this.Status != RoomStatus.Waiting || this._players.Count <= this._minPlayers) return;
 
        this.Status = RoomStatus.Starting;
        this.ShufflePlayerRoles();

        foreach (var player in this._players)
            player.Client.SendPacket(new StartRoomPacket(player.Role));
    }

    public void Run() {
        if (this.Status == RoomStatus.Running) return;

        this.Status = RoomStatus.Running;

        var readyPacket = new ReadyRoomPacket();
        foreach (var player in this._players) {
            player.Position = Vector2.Zero;

            var movementPacket = new PlayerMovementPacket(player.Position, 6, false, player.Client.ID);
            player.Client.SendPacket(movementPacket);
            player.Client.SendPacket(readyPacket);
        }

    }

    public void Shutdown() {
        if (!TTGServer.Instance.Rooms.Remove(this.ID)) return;

        var removePacket = new RemoveRoomPacket(this.ID);
        foreach (var client in TTGServer.Instance.Clients)
            if (client.Key.Equals(client.Value.TCPIPEndPoint))
                client.Value.SendPacket(removePacket);

        Console.WriteLine($"Deleted the room with ID: {this.ID}");
    }

}