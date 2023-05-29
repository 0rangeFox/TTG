using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TTG_Game.Controls; 

public class TextField : Button {

    private readonly GameWindow _gw = TTGGame.Instance.Game.Window;

    private bool _isFocusing = false;
    private readonly StringBuilder _text = new();

    public event EventHandler Change;

    public new string String {
        get => base.String;
        set {
            this._text.Clear();
            this._text.Append(value);
            base.String = value;
        }
    }

    public TextField(string text) : this(TTGGame.Instance.TextureManager.TextField, text) {}

    public TextField(Texture2D texture, string text) : base(texture, text) {
        this.Click += CheckClickOnMyBox;
    }

    private void RegisterFocusedButtonForTextInput(EventHandler<TextInputEventArgs> method) => this._gw.TextInput += method;

    private void UnRegisterFocusedButtonForTextInput(EventHandler<TextInputEventArgs> method) {
        this._gw.TextInput -= method;
        this.Change?.Invoke(this, EventArgs.Empty);
    }

    private void OnInput(object? sender, TextInputEventArgs e) {
        if (e.Key == Keys.Back) {
            if (this._text.Length > 0)
                this._text.Length--;
        } else this._text.Append(e.Character);
        this.String = this._text.ToString();
    }

    private void CheckClickOnMyBox(object? sender, EventArgs e) {
        if (!this.Clicked) return;
        if (this._isFocusing = !this._isFocusing) RegisterFocusedButtonForTextInput(OnInput);
        else UnRegisterFocusedButtonForTextInput(OnInput);
    }

}