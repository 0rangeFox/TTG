using System.Reflection;

namespace TTG_Shared.Models;

public abstract class Packet {

    private static readonly IEnumerable<Type> PacketTypes = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(assembly => assembly.GetTypes())
        .Where(type => !type.IsAbstract && typeof(Packet).IsAssignableFrom(type));

    public const byte PacketType = 0x0;

    public virtual byte[] ToBytes() => throw new NotImplementedException();

    public static Packet FromBytes(byte[] packetBytes) {
        foreach (var type in PacketTypes) {
            var packetTypeField = type.GetField("PacketType", BindingFlags.Public | BindingFlags.Static);

            if (packetTypeField == null || (byte) packetTypeField.GetValue(null)! != packetBytes[0]) continue;
            var parseMethod = type.GetMethod("FromBytes", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(byte[]) }, null);
            if (parseMethod == null) continue;

            return (Packet) parseMethod.Invoke(null, new object[] { packetBytes })!;
        }

        throw new NotImplementedException();
    }

}
