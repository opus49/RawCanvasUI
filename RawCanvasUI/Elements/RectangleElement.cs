using RawCanvasUI.Interfaces;
using System.Drawing;

namespace RawCanvasUI.Elements
{
    public class RectangleElement : BaseElement, IGeometry
    {
        public RectangleElement(Color color, int width, int height)
        {
            this.Color = color;
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Gets or sets a value indicating the rectangle color.
        /// </summary>
        public Color Color { get; set; }

        /// <inheritdoc/>
        public int Height { get; protected set; }

        /// <inheritdoc/>
        public int Width { get; protected set; }

        /// <summary>
        /// Draws the rectangle onto the specified graphics object.
        /// </summary>
        /// <param name="g">The graphics object to draw onto.</param>
        public override void Draw(Rage.Graphics g)
        {
            g.DrawRectangle(this.Bounds, this.Color);
        }

        /// <inheritdoc/>
        public override void UpdateBounds()
        {
            var x = this.Parent.Bounds.X + (this.Position.X * this.Parent.Scale.Height);
            var y = this.Parent.Bounds.Y + (this.Position.Y * this.Parent.Scale.Height);
            var size = new SizeF(this.Width * this.Parent.Scale.Height, this.Height * this.Parent.Scale.Height);
            this.Bounds = new RectangleF(new PointF(x, y), size);
        }
    }
}
