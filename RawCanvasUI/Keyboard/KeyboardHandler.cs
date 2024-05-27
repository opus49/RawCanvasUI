using Rage;
using RawCanvasUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RawCanvasUI.Keyboard
{
    internal class KeyboardHandler
    {
        private readonly Dictionary<Keys, KeyPressInfo> activeKeyPresses = new Dictionary<Keys, KeyPressInfo>();

        public IEditable Control { get; set; } = null;

        public bool IsRunning { get; private set; } = false;

        public void Start(IEditable control)
        {
            Control = control;
            activeKeyPresses.Clear();
            if (!IsRunning)
            {
                GameFiber.StartNew(Run, $"canvas-keyboardhandler-{Guid.NewGuid()}");
            }
        }

        public void Stop()
        {
            IsRunning = false;
        }

        private void Run()
        {
            IsRunning = true;
            while (IsRunning && Control != null)
            {
                GameFiber.Yield();
                var keyboardState = Game.GetKeyboardState();
                var now = DateTime.Now;
                foreach (var key in keyboardState.PressedKeys)
                {
                    if (!KeyValidator.IsValidKey(key))
                    {
                        continue;
                    }

                    if (!activeKeyPresses.TryGetValue(key, out var keyPressInfo))
                    {
                        keyPressInfo = new KeyPressInfo(key, now, keyboardState.IsShiftDown);
                        activeKeyPresses[key] = keyPressInfo;
                        Control.HandleInput(keyPressInfo.ToString());
                    }
                    else if (keyPressInfo.ShouldProcessKey(now))
                    {
                        keyPressInfo.IncrementRepeatCount();
                        Control.HandleInput(keyPressInfo.ToString());
                    }
                }

                var keysToRemove = activeKeyPresses.Keys.Except(keyboardState.PressedKeys).ToList();
                foreach (var key in keysToRemove)
                {
                    activeKeyPresses.Remove(key);
                }
            }

            IsRunning = false;
        }
    }
}
