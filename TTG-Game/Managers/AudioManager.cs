using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using TTG_Game.Models;

namespace TTG_Game.Managers; 

public class AudioManager {

    private readonly Dictionary<Sound, SoundEffect> _sounds = new();

    private SoundEffectInstance? _backgroundSoundInstance;

    public void Load() {
        var game = TTGGame.Instance;

        this._sounds.Add(Sound.Background, game.Load<SoundEffect>("Sounds/Background"));
        this._sounds.Add(Sound.SpaceShipBackground, game.Load<SoundEffect>("Sounds/SpaceShipBackground"));
        this._sounds.Add(Sound.RoleReveal, game.Load<SoundEffect>("Sounds/RoleReveal"));
        this._sounds.Add(Sound.Vote, game.Load<SoundEffect>("Sounds/Vote"));
        this._sounds.Add(Sound.Footsteps, game.Load<SoundEffect>("Sounds/Footsteps"));
        this._sounds.Add(Sound.BodyReport, game.Load<SoundEffect>("Sounds/BodyReport"));
        this._sounds.Add(Sound.EmergencyMeeting, game.Load<SoundEffect>("Sounds/EmergencyMeeting"));
        this._sounds.Add(Sound.JoinRoom, game.Load<SoundEffect>("Sounds/JoinRoom"));
        this._sounds.Add(Sound.Kill, game.Load<SoundEffect>("Sounds/Kill"));

        this._backgroundSoundInstance = this._sounds[Sound.Background].CreateInstance();
        this._backgroundSoundInstance.IsLooped = true;
        this._backgroundSoundInstance.Volume = .15f;
        this.ChangeBackgroundStatus(true);
    }

    public void ChangeBackgroundStatus(bool status) {
        if (status) this._backgroundSoundInstance?.Play();
        else this._backgroundSoundInstance?.Stop();
    }

    public SoundEffectInstance GetSound(Sound sound) => this._sounds[sound].CreateInstance();

    public void Play(Sound sound) => this._sounds[sound].Play();

}