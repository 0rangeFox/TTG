using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TTG_Game.Managers;

namespace TTG_Game.Models; 

public class Player : Sprite {

    private string _nickname;
    private Character _character;
    private bool _isNetwork;

    public Player(string nickname, Color color, bool isNetwork = false) : base(TextureManager.Empty) {
        this._nickname = nickname;
        this._character = new Character(color);
        this._isNetwork = isNetwork;

        this.Texture = this._character.Idle;
    }

    public override void Update(GameTime gameTime) {
        if (_isNetwork) return;

        var velocity = new Vector2();
        var speed = 3f;

        if (Keyboard.GetState().IsKeyDown(Keys.W))
            velocity.Y = -speed;
        else if (Keyboard.GetState().IsKeyDown(Keys.S))
            velocity.Y = speed;

        if (Keyboard.GetState().IsKeyDown(Keys.A))
            velocity.X = -speed;
        else if (Keyboard.GetState().IsKeyDown(Keys.D))
            velocity.X = speed;

        this.Position += velocity;
    }

}