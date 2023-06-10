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

    private Texture2D _quit;
    private Texture2D _start;
    private Texture2D _report;

    private Effect _highlightEffect;

    public Texture2D Button { get; private set; }
    public Texture2D TextField { get; private set; }
    public Texture2D ArrowRight { get; private set; }

    public Texture2D CharacterIdle => this._characterIdle.Clone();
    public List<Texture2D> CharacterWalk => this._characterWalk.Select(texture => texture.Clone()).ToList();
    public Texture2D CharacterDead => this._characterDead.Clone();

    public Texture2D Quit => this._quit.Clone();
    public Texture2D Start => this._start.Clone();
    public Texture2D Report => this._report.Clone();

    public Effect HighlightEffect => this._highlightEffect.Clone();

    public void Load() {
        var game = TTGGame.Instance;

        Empty = new Texture2D(game.GraphicsDeviceManager.GraphicsDevice, 1, 1);

        this.Button = game.Load<Texture2D>("Controls/Button");
        this.TextField = game.Load<Texture2D>("Controls/TextField");
        this.ArrowRight = game.Load<Texture2D>("Controls/Arrow-Right");

        this._characterIdle = game.Load<Texture2D>("Images/Character/0");
        this._characterWalk = Enumerable.Range(1, 5).Select(n => game.Load<Texture2D>($"Images/Character/{n}")).ToList();
        this._characterDead = game.Load<Texture2D>("Images/Character/6");

        this._quit = game.Load<Texture2D>("Images/Actions/Quit");
        this._start = game.Load<Texture2D>("Images/Actions/Start");
        this._report = game.Load<Texture2D>("Images/Actions/Report");

        this._highlightEffect = game.Load<Effect>("Shaders/Highlight");
    }

}