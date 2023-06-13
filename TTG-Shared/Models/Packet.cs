using System.Net.Sockets;
using System.Reflection;

namespace TTG_Shared.Models;

public abstract class Packet {

    private delegate Packet PacketAction(byte[] packetBytes);
    private static readonly Dictionary<byte, PacketAction> Packets = new();

    static Packet() {
        var packetTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => !type.IsAbstract && typeof(Packet).IsAssignableFrom(type));

        foreach (var type in packetTypes) {
            var packetTypeField = type.GetField("PacketType", BindingFlags.Public | BindingFlags.Static);
            if (packetTypeField == null) continue;

            var fromBytesMethod = type.GetMethod("FromBytes", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(byte[]) }, null);
            if (fromBytesMethod == null) continue;

            Packets.Add((byte) packetTypeField.GetValue(null)!, (PacketAction) Delegate.CreateDelegate(typeof(PacketAction), fromBytesMethod));
        }
    }

    public const byte PacketType = 0x0;
    public ProtocolType Protocol;

    protected Packet(ProtocolType protocol = ProtocolType.Tcp) {
        this.Protocol = protocol;
    }

    public virtual byte[] ToBytes() => throw new NotImplementedException();

    public static Packet FromBytes(byte[] packetBytes) {
        if (Packets.TryGetValue(packetBytes[0], out var fromBytes))
            return fromBytes.Invoke(packetBytes);
        throw new NotImplementedException();
    }

}