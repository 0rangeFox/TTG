namespace TTG_Game.Models; 

public class GameConfiguration {

    public NetworkConfiguration Network { get; set; }
    public string Nickname { get; set; }

    public class NetworkConfiguration {

        public string IP { get; set; }
        public ushort Port { get; set; }

    }

}