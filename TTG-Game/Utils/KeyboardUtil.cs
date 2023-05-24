using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TTG_Game.Utils; 

/*
 * Credit original code: https://github.com/RuiCardoso021/CreativeGame/blob/master/CreativeGame/CreativeGame/Classes/KeyboardManager.cs
 * 
 *  MonoGame - Default: 
 *    Keyboard.IsDown(tecla) -> bool 
 *    Keyboard.IsUp(tecla) -> bool
 *
 *  IPCA.KeyboardManager 
 *     Keyboard.IsKeyDown(tecla) -> bool
 *     Keyboard.IsKeyUp(tecla)   -> bool
 *     Keyboard.GoingDown(tecla) -> bool
 *     Keyboard.GoingUp(tecla)   -> bool
 */

public enum KeysState {
    Up,
    Down,
    GoingUp,
    GoingDown
}

public class KeyboardUtil {

    private static Dictionary<Keys, KeysState> _keyboardState = new();
    
    /*
        Keys.A => {
                     KeysState.GoingUp => [ Action1, Action2 ]
                     KeysState.Down => [ Action1, Action2 ]
                  }
        Keys.B => { 
                    KeysState.Up => [ Action1 ]
                    KeysState.GoingUp => [ Action2 ]
                  }
    */
    private static Dictionary<Keys, Dictionary<KeysState, List<Action>>> _actions = new();

    // Action é uma referência a uma função     void f(void)
    public static void Register(Keys key, KeysState state, Action code) {
        // Do we have this key already in the dictionary?
        if (!_actions.ContainsKey(key))
            _actions[key] = new Dictionary<KeysState, List<Action>>();

        // For this key, do we have that state created?
        if (!_actions[key].ContainsKey(state))
            _actions[key][state] = new List<Action>();

        // Add the code to the key/state pair
        _actions[key][state].Add(code);
        // Add the key to the keyboard state dictionary
        _keyboardState[key] = KeysState.Up;
    }
    
    public static bool IsKeyDown(Keys k) =>
        _keyboardState.ContainsKey(k) && _keyboardState[k] == KeysState.Down;

    public static bool IsKeyUp(Keys k) =>
        _keyboardState.ContainsKey(k) && _keyboardState[k] == KeysState.Up;
    public static bool IsGoingDown(Keys k) =>
        _keyboardState.ContainsKey(k) && _keyboardState[k] == KeysState.GoingDown;
    public static bool IsGoingUp(Keys k) =>
        _keyboardState.ContainsKey(k) && _keyboardState[k] == KeysState.GoingUp;
    
        public static void Update(GameTime gameTime) {
            var state = Keyboard.GetState();
            var pressedKeys = state.GetPressedKeys().ToList();

            // Process pressed keys
            foreach (Keys key in pressedKeys) {
                // If we didn't know anything about this key, then probably it was up.
                _keyboardState.TryAdd(key, KeysState.Up);

                // What was the previous state, and decide what is our next state
                switch (_keyboardState[key]) {
                    /*   Estado Anterior  Agora   Guardo
                     *      DOWN           DOWN    Down
                     *    GOING DOWN       DOWN    Down
                     *       UP            DOWN    Going Down
                     *    GOING UP         DOWN    Going Down
                     */
                    case KeysState.Down:
                    case KeysState.GoingDown:
                        _keyboardState[key] = KeysState.Down;
                        break;
                    case KeysState.Up:
                    case KeysState.GoingUp:
                        _keyboardState[key] = KeysState.GoingDown;
                        break;
                }
            }
            // Processed released keys
            //   Keys[] x = _keyboardState.Keys.Except(pressedKeys).ToArray();
            //   foreach (Keys key in x)
            // same as...
            foreach (var key in _keyboardState.Keys.Except(pressedKeys).ToArray()) {
                /*   Estado Anterior  Agora   Guardo
                 *      DOWN           UP      GoingUp
                 *    GOING DOWN       UP      GoingUp
                 *       UP            UP      UP
                 *    GOING UP         UP      UP
                 */
                switch (_keyboardState[key]) {
                    case KeysState.Down:
                    case KeysState.GoingDown:
                        _keyboardState[key] = KeysState.GoingUp;
                        break;
                    case KeysState.Up:
                    case KeysState.GoingUp:
                        _keyboardState[key] = KeysState.Up;
                        break;
                }
            }

            // Invocar as funções registadas!
            foreach (var key in _actions.Keys) {
                var kstate = _keyboardState[key];
                if (!_actions[key].ContainsKey(kstate)) continue;
                foreach (Action action in _actions[key][kstate])
                    action();
            }
        }
    
}