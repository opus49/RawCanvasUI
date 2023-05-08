using RawCanvasUI.Style;
using System.Drawing;

namespace RawCanvasUI.Elements
{
    /// <summary>
    /// Represents a box with text in it.
    /// </summary>
    public class TextBox : TextElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextBox"/> class.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <param name="width">The width of the text box.</param>
        /// <param name="height">The height of the text box.</param>
        public TextBox(int x, int y, int width, int height)
        {
            this.Position = new Point(x, y);
            this.Height = height;
            this.Width = width;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBox"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <param name="width">The width of the text box.</param>
        /// <param name="height">The height of the text box.</param>
        public TextBox(string text, int x, int y, int width, int height)
            : this(x, y, width, height)
        {
            this.Text = text;
        }

        /// <summary>
        /// Gets or sets the color of the text box.
        /// </summary>
        public Color BackgroundColor { get; set; } = Defaults.BackgroundColor;

        /// <summary>
        /// Gets or sets the screen boundary for the border.
        /// </summary>
        public RectangleF BorderBounds { get; protected set; } = default;

        /// <summary>
        /// Gets or sets the border color.
        /// </summary>
        public Color BorderColor { get; set; } = Defaults.BorderColor;

        /// <summary>
        /// Gets or sets the width of the border.
        /// </summary>
        public float BorderWidth { get; set; } = Defaults.BorderWidth;

        /// <summary>
        /// Gets or sets the height of the text box.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the left padding.
        /// </summary>
        public float LeftPadding { get; set; } = Defaults.LeftPadding;

        /// <summary>
        /// Gets or sets the real screen position to draw the text.
        /// </summary>
        public PointF TextPosition { get; protected set; } = default;

        /// <summary>
        /// Gets or sets the width of the text box.
        /// </summary>
        public int Width { get; set; }

        /// <inheritdoc/>
        public override void Draw(Rage.Graphics g)
        {
            g.DrawRectangle(this.BorderBounds, this.BorderColor);
            g.DrawRectangle(this.Bounds, this.BackgroundColor);
            g.DrawText(this.Text, this.FontFamily, this.ScaledFontSize, this.TextPosition, this.FontColor, this.Bounds);
        }

        /// <inheritdoc/>
        public override void UpdateBounds()
        {
            if (this.Parent != null)
            {
                var scale = this.Parent.Scale.Height;
                var screenPosition = new PointF(this.Parent.Bounds.X + (this.Position.X * scale), this.Parent.Bounds.Y + (this.Position.Y * scale));
                var scaledSize = new SizeF(this.Width * scale, this.Height * scale);
                this.Bounds = new RectangleF(screenPosition, scaledSize);
                this.UpdateBorderBounds(scale);
                this.UpdateScaledFontSize(scale);
                this.UpdateTextSize();
                this.UpdateTextPosition(scale);
            }
        }

        /// <summary>
        /// Updates the border bounds.
        /// </summary>
        /// <param name="scale">The scale to apply to the border width.</param>
        protected virtual void UpdateBorderBounds(float scale)
        {
            var borderBounds = this.Bounds;
            float borderWidth = this.BorderWidth * scale;
            borderBounds.Inflate(borderWidth, borderWidth);
            this.BorderBounds = borderBounds;
        }

        /// <summary>
        /// Updates the scaled font size.
        /// </summary>
        /// <param name="scale">The scale to apply to the font.</param>
        protected virtual void UpdateScaledFontSize(float scale)
        {
            this.ScaledFontSize = this.FontSize * scale;
        }

        /// <summary>
        /// Updates the text height.
        /// </summary>
        protected virtual void UpdateTextSize()
        {
            this.TextSize = Rage.Graphics.MeasureText("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789,^`", this.FontFamily, this.ScaledFontSize);
        }

        /// <summary>
        /// Updates the on screen text position.
        /// </summary>
        /// <param name="scale">The scale to apply to the padding.</param>
        protected virtual void UpdateTextPosition(float scale)
        {
            var x = this.Bounds.X + (this.LeftPadding * scale);
            var y = this.Bounds.Y + (this.Bounds.Height / 2f) - (this.TextSize.Height / 2f);
            this.TextPosition = new PointF(x, y);
        }
    }
}
