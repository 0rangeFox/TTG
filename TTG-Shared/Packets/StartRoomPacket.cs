using System.Net.Sockets;
using TTG_Shared.Models;

namespace TTG_Shared.Packets; 

public class StartRoomPacket : Packet {

    public new const byte PacketType = 0x15;

    public readonly Roles Role;

    public StartRoomPacket(Roles role = Roles.Citizen) : base(ProtocolType.Udp) {
        this.Role = role;
    }

    public override byte[] ToBytes() {
        var packetBytes = new byte[2];

        packetBytes[0] = PacketType;
        packetBytes[1] = (byte) this.Role;

        return packetBytes;
    }

    public new static StartRoomPacket FromBytes(byte[] packetBytes) => new((Roles) packetBytes[1]);

}