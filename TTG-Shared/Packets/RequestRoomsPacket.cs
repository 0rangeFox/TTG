using TTG_Shared.Models;

namespace TTG_Shared.Packets; 

public class RequestRoomsPacket : Packet {

    public new const byte PacketType = 0x5;

    public override byte[] ToBytes() => new[] { PacketType };

    public new static RequestRoomsPacket FromBytes(byte[] packetBytes) => new();

}