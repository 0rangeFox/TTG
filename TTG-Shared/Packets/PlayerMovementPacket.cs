using System.Numerics;
using TTG_Shared.Models;

namespace TTG_Shared.Packets; 

public class PlayerMovementPacket : Packet {

    public new const byte PacketType = 0x14;

    public readonly Guid? ID;
    public readonly Vector2 Position;
    public readonly ushort Texture;
    public readonly bool Direction;

    public PlayerMovementPacket(Vector2 position, ushort texture, bool direction, Guid? id = null) {
        this.ID = id;
        this.Position = position;
        this.Texture = texture;
        this.Direction = direction;
    }

    public override byte[] ToBytes() {
        var idBytes = this.ID?.ToByteArray() ?? Array.Empty<byte>();
        var packetSize = sizeof(byte) + sizeof(float) + sizeof(float) + sizeof(ushort) + sizeof(bool) + idBytes.Length;
        var packetBytes = new byte[packetSize];

        packetBytes[0] = PacketType;

        var positionXBytes = BitConverter.GetBytes(this.Position.X);
        Array.Copy(positionXBytes, 0, packetBytes, 1, positionXBytes.Length);

        var positionYBytes = BitConverter.GetBytes(this.Position.Y);
        Array.Copy(positionYBytes, 0, packetBytes, sizeof(float) + 1, positionYBytes.Length);

        var textureBytes = BitConverter.GetBytes(this.Texture);
        Array.Copy(textureBytes, 0, packetBytes, sizeof(float) + sizeof(float) + 1, textureBytes.Length);

        var directionBytes = BitConverter.GetBytes(this.Direction);
        Array.Copy(directionBytes, 0, packetBytes, sizeof(float) + sizeof(float) + sizeof(ushort) + 1, directionBytes.Length);

        if (this.ID == null) return packetBytes;

        Array.Copy(idBytes, 0, packetBytes, sizeof(float) + sizeof(float) + sizeof(ushort) + sizeof(bool) + 1, idBytes.Length);

        return packetBytes;
    }

    public new static PlayerMovementPacket FromBytes(byte[] packetBytes) {
        var positionX = BitConverter.ToSingle(packetBytes, 1);
        var positionY = BitConverter.ToSingle(packetBytes, sizeof(float) + 1);
        var texture = BitConverter.ToUInt16(packetBytes, sizeof(float) + sizeof(float) + 1);
        var direction = BitConverter.ToBoolean(packetBytes, sizeof(float) + sizeof(float) + sizeof(ushort) + 1);

        var id = new byte[packetBytes.Length - (sizeof(byte) + sizeof(float) + sizeof(float) + sizeof(ushort) + sizeof(bool))];
        Array.Copy(packetBytes, sizeof(byte) + sizeof(float) + sizeof(float) + sizeof(ushort) + sizeof(bool), id, 0, id.Length);

        return new PlayerMovementPacket(new Vector2(positionX, positionY), texture, direction, id.Length > 0 ? new Guid(id) : null);
    }

}