using TTG_Game.Managers;
using TTG_Game.Models.Graphics;

namespace TTG_Game.Models; 

public class Entity : Sprite, IEntity {

    public Texture2D ActionTexture { get; set; } = TextureManager.Empty;

    public bool Highlight {
        get => this.IsHighlighted;
        set => this.IsHighlighted = value;
    }

    public Entity(Texture2D texture) : base(texture) {
        TTGGame.Instance.Entities.Add(this);
    }

    ~Entity() {
        TTGGame.Instance.Entities.Remove(this);
    }

}