using System.Drawing;
using RawCanvasUI.Interfaces;
using RawCanvasUI.Style;

namespace RawCanvasUI.Elements
{
    /// <summary>
    /// Represents a drawable text based element.
    /// </summary>
    public abstract class TextElement : BaseElement, IText
    {
        /// <inheritdoc/>
        public Color DisabledFontColor { get; set; } = Defaults.DisabledFontColor;

        /// <inheritdoc/>
        public Color FontColor { get; set; } = Defaults.FontColor;

        /// <inheritdoc/>
        public string FontFamily { get; set; } = Defaults.FontFamily;

        /// <inheritdoc/>
        public float FontSize { get; set; } = Defaults.FontSize;

        /// <inheritdoc/>
        public float ScaledFontSize { get; protected set; } = Defaults.FontSize;

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