using System.Drawing;
using System.Text;
using TTG_Shared.Models;

namespace TTG_Shared.Packets; 

public class CreatedRoomPacket : Packet {

    public new const byte PacketType = 0x4;

    public readonly bool Created;
    public readonly string Message;
    public readonly Guid? ID;
    public readonly Color? Color;

    public CreatedRoomPacket(bool created, string message, Guid? id = null, Color? color = null) {
        this.Created = created;
        this.Message = message;
        this.ID = id;
        this.Color = color;
    }

    public override byte[] ToBytes() {
        var messageBytes = Encoding.UTF8.GetBytes(this.Message);
        var idBytes = this.ID?.ToByteArray() ?? Array.Empty<byte>();

        var packetSize = sizeof(byte) + sizeof(bool) + sizeof(int) + messageBytes.Length + idBytes.Length + sizeof(int);
        var packetBytes = new byte[packetSize];

        packetBytes[0] = PacketType;
        var createdBytes = BitConverter.GetBytes(this.Created);
        Array.Copy(createdBytes, 0, packetBytes, 1, createdBytes.Length);

        var messageSizeBytes = BitConverter.GetBytes(messageBytes.Length);
        Array.Copy(messageSizeBytes, 0, packetBytes, createdBytes.Length + 1, messageSizeBytes.Length);
        Array.Copy(messageBytes, 0, packetBytes, createdBytes.Length + messageSizeBytes.Length + 1, messageBytes.Length);

        if (!this.Created) return packetBytes;

        var colorARGBBytes = BitConverter.GetBytes(((Color) this.Color).ToArgb());
        Array.Copy(colorARGBBytes, 0, packetBytes, createdBytes.Length + messageSizeBytes.Length + messageBytes.Length + 1, colorARGBBytes.Length);

        Array.Copy(idBytes, 0, packetBytes, createdBytes.Length + messageSizeBytes.Length + messageBytes.Length + colorARGBBytes.Length + 1, idBytes.Length);

        return packetBytes;
    }

    public new static CreatedRoomPacket FromBytes(byte[] packetBytes) {
        var created = BitConverter.ToBoolean(packetBytes, 1);
        var messageLength = BitConverter.ToInt32(packetBytes, sizeof(bool) + 1);
        var message = Encoding.UTF8.GetString(packetBytes, sizeof(bool) + sizeof(int) + 1, messageLength);

        if (!created)
            return new CreatedRoomPacket(created, message);

        var colorARGB = BitConverter.ToInt32(packetBytes, sizeof(byte) + sizeof(bool) + sizeof(int) + messageLength);

        var id = new byte[packetBytes.Length - (sizeof(byte) + sizeof(bool) + sizeof(int) + messageLength + sizeof(int))];
        Array.Copy(packetBytes, sizeof(byte) + sizeof(bool) + sizeof(int) + messageLength + sizeof(int), id, 0, id.Length);

        return new CreatedRoomPacket(created, message, new Guid(id), System.Drawing.Color.FromArgb(colorARGB));
    }

}