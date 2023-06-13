using System.Net.Sockets;
using TTG_Shared.Models;

namespace TTG_Shared.Packets; 

public class DisconnectRoomPacket : Packet {

    public new const byte PacketType = 0x13;

    public readonly Guid ID;

    public DisconnectRoomPacket(Guid id) : base(ProtocolType.Udp) {
        this.ID = id;
    }

    public override byte[] ToBytes() {
        var idBytes = this.ID.ToByteArray();
        
        var packetSize = sizeof(byte) + idBytes.Length;
        var packetBytes = new byte[packetSize];
        
        packetBytes[0] = PacketType;
        Array.Copy(idBytes, 0, packetBytes, 1, idBytes.Length);

        return packetBytes;
    }

    public new static DisconnectRoomPacket FromBytes(byte[] packetBytes) {
        var id = new byte[packetBytes.Length - 1];
        Array.Copy(packetBytes, 1, id, 0, id.Length);

        return new DisconnectRoomPacket(new Guid(id));
    }

}