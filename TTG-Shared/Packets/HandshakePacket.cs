using TTG_Shared.Models;

namespace TTG_Shared.Packets; 

public class HandshakePacket : Packet {

    public new const byte PacketType = 0x1;

    public DateTime Time;
    public Guid Code;

    public HandshakePacket(DateTime time, Guid code) {
        this.Time = time;
        this.Code = code;
    }

    public override byte[] ToBytes() {
        var codeBytes = this.Code.ToByteArray();

        var packetSize = sizeof(byte) + sizeof(long) + codeBytes.Length;
        var packetBytes = new byte[packetSize];

        packetBytes[0] = PacketType;
        var ticksBytes = BitConverter.GetBytes(this.Time.Ticks);
        Array.Copy(ticksBytes, 0, packetBytes, 1, ticksBytes.Length);
        Array.Copy(codeBytes, 0, packetBytes, ticksBytes.Length + 1, codeBytes.Length);

        return packetBytes;
    }

    public new static HandshakePacket FromBytes(byte[] packetBytes) {
        var ticks = BitConverter.ToInt64(packetBytes, 1);
        var code = new byte[packetBytes.Length - (sizeof(byte) + sizeof(long))];
        Array.Copy(packetBytes, sizeof(byte) + sizeof(long), code, 0, code.Length);

        return new HandshakePacket(new DateTime(ticks), new Guid(code));
    }

}