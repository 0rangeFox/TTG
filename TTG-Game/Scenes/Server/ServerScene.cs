using System;
using Microsoft.Xna.Framework;
using TTG_Game.Models;

namespace TTG_Game.Scenes.Server; 

public class ServerScene : Scene {

    private SubScene _subScene;

    public ServerScene() {
        this._subScene = new ServerSelectorScene(this.SwitchScene) {
            BackCallback = BackToMainMenu
        };
    }

    private static void BackToMainMenu(object? sender, EventArgs e) => TTGGame.Instance.Scene = new MainScene();

    private void SwitchScene(object? sender, EventArgs e) {
        this._subScene = this._subScene switch {
            ServerSelectorScene => new ServerCreatorScene() { BackCallback = this.SwitchScene },
            ServerCreatorScene => new ServerSelectorScene(this.SwitchScene) { BackCallback = BackToMainMenu },
            _ => this._subScene
        };
    }

    public override void Update(GameTime gameTime) {
        this._subScene.Update(gameTime);
    }

    public override void Draw(GameTime gameTime) {
        this._subScene.Draw(gameTime);
    }

}