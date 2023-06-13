using System.Drawing;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using TTG_Shared.Models;

namespace TTG_Shared.Packets; 

public class ConnectRoomPacket : Packet {

    public new const byte PacketType = 0x12;

    public readonly Guid ID;
    public readonly string Nickname;
    public readonly Color Color;
    public readonly Vector2 Position;

    public ConnectRoomPacket(Guid id, string nickname, Color color, Vector2 position) : base(ProtocolType.Udp) {
        this.ID = id;
        this.Nickname = nickname;
        this.Color = color;
        this.Position = position;
    }

    public override byte[] ToBytes() {
        var nicknameBytes = Encoding.UTF8.GetBytes(this.Nickname);
        var idBytes = this.ID.ToByteArray();

        var packetSize = sizeof(byte) + sizeof(int) + sizeof(float) + sizeof(float) + sizeof(int) + nicknameBytes.Length + idBytes.Length;
        var packetBytes = new byte[packetSize];

        packetBytes[0] = PacketType;

        var colorBytes = BitConverter.GetBytes(this.Color.ToArgb());
        Array.Copy(colorBytes, 0, packetBytes, 1, colorBytes.Length);

        var positionXBytes = BitConverter.GetBytes(this.Position.X);
        Array.Copy(positionXBytes, 0, packetBytes, sizeof(int) + 1, positionXBytes.Length);
        
        var positionYBytes = BitConverter.GetBytes(this.Position.X);
        Array.Copy(positionYBytes, 0, packetBytes, sizeof(int) + sizeof(float) + 1, positionYBytes.Length);

        var nicknameLengthBytes = BitConverter.GetBytes(this.Nickname.Length);
        Array.Copy(nicknameLengthBytes, 0, packetBytes, sizeof(int) + sizeof(float) + sizeof(float) + 1, nicknameLengthBytes.Length);
        Array.Copy(nicknameBytes, 0, packetBytes, sizeof(int) + sizeof(float) + sizeof(float) + sizeof(int) + 1, nicknameBytes.Length);
        Array.Copy(idBytes, 0, packetBytes, sizeof(int) + sizeof(float) + sizeof(float) + sizeof(int) + nicknameBytes.Length + 1, idBytes.Length);

        return packetBytes;
    }

    public new static ConnectRoomPacket FromBytes(byte[] packetBytes) {
        var colorARGB = BitConverter.ToInt32(packetBytes, 1);
        var positionX = BitConverter.ToSingle(packetBytes, sizeof(int) + 1);
        var positionY = BitConverter.ToSingle(packetBytes, sizeof(int) + sizeof(float) + 1);
        var nicknameLength = BitConverter.ToInt32(packetBytes, sizeof(int) + sizeof(float) + sizeof(float) + 1);
        var nickname = Encoding.UTF8.GetString(packetBytes, sizeof(int) + sizeof(float) + sizeof(float) + sizeof(int) + 1, nicknameLength);

        var id = new byte[packetBytes.Length - (sizeof(int) + sizeof(float) + sizeof(float) + sizeof(int) + nicknameLength + 1)];
        Array.Copy(packetBytes, sizeof(int) + sizeof(float) + sizeof(float) + sizeof(int) + nicknameLength + 1, id, 0, id.Length);

        return new ConnectRoomPacket(new Guid(id), nickname, System.Drawing.Color.FromArgb(colorARGB), new Vector2(positionX, positionY));
    }

}