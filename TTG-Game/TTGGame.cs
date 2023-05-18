using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TTG_Shared;

namespace TTG_Game;

public class TTGGame : Game {

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private TcpClient _client;
    private NetworkStream _stream;

    private bool _isConnected;
    private string _serverResponse;
    
    private SpriteFont _font;

    public TTGGame() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize() {
        this.Window.Title = $"The Traitor's Gambit v{GitInformation.Version} ({GitInformation.ShortSha})";

        // Set the IP address and port number of the master server
        IPAddress serverIpAddress = IPAddress.Parse("127.0.0.1"); // Use the IP address of the master server
        int serverPort = 7325; // Use the port number of the master server

        // Connect to the server in a separate task to avoid blocking the game
        Task.Run(() => ConnectToServer(serverIpAddress, serverPort));

        base.Initialize();
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        _font = Content.Load<SpriteFont>("Fonts/AmongUs");
    }

    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        if (_isConnected) {
            _spriteBatch.DrawString(
                _font,
                "Connected to server. Server response: " + _serverResponse,
                new Vector2(10, 10),
                Color.White
            );
        } else {
            _spriteBatch.DrawString(
                _font,
                "Not connected to server.",
                new Vector2(10, 10),
                Color.White
            );
        }
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void ConnectToServer(IPAddress serverIpAddress, int serverPort) {
        try {
            // Create a TCP client and connect to the server
            _client = new TcpClient();
            _client.Connect(serverIpAddress, serverPort);
            _stream = _client.GetStream();
            _isConnected = true;
            Console.WriteLine("Connected to server.");

            // Send a message to the server
            string message = "Hello from client!";
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            _stream.Write(messageBytes, 0, messageBytes.Length);
            Console.WriteLine("Sent message to server.");

            // Receive a response from the server
            while (true) {
                byte[] buffer = new byte[1024];
                int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                _serverResponse = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Received response from server: " + _serverResponse);
            }
        } catch (Exception ex) {
            Console.WriteLine("An error occurred: " + ex.Message);
        } finally {
            // Clean up the client connection
            _stream?.Close();
            _client?.Close();
            _isConnected = false;
            Console.WriteLine("Disconnected from server.");
        }
    }

}