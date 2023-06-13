using System.Net.Sockets;
using System.Text;
using TTG_Shared.Models;

namespace TTG_Shared.Packets; 

public class ExecuteActionPacket : Packet {

    public new const byte PacketType = 0x17;

    public readonly Actions Action;
    public readonly Guid? To;

    public ExecuteActionPacket(Actions action, Guid? to = null) : base(ProtocolType.Udp) {
        this.Action = action;
        this.To = to;
    }

    public override byte[] ToBytes() {
        var toBytes = this.To?.ToByteArray() ?? Array.Empty<byte>();
        var packetBytes = new byte[toBytes.Length + 2];

        packetBytes[0] = PacketType;
        packetBytes[1] = (byte) this.Action;

        if (this.To == null) return packetBytes;

        Array.Copy(toBytes, 0, packetBytes, 2, toBytes.Length);

        return packetBytes;
    }

    public new static ExecuteActionPacket FromBytes(byte[] packetBytes) {
        var to = new byte[packetBytes.Length - 2];
        Array.Copy(packetBytes, 2, to, 0, to.Length);

        return new ExecuteActionPacket((Actions) packetBytes[1], to.Length > 0 ? new Guid(to) : null);
    }

}