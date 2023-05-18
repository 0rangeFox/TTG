using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TTG_Server;

public class TTGServer : IDisposable {

    private Timer? _broadcastTimer;
    private readonly List<TcpClient> _clients = new();
    private readonly TcpListener _listener;

    public TTGServer(string ip, ushort port) {
        this._listener = new TcpListener(IPAddress.Parse(ip), port);
    }

    public void Dispose() {
        // Stop the timer and clean up
        this._broadcastTimer?.Dispose();
        this._listener.Stop();

        foreach (var client in this._clients)
            client.Close();
    }

    public void Run() {
        try {
            // Start listening for incoming connection requests
            _listener.Start();
            Console.WriteLine("TTG Server started. Listening for connections...");

            // Start the timer to broadcast messages every second
            _broadcastTimer = new Timer(BroadcastMessage, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

            while (true) {
                // Accept an incoming client connection
                var client = _listener.AcceptTcpClient();
                Console.WriteLine("Client connected.");

                // Add the client to the list
                _clients.Add(client);

                // Handle the client connection in a separate task
                Task.Run(() => HandleClient(client));
            }
        } catch (Exception ex) {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    private void HandleClient(TcpClient client) {
        // Get the client's network stream for reading and writing
        var stream = client.GetStream();

        var buffer = new byte[1024];
        int bytesRead;

        try {
            // Receive and process data from the client
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0) {
                // Convert the received data to a string
                var data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Received data: " + data);
            }
        } catch (Exception ex) {
            Console.WriteLine("An error occurred: " + ex.Message);
        } finally {
            // Clean up the client connection
            stream.Close();
            client.Close();

            // Remove the client from the list
            _clients.Remove(client);

            Console.WriteLine("Client disconnected.");
        }
    }

    private void BroadcastMessage(object? state) {
        // Prepare the message to broadcast
        var message = $"{DateTime.Now} | Broadcast message from server.";

        // Convert the message to bytes
        var messageBytes = Encoding.ASCII.GetBytes(message);

        // Send the message to all connected clients
        foreach (var client in _clients)
            try {
                var stream = client.GetStream();
                stream.Write(messageBytes, 0, messageBytes.Length);
            } catch (Exception ex) {
                Console.WriteLine("An error occurred while broadcasting message to a client: " + ex.Message);
            }
    }

}