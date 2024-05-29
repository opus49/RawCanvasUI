using RawCanvasUI.Elements;
using RawCanvasUI.Style;
using System.Diagnostics;

namespace RawCanvasUI.Keyboard
{
    public class Caret : RectangleElement
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        private readonly EditableTextBox textbox;
        private bool isBlinkedOn = true;

        public Caret(EditableTextBox textbox)
            : base(Defaults.CaretColor, 0, 0)
        {
            this.textbox = textbox;
            this.IsVisible = false;
        }

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
            if (this.textbox != null) 
            {
                this.Bounds = this.textbox.GetCaretBounds();
            }
        }

        private void UpdateBlink()
        {
            if (stopwatch.ElapsedMilliseconds > Constants.CaretBlinkRate)
            {
                this.isBlinkedOn = !this.isBlinkedOn;
                Logging.Debug($"Caret flipping blinked on status to {(this.isBlinkedOn ? "true" : "false")}");
                this.stopwatch.Restart();
            }
        }
    }
}
