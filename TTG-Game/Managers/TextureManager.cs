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

    public Texture2D CharacterIdle => this._characterIdle.Clone();
    public List<Texture2D> CharacterWalk => this._characterWalk.Select(texture => texture.Clone()).ToList();
    public Texture2D CharacterDead => this._characterDead.Clone();

    public void Load() {
        Empty = new Texture2D(TTGGame.Instance.GraphicsDeviceManager.GraphicsDevice, 1, 1);

        this._characterIdle = TTGGame.Instance.Load<Texture2D>("Images/Character/0");
        this._characterWalk = Enumerable.Range(1, 5).Select(n => TTGGame.Instance.Load<Texture2D>($"Images/Character/{n}")).ToList();
        this._characterDead = TTGGame.Instance.Load<Texture2D>("Images/Character/6");
    }

}