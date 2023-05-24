using System;

namespace TTG_Game.Models;

public interface IEntity {

    public bool Highlight { get; set; }

    public void ExecuteAction() {
        switch (this) {
            case Player player:
                Console.WriteLine("Player");
                break;
            case Entity entity:
                Console.WriteLine("Entity");
                break;
        }
    }

}