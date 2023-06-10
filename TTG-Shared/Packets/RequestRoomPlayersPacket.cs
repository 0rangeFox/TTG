using TTG_Shared.Models;

namespace TTG_Shared.Packets; 

public class RequestRoomPlayersPacket : Packet {

    public new const byte PacketType = 0x10;

    public override byte[] ToBytes() => new[] { PacketType };

    public new static RequestRoomPlayersPacket FromBytes(byte[] packetBytes) => new();

}