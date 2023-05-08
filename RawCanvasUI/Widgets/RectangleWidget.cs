using RawCanvasUI.Style;
using RawCanvasUI.Widgets;
using System.Drawing;

namespace RawCanvasUI.Widgets
{
    /// <summary>
    /// Represents a plain colored rectangular widget.
    /// </summary>
    public class RectangleWidget : BaseWidget
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleWidget"/> class.
        /// </summary>
        /// <param name="width">The width of the rectangle in relation to the canvas.</param>
        /// <param name="height">The height of the rectangle in relation to the canvas.</param>
        public RectangleWidget(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Gets or sets the background color of the rectangle.
        /// </summary>
        public Color BackgroundColor { get; set; } = Defaults.BackgroundColor;

        /// <inheritdoc/>
        public override void Draw(Rage.Graphics g)
        {
            g.DrawRectangle(this.Bounds, this.BackgroundColor);
            base.Draw(g);
        }

        /// <summary>
        /// Resizes the widget.
        /// </summary>
        /// <param name="size">The new size.</param>
        public void Resize(Size size)
        {
            this.Width = size.Width;
            this.Height = size.Height;
            this.UpdateBounds();
        }
    }
}
