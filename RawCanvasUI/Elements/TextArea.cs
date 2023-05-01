using System.Collections.Generic;
using System.Drawing;
using RawCanvasUI.Interfaces;
using RawCanvasUI.Mouse;

namespace RawCanvasUI.Elements
{
    /// <summary>
    /// Represents an area of text that can have multiple lines.
    /// </summary>
    public class TextArea : TextBox, IScrollable
    {
        private readonly List<string> text = new List<string>();
        private int firstLineIndex = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextArea"/> class.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <param name="width">The width of the text area.</param>
        /// <param name="height">The height of the text area.</param>
        public TextArea(int x, int y, int width, int height)
            : base(x, y, width, height)
        {
            this.Position = new Point(x, y);
            this.Height = height;
            this.Width = width;
        }

        /// <summary>
        /// Clears the text from the text area.
        /// </summary>
        public void Clear()
        {
            this.text.Clear();
            this.firstLineIndex = 0;
            this.UpdateBounds();
        }

        /// <inheritdoc/>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the line gap used in between lines.
        /// </summary>
        public float LineGap { get; set; } = 1.2f;

        /// <summary>
        /// Gets or sets the maximum number of lines that can be displayed in the text area.
        /// </summary>
        public int MaxLines { get; protected set; } = 1;

        /// <summary>
        /// Gets or sets the scaled line gap.
        /// </summary>
        public float ScaledLineGap { get; protected set; } = 0.25f;

        /// <summary>
        /// Add a line of text to the text box.
        /// </summary>
        /// <param name="text">The line of text to add.</param>
        public void Add(string text)
        {
            this.text.Add(text);
            if (this.text.Count > this.MaxLines)
            {
                this.firstLineIndex = this.text.Count - this.MaxLines;
            }
        }

        /// <inheritdoc/>
        public bool Contains(Cursor cursor)
        {
            return this.Bounds.Contains(cursor.Bounds.Location);
        }

        /// <inheritdoc/>
        public override void Draw(Rage.Graphics g)
        {
            g.DrawRectangle(this.BorderBounds, this.BorderColor);
            g.DrawRectangle(this.Bounds, this.BackgroundColor);
            for (int i = this.firstLineIndex; i < this.text.Count; i++)
            {
                if (i >= this.firstLineIndex + this.MaxLines)
                {
                    return;
                }

                g.DrawText(this.text[i], this.FontFamily, this.ScaledFontSize, new PointF(this.TextPosition.X, this.TextPosition.Y + ((i - this.firstLineIndex) * (this.TextSize.Height + (this.TextSize.Height * this.ScaledLineGap)))), this.FontColor);
            }
        }

        /// <inheritdoc/>
        public virtual void Scroll(ScrollWheelStatus status)
        {
            if (status == ScrollWheelStatus.Up)
            {
                if (this.firstLineIndex > 0)
                {
                    this.firstLineIndex--;
                }
            }
            else if (status == ScrollWheelStatus.Down)
            {
                if (this.text.Count > this.MaxLines && this.firstLineIndex < this.text.Count - this.MaxLines)
                {
                    this.firstLineIndex++;
                }
            }
        }

        /// <summary>
        /// Updates the number of maximum lines that can be displayed in the text area.
        /// </summary>
        protected void UpdateMaxLines()
        {
            this.MaxLines = System.Convert.ToInt32(System.Math.Floor(this.Bounds.Height / (this.TextSize.Height + (this.TextSize.Height * this.ScaledLineGap))));
        }

        /// <summary>
        /// Updates the scaled line gap.
        /// </summary>
        /// <param name="scale">The scale to apply to the line gap.</param>
        protected void UpdateScaledLineGap(float scale)
        {
            this.ScaledLineGap = this.LineGap * scale;
        }

        /// <inheritdoc/>
        protected override void UpdateTextPosition(float scale)
        {
            this.UpdateScaledLineGap(scale);
            this.UpdateMaxLines();
            float x = this.Bounds.X + (this.LeftPadding * scale);
            var totalTextSize = this.MaxLines * (this.TextSize.Height + (this.TextSize.Height * this.ScaledLineGap));
            var totalGap = this.Bounds.Height - totalTextSize + (this.TextSize.Height * this.ScaledLineGap);
            float y = this.Bounds.Y + (totalGap / 2f);
            this.TextPosition = new PointF(x, y);
        }
    }
}
