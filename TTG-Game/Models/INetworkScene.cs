using TTG_Shared.Models;

namespace TTG_Game.Models; 

public interface INetworkScene {

    public void PacketReceivedCallback(Packet packet);

}