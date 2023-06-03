using System;

namespace TTG_Game.Models; 

public class ServerRowData {

    public Guid ID { get; }
    public string Text { get; }
    public ushort Players { get; }
    public ushort MaxPlayers { get; }
    public ushort MaxImpostors { get; }

    public ServerRowData(ServerRowData serverRowData) : this(serverRowData.ID, serverRowData.Text, serverRowData.Players, serverRowData.MaxPlayers, serverRowData.MaxImpostors) {}

    public ServerRowData(Guid id, string text, ushort players, ushort maxPlayers, ushort maxImpostors) {
        this.ID = id;
        this.Text = text;
        this.Players = players;
        this.MaxPlayers = maxPlayers;
        this.MaxImpostors = maxImpostors;
    }

}
