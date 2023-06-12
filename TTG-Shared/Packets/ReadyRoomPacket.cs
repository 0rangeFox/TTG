using TTG_Shared.Models;

namespace TTG_Shared.Packets; 

public class ReadyRoomPacket : Packet {

    public new const byte PacketType = 0x16;

    public override byte[] ToBytes() => new[] { PacketType };

    public new static ReadyRoomPacket FromBytes(byte[] packetBytes) => new();

}