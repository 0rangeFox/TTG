using System.Drawing;
using System.Numerics;

namespace TTG_Server.Models; 

public class Player {

    public Client Client;
    public string Nickname;
    public Color Color;
    public Vector2 Position;

    public Player(Client client, string nickname, Color? color = null, Vector2? position = null) {
        this.Client = client;
        this.Nickname = nickname;
        this.Color = color ?? Color.Empty;
        this.Position = position ?? Vector2.Zero;
    }

}