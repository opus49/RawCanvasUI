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

        /// <inheritdoc/>
        public override void Draw(Rage.Graphics g)
        {
            g.DrawText(this.Text, this.FontFamily, this.ScaledFontSize, this.Bounds.Location, this.FontColor);
        }
    }
}
