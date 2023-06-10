using System.Text;
using TTG_Shared.Models;

namespace TTG_Shared.Packets; 

public class CreateRoomPacket : Packet {

    public new const byte PacketType = 0x3;

    public readonly string Nickname;
    public readonly string Name;
    public readonly ushort MaxPlayers;
    public readonly ushort MaxTraitors;

    public CreateRoomPacket(string nickname, string name, ushort maxPlayers, ushort maxTraitors) {
        this.Nickname = nickname;
        this.Name = name;
        this.MaxPlayers = maxPlayers;
        this.MaxTraitors = maxTraitors;
    }

    public override byte[] ToBytes() {
        var nicknameBytes = Encoding.UTF8.GetBytes(this.Nickname);
        var nameBytes = Encoding.UTF8.GetBytes(this.Name);
        var packetSize = sizeof(byte) + sizeof(ushort) + sizeof(ushort) + sizeof(int) + nicknameBytes.Length + nameBytes.Length;
        var packetBytes = new byte[packetSize];

        packetBytes[0] = PacketType;
        var maxPlayersBytes = BitConverter.GetBytes(this.MaxPlayers);
        Array.Copy(maxPlayersBytes, 0, packetBytes, 1, maxPlayersBytes.Length);
        
        var maxTraitorsBytes = BitConverter.GetBytes(this.MaxTraitors);
        Array.Copy(maxTraitorsBytes, 0, packetBytes, maxPlayersBytes.Length + 1, maxTraitorsBytes.Length);

        var nicknameLengthBytes = BitConverter.GetBytes(this.Nickname.Length);
        Array.Copy(nicknameLengthBytes, 0, packetBytes, maxPlayersBytes.Length + maxTraitorsBytes.Length + 1, nicknameLengthBytes.Length);
        Array.Copy(nicknameBytes, 0, packetBytes, maxPlayersBytes.Length + maxTraitorsBytes.Length + nicknameLengthBytes.Length + 1, nicknameBytes.Length);
        Array.Copy(nameBytes, 0, packetBytes, maxPlayersBytes.Length + maxTraitorsBytes.Length + nicknameLengthBytes.Length + nicknameBytes.Length + 1, nameBytes.Length);

        return packetBytes;
    }
    
    public new static CreateRoomPacket FromBytes(byte[] packetBytes) {
        var maxPlayers = BitConverter.ToUInt16(packetBytes, 1);
        var maxTraitors = BitConverter.ToUInt16(packetBytes, sizeof(ushort) + 1);
        var nicknameLength = BitConverter.ToInt32(packetBytes, sizeof(ushort) + sizeof(ushort) + 1);
        var nickname = Encoding.UTF8.GetString(packetBytes, sizeof(ushort) + sizeof(ushort) + sizeof(int) + 1, nicknameLength);
        var name = Encoding.UTF8.GetString(packetBytes, sizeof(ushort) + sizeof(ushort) + sizeof(int) + nicknameLength + 1, packetBytes.Length - (sizeof(ushort) + sizeof(ushort) + sizeof(int) + nicknameLength + 1));

        return new CreateRoomPacket(nickname, name, maxPlayers, maxTraitors);
    }

}