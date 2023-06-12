using System;
using System.Net.Sockets;
using TTG_Game.Models.Graphics;
using TTG_Shared.Models;
using TTG_Shared.Packets;

namespace TTG_Game.Models;

public interface IEntity {

    public Texture2D ActionTexture { get; set; }
    public bool Highlight { get; set; }

    public void ExecuteAction() {
        switch (this) {
            case Player player:
                switch (player.ActionTexture.ID) {
                    case Texture.Kill:
                        TTGGame.Instance.NetworkManager.SendPacket(ProtocolType.Udp, new ExecuteActionPacket(Actions.Kill, player.ID));
                        break;
                }
                break;
            case Entity entity:
                Console.WriteLine("Entity");
                break;
        }
    }

}