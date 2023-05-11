﻿using System.Drawing;

namespace RawCanvasUI.Elements
{
    /// <summary>
    /// Represents a text-based label.
    /// </summary>
    public class Label : TextElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        /// <param name="text">The text of the label.</param>
        /// <param name="x">The x-coordinate on the canvas.</param>
        /// <param name="y">The y-coordinate on the canvas.</param>
        public Label(string text, int x, int y)
        {
            this.Text = text;
            this.Position = new Point(x, y);
        }

        /// <summary>
        /// Gets or sets the real screen position to draw the text.
        /// </summary>
        public PointF TextPosition { get; protected set; } = default;

        /// <inheritdoc/>
        public override void Draw(Rage.Graphics g)
        {
            // g.DrawText(this.Text, this.FontFamily, this.ScaledFontSize, this.Bounds.Location, this.FontColor);
            if (this.Parent != null)
            {
                g.DrawText(this.Text, this.FontFamily, this.ScaledFontSize, this.TextPosition, this.FontColor, this.Parent.Bounds);
            }
        }

        /// <inheritdoc/>
        public override void UpdateBounds()
        {
            if (this.Parent != null)
            {
                base.UpdateBounds();
                this.UpdateTextPosition(this.Parent.Scale.Height);
            }
        }

        /// <summary>
        /// Updates the on screen text position.
        /// </summary>
        /// <param name="scale">The scale to apply to the padding.</param>
        protected virtual void UpdateTextPosition(float scale)
        {
            float leading = Constants.Leading.TryGetValue(this.FontFamily, out float result) ? result : 0.25f;
            var x = this.Bounds.X;
            var y = this.Bounds.Y + (this.Bounds.Height / 2f) - (this.TextSize.Height / 2f) - (this.TextSize.Height * leading);
            this.TextPosition = new PointF(x, y);
        }
    }
}
