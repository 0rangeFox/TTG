using System.Collections.Generic;
using TTG_Game.Models.Graphics;
using XnaEffect = Microsoft.Xna.Framework.Graphics.Effect;
using XnaTexture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

namespace TTG_Game.Managers; 

public class TextureManager {

    public static Texture2D Empty = null!;

    private readonly Dictionary<Texture, Texture2D> _textures = new();
    private readonly Dictionary<Effect, XnaEffect> _effects = new();

    private void LoadTexture(Texture id, XnaTexture2D texture) => this._textures.Add(id, new Texture2D(id, texture));

    public void Load() {
        var game = TTGGame.Instance;

        Empty = new Texture2D(TTGGame.Instance.GraphicsDeviceManager.GraphicsDevice, Texture.Empty, 1, 1);
        this.LoadTexture(Texture.Logo, game.Load<XnaTexture2D>("Images/Logo"));
        this.LoadTexture(Texture.Background, game.Load<XnaTexture2D>("Images/Background"));
        this.LoadTexture(Texture.RevealRole, game.Load<XnaTexture2D>("Images/RevealRole"));

        this.LoadTexture(Texture.Button, game.Load<XnaTexture2D>("Controls/Button"));
        this.LoadTexture(Texture.TextField, game.Load<XnaTexture2D>("Controls/TextField"));
        this.LoadTexture(Texture.ArrowRight, game.Load<XnaTexture2D>("Controls/Arrow-Right"));

        this.LoadTexture(Texture.CharacterIdle, game.Load<XnaTexture2D>("Images/Character/0"));
        this.LoadTexture(Texture.CharacterWalk0, game.Load<XnaTexture2D>("Images/Character/1"));
        this.LoadTexture(Texture.CharacterWalk1, game.Load<XnaTexture2D>("Images/Character/2"));
        this.LoadTexture(Texture.CharacterWalk2, game.Load<XnaTexture2D>("Images/Character/3"));
        this.LoadTexture(Texture.CharacterWalk3, game.Load<XnaTexture2D>("Images/Character/4"));
        this.LoadTexture(Texture.CharacterWalk4, game.Load<XnaTexture2D>("Images/Character/5"));
        this.LoadTexture(Texture.CharacterDead, game.Load<XnaTexture2D>("Images/Character/6"));

        this.LoadTexture(Texture.Quit, game.Load<XnaTexture2D>("Images/Actions/Quit"));
        this.LoadTexture(Texture.Start, game.Load<XnaTexture2D>("Images/Actions/Start"));
        this.LoadTexture(Texture.Report, game.Load<XnaTexture2D>("Images/Actions/Report"));

        this._effects.Add(Effect.Highlight, game.Load<XnaEffect>("Shaders/Highlight"));
    }

    public Texture2D GetTexture(Texture id) => this._textures[id].Clone();

    public XnaEffect GetEffect(Effect id) => this._effects[id].Clone();

}