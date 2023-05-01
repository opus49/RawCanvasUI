﻿using System.Collections.Generic;
using System.Drawing;
using RawCanvasUI.Interfaces;
using RawCanvasUI.Mouse;

namespace RawCanvasUI.Elements
{
    /// <summary>
    /// Represents a button that is drawn as a rectangle rather than using a texture.
    /// </summary>
    public class RectangleButton : TextElement, IButton, IGeometry, IText
    {
        private readonly List<IObserver> observers = new List<IObserver>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleButton"/> class.
        /// </summary>
        /// <param name="id">The unique id of the button.</param>
        /// <param name="width">The width of the button relative to the canvas.</param>
        /// <param name="height">The height of the button relative to the canvas.</param>
        /// <param name="text">The text written on the button.</param>
        public RectangleButton(string id, int width, int height, string text)
        {
            this.Id = id;
            this.Width = width;
            this.Height = height;
            this.Text = text;
            this.FontColor = Color.White;
        }

        /// <summary>
        /// Gets or sets the button's background color.
        /// </summary>
        public Color BackgroundColor { get; set; } = Color.Black;

        /// <summary>
        /// Gets or sets the border color.
        /// </summary>
        public Color BorderColor { get; set; } = Color.Black;

        /// <summary>
        /// Gets or sets the border width.
        /// </summary>
        public float BorderWidth { get; set; } = 0f;

        /// <inheritdoc/>
        public int Height { get; protected set; }

        /// <inheritdoc/>
        public string Id { get; private set; }

        /// <inheritdoc/>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the real screen position to draw the text.
        /// </summary>
        public PointF TextPosition { get; set; } = default;

        /// <inheritdoc/>
        public int Width { get; protected set; }

        /// <inheritdoc/>
        public void AddObserver(IObserver observer)
        {
            this.observers.Add(observer);
        }

        /// <inheritdoc/>
        public void Click()
        {
            foreach (var observer in this.observers)
            {
                observer.OnUpdated(this);
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
            if (this.BorderWidth > 0 && this.Parent != null)
            {
                float scaledBorderWidth = this.BorderWidth * this.Parent.Scale.Height;
                var borderBounds = this.Bounds;
                borderBounds.Inflate(scaledBorderWidth, scaledBorderWidth);
                g.DrawRectangle(borderBounds, this.BorderColor);
            }

            g.DrawRectangle(this.Bounds, this.BackgroundColor);
            g.DrawText(this.Text, this.FontFamily, this.ScaledFontSize, this.TextPosition, this.FontColor);
        }

        /// <inheritdoc/>
        public void RemoveObserver(IObserver observer)
        {
            this.observers.Remove(observer);
        }

        /// <summary>
        /// Resizes the button relative to the canvas.
        /// </summary>
        /// <param name="size">The new size.</param>
        public void Resize(Size size)
        {
            this.Width = size.Width;
            this.Height = size.Height;
            this.UpdateBounds();
        }

        /// <inheritdoc/>
        public override void UpdateBounds()
        {
            if (this.Parent == null)
            {
                return;
            }

            float x = this.Parent.Bounds.X + (this.Position.X * this.Parent.Scale.Height);
            float y = this.Parent.Bounds.Y + (this.Position.Y * this.Parent.Scale.Height);
            var size = new SizeF(this.Width * this.Parent.Scale.Height, this.Height * this.Parent.Scale.Height);
            this.Bounds = new RectangleF(new PointF(x, y), size);
            this.UpdateText(this.Parent.Scale.Height);
        }

        /// <summary>
        /// Updates the text metadata.
        /// </summary>
        /// <param name="scale">The parent scale.</param>
        protected virtual void UpdateText(float scale)
        {
            // centers the text on the button
            this.ScaledFontSize = this.FontSize * scale;
            this.TextSize = Rage.Graphics.MeasureText(this.Text, this.FontFamily, this.ScaledFontSize);
            var x = this.Bounds.X + (this.Bounds.Width / 2) - (this.TextSize.Width / 2);
            var y = this.Bounds.Y + (this.Bounds.Height / 2) - (this.TextSize.Height / 2);
            this.TextPosition = new PointF(x, y);
        }
    }
}
