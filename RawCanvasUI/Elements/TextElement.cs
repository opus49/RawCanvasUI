using System.Drawing;
using RawCanvasUI.Interfaces;

namespace RawCanvasUI.Elements
{
    /// <summary>
    /// Represents a drawable text based element.
    /// </summary>
    public abstract class TextElement : BaseElement, IText
    {
        /// <inheritdoc/>
        public Color FontColor { get; set; }

        /// <inheritdoc/>
        public string FontFamily { get; set; } 

        /// <inheritdoc/>
        public float FontSize { get; set; }

        /// <inheritdoc/>
        public float ScaledFontSize { get; protected set; } = 14f;

        /// <inheritdoc/>
        public string Text { get; set; } = string.Empty;

        /// <inheritdoc/>
        public SizeF TextSize { get; protected set; } = default;

        /// <inheritdoc/>
        public override void UpdateBounds()
        {
            if (this.Parent != null)
            {
                var x = this.Parent.Bounds.X + (this.Position.X * this.Parent.Scale.Height);
                var y = this.Parent.Bounds.Y + (this.Position.Y * this.Parent.Scale.Height);
                this.ScaledFontSize = this.FontSize * this.Parent.Scale.Height;
                this.TextSize = Rage.Graphics.MeasureText(this.Text, this.FontFamily, this.ScaledFontSize);
                this.Bounds = new RectangleF(new PointF(x, y), this.TextSize);
            }
        }
    }
}