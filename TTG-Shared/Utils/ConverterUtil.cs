using System.Net;

namespace TTG_Shared.Utils; 

public static class ConverterUtil {

    public static long ConvertIpToBigEndian(string ip) => ConvertIpToBigEndian(IPAddress.Parse(ip));

    /// <summary>
    /// Convert the IP in string to Big-Endian long.
    /// </summary>
    /// <param name="ip">The IP to be converted.</param>
    /// <returns>The IP in Big-Endian long.</returns>
    public static long ConvertIpToBigEndian(IPAddress ip) {
        var addressBytes = ip.GetAddressBytes();

        if (BitConverter.IsLittleEndian)
            Array.Reverse(addressBytes);

        return IPAddress.NetworkToHostOrder(BitConverter.ToInt32(addressBytes, 0));
    }

}