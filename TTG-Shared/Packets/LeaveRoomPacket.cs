using System.Net.Sockets;
using TTG_Shared.Models;

namespace TTG_Shared.Packets; 

public class LeaveRoomPacket : Packet {

    public new const byte PacketType = 0x11;

    public LeaveRoomPacket() : base(ProtocolType.Udp) {}

    public override byte[] ToBytes() => new[] { PacketType };

    public new static LeaveRoomPacket FromBytes(byte[] packetBytes) => new();

}