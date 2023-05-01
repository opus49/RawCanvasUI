using System.Collections.Generic;
using System.ComponentModel;
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
        /// Gets or sets the index of the first line of text to be drawn.
        /// </summary>
        public int FirstLineIndex { get; protected set; } = 0;

        /// <inheritdoc/>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the lines belonging to the text area.
        /// </summary>
        public List<string> Lines { get; protected set; } = new List<string>();

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
        /// Gets or sets the real screen bounds of the inner scrollbar.
        /// </summary>
        public RectangleF ScrollbarInnerBounds { get; protected set; } = default;

        /// <summary>
        /// Gets or sets the scrollbar inner color.
        /// </summary>
        public Color ScrollbarInnerColor { get; set; } = Color.FromArgb(92, 92, 92);

        /// <summary>
        /// Gets or sets the real screen bounds of the outer scrollbar.
        /// </summary>
        public RectangleF ScrollbarOuterBounds { get; protected set; } = default;

        /// <summary>
        /// Gets or sets the scrollbar outer color.
        /// </summary>
        public Color ScrollbarOuterColor { get; set; } = Color.FromArgb(0, 0, 0);

        /// <summary>
        /// Gets or sets the width of the scrollbar.
        /// </summary>
        public float ScrollbarWidth { get; set; } = 8f;

        /// <summary>
        /// Add a line of text to the text box.
        /// </summary>
        /// <param name="text">The line of text to add.</param>
        public void Add(string text)
        {
            this.Lines.Add(text);
            if (this.Lines.Count > this.MaxLines)
            {
                this.FirstLineIndex = this.Lines.Count - this.MaxLines;
            }
        }

        /// <summary>
        /// Clears the text from the text area.
        /// </summary>
        public void Clear()
        {
            this.Lines.Clear();
            this.FirstLineIndex = 0;
            this.UpdateBounds();
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
            for (int i = this.FirstLineIndex; i < this.Lines.Count; i++)
            {
                if (i >= this.FirstLineIndex + this.MaxLines)
                {
                    break;
                }

                g.DrawText(this.Lines[i], this.FontFamily, this.ScaledFontSize, new PointF(this.TextPosition.X, this.TextPosition.Y + ((i - this.FirstLineIndex) * (this.TextSize.Height + (this.TextSize.Height * this.ScaledLineGap)))), this.FontColor);
            }

            if (this.ScrollbarWidth > 0)
            {
                this.DrawScrollbar(g);
            }
        }

        /// <inheritdoc/>
        public virtual void Scroll(ScrollWheelStatus status)
        {
            if (status == ScrollWheelStatus.Up)
            {
                if (this.FirstLineIndex > 0)
                {
                    this.FirstLineIndex--;
                }
            }
            else if (status == ScrollWheelStatus.Down)
            {
                if (this.Lines.Count > this.MaxLines && this.FirstLineIndex < this.Lines.Count - this.MaxLines)
                {
                    this.FirstLineIndex++;
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

        protected void DrawScrollbar(Rage.Graphics g)
        {
            if (this.Parent == null || this.Lines.Count <= this.MaxLines)
            {
                return;
            }

            float scrollbarLength = System.Math.Max(10f, this.Bounds.Height * ((float)this.MaxLines / this.Lines.Count));
            float scrollbarWidth = this.ScrollbarWidth * this.Parent.Scale.Height;
            float slotLength = this.Bounds.Height - scrollbarLength;
            float normalizedPosition = (float)this.FirstLineIndex / (this.Lines.Count - this.MaxLines);
            float positionY = this.Bounds.Y + (slotLength * normalizedPosition);
            float positionX = this.Bounds.X + this.Bounds.Width - scrollbarWidth;
            this.ScrollbarInnerBounds = new RectangleF(positionX, positionY, scrollbarWidth, scrollbarLength);
            this.ScrollbarOuterBounds = new RectangleF(positionX, this.Bounds.Y, scrollbarWidth, this.Bounds.Height);
            g.DrawRectangle(this.ScrollbarOuterBounds, this.ScrollbarOuterColor);
            g.DrawRectangle(this.ScrollbarInnerBounds, this.ScrollbarInnerColor);
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
