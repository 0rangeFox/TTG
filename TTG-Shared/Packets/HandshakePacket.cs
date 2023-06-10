using TTG_Shared.Models;

namespace TTG_Shared.Packets; 

public class HandshakePacket : Packet {

    public new const byte PacketType = 0x1;

    public Guid ClientID;
    public DateTime Time;
    public Guid Code;

    public HandshakePacket(Guid clientID, DateTime time, Guid code) {
        this.ClientID = clientID;
        this.Time = time;
        this.Code = code;
    }

    public override byte[] ToBytes() {
        var clientIDBytes = this.ClientID.ToByteArray();
        var codeBytes = this.Code.ToByteArray();

        var packetSize = sizeof(byte) + sizeof(long) + sizeof(int) + clientIDBytes.Length + codeBytes.Length;
        var packetBytes = new byte[packetSize];

        packetBytes[0] = PacketType;
        var ticksBytes = BitConverter.GetBytes(this.Time.Ticks);
        Array.Copy(ticksBytes, 0, packetBytes, 1, ticksBytes.Length);

        var clientIDLengthBytes = BitConverter.GetBytes(clientIDBytes.Length);
        Array.Copy(clientIDLengthBytes, 0, packetBytes, ticksBytes.Length + 1, clientIDLengthBytes.Length);
        Array.Copy(clientIDBytes, 0, packetBytes, ticksBytes.Length + clientIDLengthBytes.Length + 1, clientIDBytes.Length);
        Array.Copy(codeBytes, 0, packetBytes, ticksBytes.Length + clientIDLengthBytes.Length + clientIDBytes.Length + 1, codeBytes.Length);

        return packetBytes;
    }

    public new static HandshakePacket FromBytes(byte[] packetBytes) {
        var ticks = BitConverter.ToInt64(packetBytes, 1);
        var clientIDLength = BitConverter.ToInt32(packetBytes, sizeof(long) + 1);

        var clientID = new byte[clientIDLength];
        Array.Copy(packetBytes, sizeof(byte) + sizeof(long) + sizeof(int), clientID, 0, clientID.Length);

        var code = new byte[packetBytes.Length - (sizeof(byte) + sizeof(long) + sizeof(int) + clientIDLength)];
        Array.Copy(packetBytes, sizeof(byte) + sizeof(long) + sizeof(int) + clientIDLength, code, 0, code.Length);

        return new HandshakePacket(new Guid(clientID), new DateTime(ticks), new Guid(code));
    }

}