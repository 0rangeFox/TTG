using System.Net;
using System.Net.Sockets;
using TTG_Server.Managers;
using TTG_Server.Models;
using TTG_Shared;
using TTG_Shared.Packets;

namespace TTG_Server;

public class TTGServer : IDisposable {

    private const string CommandShutdown = "stop";

    public static TTGServer Instance { get; private set; } = null!;

    public readonly TaskManager TaskManager = new();
    public readonly NetworkManager NetworkManager;

    public readonly Dictionary<IPEndPoint, Client> Clients = new();
    public readonly Dictionary<Guid, TcpClient> ClientsHandshaking = new();
    public readonly Dictionary<Guid, Room> Rooms = new();

    public TTGServer(string ip, ushort port) {
        Instance = this;

        this.NetworkManager = new NetworkManager(ip, port);
    }

    public void Run() {
        if (!this.NetworkManager.Initialize())
            return;

        Console.WriteLine($"TTG Server v{GitInformation.Version} ({GitInformation.ShortSha}) started. Write \"Stop\" to shutdown.");
        var input = string.Empty;
        while (!string.Equals(CommandShutdown, input, StringComparison.OrdinalIgnoreCase))
            this.ExecuteCommand(input = Console.ReadLine());
    }

    private void ExecuteCommand(string? command) {
        switch (command?.ToLower()) {
            case "players":
                Console.WriteLine($"There's {this.Clients.Count / 2} clients connected to the server.");
                break;
            case "handshaking":
                Console.WriteLine($"There's {this.ClientsHandshaking.Count} clients handshaking to the server.");
                break;
            case "rooms":
                Console.WriteLine($"There's {this.Rooms.Count} rooms created.");

                foreach (var room in this.Rooms.Values)
                    Console.WriteLine($"Room ID: {room.ID} | Name: {room.Name} | Players: {room.Players.Count} / {room.MaxPlayers}");

                break;
        }
    }

    public void Dispose() {
        this.TaskManager.Dispose();
        this.NetworkManager.Dispose();

        foreach (var client in this.Clients.Values)
            client.Shutdown();

        GC.SuppressFinalize(this);
    }

}