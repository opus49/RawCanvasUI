using Rage;
using Rage.Native;
using RawCanvasUI.Interfaces;
using RawCanvasUI.Mouse;
using RawCanvasUI.Util;
using System;
using System.Drawing;

namespace RawCanvasUI
{
    /// <summary>
    /// A canvas representing the screen area where elements can be added and positioned.
    /// </summary>
    public sealed class Canvas : IParent
    {
        private readonly WidgetManager widgetManager;
        private bool isInteractive = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Canvas"/> class.
        /// </summary>
        public Canvas()
        {
            this.UUID = $"canvas-{Guid.NewGuid()}";
            this.widgetManager = new WidgetManager();
            this.Cursor = new Cursor()
            {
                Parent = this,
            };
        }

        /// <inheritdoc />
        public RectangleF Bounds { get; private set; }

        /// <summary>
        /// Gets the cursor belonging to the canvas.
        /// </summary>
        public Cursor Cursor { get; }

        /// <inheritdoc />
        public string Id
        {
            get
            {
                return this.UUID;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the canvas is interactive.
        /// </summary>
        public bool IsInteractive
        {
            get => this.isInteractive;
            set
            {
                if (this.isInteractive != value)
                {
                    this.isInteractive = value;
                    if (this.isInteractive)
                    {
                        NativeFunction.Natives.SET_USER_RADIO_CONTROL_ENABLED(false);
                    }
                    else
                    {
                        NativeFunction.Natives.SET_USER_RADIO_CONTROL_ENABLED(true);
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the game is paused.
        /// This is safe to check on the raw frame render event.
        /// </summary>
        public bool IsGamePaused { get; private set; } = false;

        /// <inheritdoc />
        public Point Position { get; } = new Point(0, 0);

        /// <summary>
        /// Gets the screen resolution.
        /// </summary>
        public Size Resolution { get; private set; }

        /// <inheritdoc />
        public SizeF Scale { get; private set; }

        /// <inheritdoc />
        public string UUID { get; private set; }

        /// <summary>
        /// Adds a widget to the canvas.
        /// </summary>
        /// <param name="widget">The widget to add.</param>
        public void Add(IWidget widget)
        {
            widget.Parent = this;
            this.widgetManager.AddWidget(widget);
        }

        /// <summary>
        /// Loads the canvas.
        /// </summary>
        /// <param name="texturePath">The fully qualified path to the textures.</param>
        /// <param name="stylesheetPath">The fully qualified path to the textures.</param>
        public void Load(string texturePath, string stylesheetPath)
        {
            TextureHandler.Load(this.UUID, texturePath);
            this.widgetManager.ApplyStyle(stylesheetPath);
            this.UpdateBounds();
            Rage.Game.FrameRender -= this.Game_FrameRender;
            Rage.Game.RawFrameRender -= this.Game_RawFrameRender;
            Rage.Game.FrameRender += this.Game_FrameRender;
            Rage.Game.RawFrameRender += this.Game_RawFrameRender;
        }

        /// <inheritdoc />
        public void MoveTo(Point position)
        {
            Logging.Warning("you cannot move the canvas, dumbass");
        }

        private void Game_FrameRender(object sender, Rage.GraphicsEventArgs e)
        {
            this.IsGamePaused = Game.IsPaused || NativeFunction.Natives.IS_PAUSE_MENU_ACTIVE<bool>();
            if (!this.IsInteractive || Game.Console.IsOpen || this.IsGamePaused)
            {
                return;
            }

            if (this.Resolution != Game.Resolution)
            {
                this.UpdateBounds();
            }

            this.DisableControls();
            if (NativeFunction.Natives.IS_DISABLED_CONTROL_JUST_RELEASED<bool>(0, (int)GameControl.CursorCancel))
            {
                this.IsInteractive = false;
            }
            else
            {
                this.Cursor.UpdateStatus();
                this.widgetManager.HandleMouseEvents(this.Cursor);
            }
        }

        private void Game_RawFrameRender(object sender, Rage.GraphicsEventArgs e)
        {
            if (!this.IsGamePaused)
            {
                this.widgetManager.Draw(e.Graphics);
                if (this.IsInteractive)
                {
                    this.Cursor.Draw(e.Graphics);
                }
            }
        }

        private void DisableControls()
        {
            NativeFunction.Natives.SET_INPUT_EXCLUSIVE(2, (int)GameControl.CursorX);
            NativeFunction.Natives.SET_INPUT_EXCLUSIVE(2, (int)GameControl.CursorY);
            NativeFunction.Natives.SET_INPUT_EXCLUSIVE(2, (int)GameControl.CursorScrollUp);
            NativeFunction.Natives.SET_INPUT_EXCLUSIVE(2, (int)GameControl.CursorScrollDown);
            NativeFunction.Natives.SET_INPUT_EXCLUSIVE(2, (int)GameControl.VehicleAim);
            Game.DisableControlAction(0, GameControl.CursorX, true);
            Game.DisableControlAction(0, GameControl.CursorY, true);
            Game.DisableControlAction(0, GameControl.CursorAccept, true);
            Game.DisableControlAction(0, GameControl.CursorCancel, true);
            Game.DisableControlAction(0, GameControl.CursorScrollUp, true);
            Game.DisableControlAction(0, GameControl.CursorScrollDown, true);
            Game.DisableControlAction(0, GameControl.WeaponWheelNext, true);
            Game.DisableControlAction(0, GameControl.WeaponWheelPrev, true);
            Game.DisableControlAction(0, GameControl.Attack, true);
            Game.DisableControlAction(0, GameControl.Attack2, true);
            Game.DisableControlAction(0, GameControl.Reload, true);
            Game.DisableControlAction(0, GameControl.MeleeAttack1, true);
            Game.DisableControlAction(0, GameControl.MeleeAttack2, true);
            Game.DisableControlAction(0, GameControl.MeleeAttackAlternate, true);
            Game.DisableControlAction(0, GameControl.VehicleAim, true);
            Game.DisableControlAction(0, GameControl.VehicleAttack, true);
            Game.DisableControlAction(0, GameControl.VehicleAttack2, true);
        }

        private void UpdateBounds()
        {
            Logging.Info("game resolution has changed, updating bounds");
            this.Resolution = Rage.Game.Resolution;
            this.Scale = new SizeF(this.Resolution.Width / Constants.CanvasWidth, this.Resolution.Height / Constants.CanvasHeight);
            this.Bounds = new RectangleF(this.Position, this.Resolution);
            this.widgetManager.UpdateWidgetBounds();
        }
    }
}
