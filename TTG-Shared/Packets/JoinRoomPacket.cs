using System.Text;
using TTG_Shared.Models;

namespace TTG_Shared.Packets; 

public class JoinRoomPacket : Packet {

    public new const byte PacketType = 0x8;

    public readonly Guid ID;
    public readonly string Nickname;

    public JoinRoomPacket(Guid id, string nickname) {
        this.ID = id;
        this.Nickname = nickname;
    }

    public override byte[] ToBytes() {
        var nicknameBytes = Encoding.UTF8.GetBytes(this.Nickname);
        var idBytes = this.ID.ToByteArray();

        var packetSize = sizeof(byte) + sizeof(int) + nicknameBytes.Length + idBytes.Length;
        var packetBytes = new byte[packetSize];

        packetBytes[0] = PacketType;
        var nicknameLengthBytes = BitConverter.GetBytes(this.Nickname.Length);
        Array.Copy(nicknameLengthBytes, 0, packetBytes, 1, nicknameLengthBytes.Length);
        Array.Copy(nicknameBytes, 0, packetBytes, sizeof(int) + 1, nicknameBytes.Length);
        Array.Copy(idBytes, 0, packetBytes, sizeof(int) + nicknameBytes.Length + 1, idBytes.Length);

        return packetBytes;
    }

    public new static JoinRoomPacket FromBytes(byte[] packetBytes) {
        var nicknameLength = BitConverter.ToInt32(packetBytes, 1);
        var nickname = Encoding.UTF8.GetString(packetBytes, sizeof(int) + 1, nicknameLength);

        var id = new byte[packetBytes.Length - (sizeof(int) + nicknameLength + 1)];
        Array.Copy(packetBytes, sizeof(int) + nicknameLength + 1, id, 0, id.Length);

        return new JoinRoomPacket(new Guid(id), nickname);
    }

}