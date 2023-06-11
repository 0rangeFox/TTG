using System.Collections.Generic;
using TTG_Game.Models.Graphics;

namespace TTG_Game.Models; 

public class AnimatedEntity : AnimatedSprite, IEntity {

    public bool Highlight {
        get => this.IsHighlighted;
        set => this.IsHighlighted = value;
    }

    public AnimatedEntity(List<Texture2D> textures) : base(textures) {
        TTGGame.Instance.Entities.Add(this);
    }

    ~AnimatedEntity() {
        TTGGame.Instance.Entities.Remove(this);
    }

}