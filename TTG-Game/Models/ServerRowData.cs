using System;
using TTG_Shared.Packets;

namespace TTG_Game.Models; 

public class ServerRowData {

    public Guid ID { get; }
    public string Text { get; }
    public ushort Players { get; }
    public ushort MaxPlayers { get; }
    public ushort MaxTraitors { get; }

    public ServerRowData(AddRoomPacket packet) : this(packet.ID, packet.Name, 1, packet.MaxPlayers, packet.MaxTraitors) {}
    
    public ServerRowData(ServerRowData serverRowData) : this(serverRowData.ID, serverRowData.Text, serverRowData.Players, serverRowData.MaxPlayers, serverRowData.MaxTraitors) {}

    public ServerRowData(Guid id, string text, ushort players, ushort maxPlayers, ushort maxTraitors) {
        this.ID = id;
        this.Text = text;
        this.Players = players;
        this.MaxPlayers = maxPlayers;
        this.MaxTraitors = maxTraitors;
    }

}
