using System.Text;
using TTG_Shared.Models;

namespace TTG_Shared.Packets; 

public class AddRoomPacket : Packet {

    public new const byte PacketType = 0x6;

    public readonly Guid ID;
    public readonly string Name;
    public readonly ushort MaxPlayers;
    public readonly ushort MaxTraitors;

    public AddRoomPacket(Guid id, string name, ushort maxPlayers, ushort maxTraitors) {
        this.ID = id;
        this.Name = name;
        this.MaxPlayers = maxPlayers;
        this.MaxTraitors = maxTraitors;
    }

    public override byte[] ToBytes() {
        var nameBytes = Encoding.UTF8.GetBytes(this.Name);
        var idBytes = this.ID.ToByteArray();

        var packetSize = sizeof(byte) + sizeof(ushort) + sizeof(ushort) + sizeof(int) + nameBytes.Length + idBytes.Length;
        var packetBytes = new byte[packetSize];

        packetBytes[0] = PacketType;
        var maxPlayersBytes = BitConverter.GetBytes(this.MaxPlayers);
        Array.Copy(maxPlayersBytes, 0, packetBytes, 1, maxPlayersBytes.Length);

        var maxTraitorsBytes = BitConverter.GetBytes(this.MaxTraitors);
        Array.Copy(maxTraitorsBytes, 0, packetBytes, maxPlayersBytes.Length + 1, maxTraitorsBytes.Length);

        var nameSizeBytes = BitConverter.GetBytes(nameBytes.Length);
        Array.Copy(nameSizeBytes, 0, packetBytes, maxPlayersBytes.Length + maxTraitorsBytes.Length + 1, nameSizeBytes.Length);
        Array.Copy(nameBytes, 0, packetBytes, maxPlayersBytes.Length + maxTraitorsBytes.Length + nameSizeBytes.Length + 1, nameBytes.Length);
        Array.Copy(idBytes, 0, packetBytes, maxPlayersBytes.Length + maxTraitorsBytes.Length + nameSizeBytes.Length + nameBytes.Length + 1, idBytes.Length);

        return packetBytes;
    }

    public new static AddRoomPacket FromBytes(byte[] packetBytes) {
        var maxPlayers = BitConverter.ToUInt16(packetBytes, 1);
        var maxTraitors = BitConverter.ToUInt16(packetBytes,  sizeof(ushort) + 1);
        var nameLength = BitConverter.ToInt32(packetBytes, sizeof(ushort) + sizeof(ushort) + 1);
        var name = Encoding.UTF8.GetString(packetBytes, sizeof(ushort) + sizeof(ushort) + sizeof(int) + 1, nameLength);

        var id = new byte[packetBytes.Length - (sizeof(ushort) + sizeof(ushort) + sizeof(int) + nameLength + 1)];
        Array.Copy(packetBytes, sizeof(ushort) + sizeof(ushort) + sizeof(int) + nameLength + 1, id, 0, id.Length);

        return new AddRoomPacket(new Guid(id), name, maxPlayers, maxTraitors);
    }

}