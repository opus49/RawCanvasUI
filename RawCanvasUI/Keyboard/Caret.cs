using RawCanvasUI.Elements;
using RawCanvasUI.Interfaces;
using RawCanvasUI.Style;
using System.Diagnostics;
using System.Drawing;

namespace RawCanvasUI.Keyboard
{
    public class Caret : RectangleElement
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        private bool isBlinkedOn = true;

        public Caret()
            : base(Defaults.CaretColor, 0, 0)
        {
            this.IsVisible = false;
        }

        /// <summary>
        /// Gets or sets a value indicating the target edtiable control.
        /// </summary>
        public IEditable Control { get; set; } = null;

        /// <inheritdoc/>
        public override bool IsVisible 
        { 
            get => base.IsVisible;
            set
            {
                Logging.Debug($"Caret visibility set to {(value ? "true" : "false")}");
                base.IsVisible = value;
                if (value)
                {
                    this.UpdateBounds();
                    this.stopwatch.Restart();
                    this.isBlinkedOn = true;
                }
                else
                {
                    this.stopwatch.Stop();
                }
            }
        }

        /// <inheritdoc/>
        public override void Draw(Rage.Graphics g)
        {
            if (this.IsVisible)
            {
                this.UpdateBlink();
                if (this.isBlinkedOn)
                {
                    base.Draw(g);
                }
            }
        }

        /// <inheritdoc/>
        public override void UpdateBounds()
        {
            if (this.Control != null)
            {
                Logging.Debug("Caret updating bounds");
                this.Bounds = this.Control.GetCaretBounds();
            }
            else
            {
                Logging.Debug("Caret cannot update bounds due to control being null");
            }
        }

        private void UpdateBlink()
        {
            if (stopwatch.ElapsedMilliseconds > Constants.CaretBlinkRate)
            {
                this.isBlinkedOn = !this.isBlinkedOn;
                this.stopwatch.Restart();
            }
        }
    }
}
