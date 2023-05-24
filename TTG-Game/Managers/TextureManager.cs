using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using TTG_Game.Utils.Extensions;

namespace TTG_Game.Managers; 

public class TextureManager {

    public static Texture2D Empty;

    private Texture2D _characterIdle;
    private List<Texture2D> _characterWalk;
    private Texture2D _characterDead;

    private Texture2D _report;

    private Effect _highlightEffect;

    public Texture2D CharacterIdle => this._characterIdle.Clone();
    public List<Texture2D> CharacterWalk => this._characterWalk.Select(texture => texture.Clone()).ToList();
    public Texture2D CharacterDead => this._characterDead.Clone();

    public Texture2D Report => this._report.Clone();

    public Effect HighlightEffect => this._highlightEffect.Clone();

    public void Load() {
        var game = TTGGame.Instance;

        Empty = new Texture2D(game.GraphicsDeviceManager.GraphicsDevice, 1, 1);

        this._characterIdle = game.Load<Texture2D>("Images/Character/0");
        this._characterWalk = Enumerable.Range(1, 5).Select(n => game.Load<Texture2D>($"Images/Character/{n}")).ToList();
        this._characterDead = game.Load<Texture2D>("Images/Character/6");

        this._report = game.Load<Texture2D>("Images/Actions/Report");

        this._highlightEffect = game.Load<Effect>("Shaders/Highlight");
    }

}