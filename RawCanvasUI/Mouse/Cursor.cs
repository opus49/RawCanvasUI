using System.Diagnostics;
using System.Drawing;
using Rage;
using Rage.Native;
using RawCanvasUI.Elements;

namespace RawCanvasUI.Mouse
{
    /// <summary>
    /// A special sprite that represents the mouse cursor on the screen.
    /// </summary>
    public sealed class Cursor : Sprite
    {
        private readonly Stopwatch clickTimer = new Stopwatch();
        private bool isVisible;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cursor"/> class.
        /// </summary>
        public Cursor()
            : base("cursor/default.png", 16, 16)
        {
            this.MouseStatus = MouseStatus.Up;
            this.ScrollWheelStatus = ScrollWheelStatus.None;
        }

        /// <summary>
        /// Gets the duration of a mouse down event.
        /// </summary>
        public long ClickDuration
        {
            get
            {
                return this.clickTimer.ElapsedMilliseconds;
            }
        }

        /// <summary>
        /// Gets or sets the long click duration used for dragging.
        /// </summary>
        public long LongClickDuration { get; set; } = Constants.LongClickDuration;

        /// <inheritdoc/>
        public override bool IsVisible
        {
            get
            {
                return this.isVisible;
            }

            set
            {
                this.isVisible = value;
                this.clickTimer.Reset();
                this.SetCursorType(CursorType.Default);
            }
        }

        /// <summary>
        /// Gets the mouse down or up status.
        /// </summary>
        public MouseStatus MouseStatus { get; private set; }

        /// <summary>
        /// Gets the mouse scroll wheel down, up, or none status.
        /// </summary>
        public ScrollWheelStatus ScrollWheelStatus { get; private set; }

        /// <summary>
        /// Sets the cursor type.
        /// </summary>
        /// <param name="cursorType">The type of cursor (drag, pointer, resize).</param>
        public void SetCursorType(CursorType cursorType)
        {
            switch (cursorType)
            {
                case CursorType.Default:
                    this.SetTextureName("cursor/default.png");
                    break;
                case CursorType.Pointing:
                    this.SetTextureName("cursor/pointing.png");
                    break;
            }
        }

        /// <inheritdoc/>
        public override void Draw(Rage.Graphics g)
        {
            if (this.Texture != null)
            {
                base.Draw(g);
            }
        }

        /// <summary>
        /// Updates the position, mouse status, and scroll wheel status.  The position is based on the canvas.
        /// </summary>
        public void UpdateStatus()
        {
            this.UpdatePosition();
            this.UpdateMouseStatus();
            this.UpdateScrollWheelStatus();
        }

        /// <summary>
        /// Updates the status of the mouse button.
        /// </summary>
        private void UpdateMouseStatus()
        {
            if (NativeFunction.Natives.IS_DISABLED_CONTROL_PRESSED<bool>(0, (int)GameControl.CursorAccept))
            {
                if (this.MouseStatus != MouseStatus.Down)
                {
                    this.clickTimer.Restart();
                    this.MouseStatus = MouseStatus.Down;
                }
            }
            else
            {
                if (this.MouseStatus != MouseStatus.Up)
                {
                    this.clickTimer.Reset();
                    this.MouseStatus = MouseStatus.Up;
                }
            }
        }

        /// <summary>
        /// Updates the cursor's position based on the mouse position.
        /// </summary>
        private void UpdatePosition()
        {
            var x = NativeFunction.Natives.GET_DISABLED_CONTROL_NORMAL<float>(0, (int)GameControl.CursorX);
            var y = NativeFunction.Natives.GET_DISABLED_CONTROL_NORMAL<float>(0, (int)GameControl.CursorY);
            this.Position = new Point((int)System.Math.Round(x * Constants.CanvasWidth), (int)System.Math.Round(y * Constants.CanvasHeight));
            this.UpdateBounds();
        }

        /// <summary>
        /// Update's the status of the scroll wheel.
        /// </summary>
        private void UpdateScrollWheelStatus()
        {
            if (NativeFunction.Natives.IS_DISABLED_CONTROL_PRESSED<bool>(0, (int)GameControl.CursorScrollDown))
            {
                this.ScrollWheelStatus = ScrollWheelStatus.Down;
            }
            else if (NativeFunction.Natives.IS_DISABLED_CONTROL_PRESSED<bool>(0, (int)GameControl.CursorScrollUp))
            {
                this.ScrollWheelStatus = ScrollWheelStatus.Up;
            }
            else
            {
                this.ScrollWheelStatus = ScrollWheelStatus.None;
            }
        }
    }
}
