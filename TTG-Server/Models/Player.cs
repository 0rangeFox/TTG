using System.Drawing;

namespace TTG_Server.Models; 

public class Player {

    public Client Client;
    public string Nickname;
    public Color Color;

    public Player(Client client, string nickname, Color color) {
        this.Client = client;
        this.Nickname = nickname;
        this.Color = color;
    }

}