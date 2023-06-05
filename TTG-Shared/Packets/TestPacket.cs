using TTG_Shared.Models;

namespace TTG_Shared.Packets; 

public class TestPacket : Packet {

    public new const byte PacketType = 0x2;

    public override byte[] ToBytes() {
        var packetSize = sizeof(byte);
        var packetBytes = new byte[packetSize];

        packetBytes[0] = PacketType;

        return packetBytes;
    }

    public new static TestPacket FromBytes(byte[] packetBytes) => new();

}