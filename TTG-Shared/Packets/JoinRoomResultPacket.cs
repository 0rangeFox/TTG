using System.Drawing;
using System.Text;
using TTG_Shared.Models;

namespace TTG_Shared.Packets; 

public class JoinRoomResultPacket : Packet {

    public new const byte PacketType = 0x9;

    public readonly bool Joined;
    public readonly string Message;
    public readonly Guid? ID;
    public readonly ushort? MaxPlayers;
    public readonly Color? Color;

    public JoinRoomResultPacket(bool joined, string message, Guid? id = null, ushort? maxPlayers = null, Color? color = null) {
        this.Joined = joined;
        this.Message = message;
        this.ID = id;
        this.MaxPlayers = maxPlayers;
        this.Color = color;
    }

    public override byte[] ToBytes() {
        var messageBytes = Encoding.UTF8.GetBytes(this.Message);
        var idBytes = this.ID?.ToByteArray() ?? Array.Empty<byte>();

        var packetSize = sizeof(byte) + sizeof(bool) + sizeof(int) + messageBytes.Length + (this.Joined ? sizeof(ushort) + sizeof(int) + idBytes.Length : 0);
        var packetBytes = new byte[packetSize];

        packetBytes[0] = PacketType;
        var joinedBytes = BitConverter.GetBytes(this.Joined);
        Array.Copy(joinedBytes, 0, packetBytes, 1, joinedBytes.Length);

        var messageSizeBytes = BitConverter.GetBytes(messageBytes.Length);
        Array.Copy(messageSizeBytes, 0, packetBytes, joinedBytes.Length + 1, messageSizeBytes.Length);
        Array.Copy(messageBytes, 0, packetBytes, joinedBytes.Length + messageSizeBytes.Length + 1, messageBytes.Length);

        if (!this.Joined) return packetBytes;

        var maxPlayersBytes = BitConverter.GetBytes((ushort) this.MaxPlayers);
        Array.Copy(maxPlayersBytes, 0, packetBytes, joinedBytes.Length + messageSizeBytes.Length + messageBytes.Length + 1, maxPlayersBytes.Length);

        var colorARGBBytes = BitConverter.GetBytes(((Color) this.Color).ToArgb());
        Array.Copy(colorARGBBytes, 0, packetBytes, joinedBytes.Length + messageSizeBytes.Length + messageBytes.Length + sizeof(ushort) + 1, colorARGBBytes.Length);

        Array.Copy(idBytes, 0, packetBytes, joinedBytes.Length + messageSizeBytes.Length + messageBytes.Length + sizeof(ushort) + sizeof(int) + 1, idBytes.Length);

        return packetBytes;
    }

    public new static JoinRoomResultPacket FromBytes(byte[] packetBytes) {
        var joined = BitConverter.ToBoolean(packetBytes, 1);
        var messageLength = BitConverter.ToInt32(packetBytes, sizeof(bool) + 1);
        var message = Encoding.UTF8.GetString(packetBytes, sizeof(bool) + sizeof(int) + 1, messageLength);

        if (!joined)
            return new JoinRoomResultPacket(joined, message);

        var maxPlayers = BitConverter.ToUInt16(packetBytes, sizeof(byte) + sizeof(bool) + sizeof(int) + messageLength);
        var colorARGB = BitConverter.ToInt32(packetBytes, sizeof(byte) + sizeof(bool) + sizeof(int) + messageLength + sizeof(ushort));
        var id = new byte[packetBytes.Length - (sizeof(byte) + sizeof(bool) + sizeof(int) + messageLength + sizeof(ushort) + sizeof(int))];
        Array.Copy(packetBytes, sizeof(byte) + sizeof(bool) + sizeof(int) + messageLength + sizeof(ushort) + sizeof(int), id, 0, id.Length);

        return new JoinRoomResultPacket(joined, message, new Guid(id), maxPlayers, System.Drawing.Color.FromArgb(colorARGB));
    }

}