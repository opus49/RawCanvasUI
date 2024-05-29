using Rage;
using RawCanvasUI.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace RawCanvasUI.Keyboard
{
    internal class KeyboardHandler
    {
        private readonly Dictionary<Keys, KeyPress> activeKeyPresses = new Dictionary<Keys, KeyPress>();
        private readonly Stopwatch stopwatch = new Stopwatch();

        public KeyboardHandler(WidgetManager widgetManager)
        {
            this.WidgetManager = widgetManager;
        }

        /// <summary>
        /// Gets a value indicating whether or not hte keyboard manager is actively running.
        /// </summary>
        public bool IsRunning { get; private set; } = false;

        /// <summary>
        /// Gets the widget manager for this keyboard handler.
        /// </summary>
        public WidgetManager WidgetManager { get; private set; }

        /// <summary>
        /// Starts the keyboard handler which in turn will start sending inputs to the widget manager.
        /// </summary>
        public void Start()
        {
            if (!this.IsRunning)
            {
                Logging.Debug("KeyboardHandler starting...");
                this.activeKeyPresses.Clear();
                this.IsRunning = true;
                Game.IsPaused = true;
                stopwatch.Restart();
                GameFiber.StartNew(Run, $"canvas-keyboardhandler-{Guid.NewGuid()}");
            }
        }

        /// <summary>
        /// Stops the keyboard handler.
        /// </summary>
        public void Stop()
        {
            Logging.Debug("KeyboardHandler stopping...");
            this.IsRunning = false;
        }

        private void Run()
        {
            while (this.IsRunning && Game.IsPaused)
            {
                var elapsedMillis = this.stopwatch.ElapsedMilliseconds;
                var keyboardState = Game.GetKeyboardState();
                foreach (var key in keyboardState.PressedKeys)
                {
                    if (!KeyValidator.IsValidKey(key))
                    {
                        continue;
                    }

                    if (!activeKeyPresses.TryGetValue(key, out var keyPressInfo))
                    {
                        keyPressInfo = new KeyPress(key, elapsedMillis, keyboardState.IsShiftDown);
                        activeKeyPresses[key] = keyPressInfo;
                        this.WidgetManager.HandleKeyboardInput(keyPressInfo.ToString());
                    }
                    else if (keyPressInfo.ShouldProcessKey(elapsedMillis))
                    {
                        keyPressInfo.IncrementRepeatCount();
                        this.WidgetManager.HandleKeyboardInput(keyPressInfo.ToString());
                    }
                }

                var keysToRemove = activeKeyPresses.Keys.Except(keyboardState.PressedKeys).ToList();
                foreach (var key in keysToRemove)
                {
                    activeKeyPresses.Remove(key);
                }
                GameFiber.Yield();
            }

            Logging.Debug("KeyboardHandler run fiber ending");
            var isRunningStatus = this.IsRunning ? "true" : "false";
            var isPausedStatus = Game.IsPaused ? "true" : "false";
            Logging.Debug($"IsRunning: {isRunningStatus}  IsPaused: {isPausedStatus}");
            this.IsRunning = false;
            Game.IsPaused = false;
            this.stopwatch.Stop();
        }
    }
}
