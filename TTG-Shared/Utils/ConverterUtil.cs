using System.Net;
using System.Net.Sockets;

namespace TTG_Shared.Utils; 

public static class ConverterUtil {

    public static long ConvertIpToBigEndian(string ip) => ConvertIpToBigEndian(IPAddress.Parse(ip));

    /// <summary>
    /// Convert the IP in string to Big-Endian long.
    /// </summary>
    /// <param name="ip">The IP to be converted.</param>
    /// <returns>The IP in Big-Endian long.</returns>
    public static long ConvertIpToBigEndian(IPAddress ip) => ip.AddressFamily switch {
        AddressFamily.InterNetwork => BitConverter.ToUInt32(ip.GetAddressBytes(), 0),
        AddressFamily.InterNetworkV6 => BitConverter.ToInt64(ip.GetAddressBytes(), 0),
        _ => throw new ArgumentException("Unsupported IP address format.")
    };

}