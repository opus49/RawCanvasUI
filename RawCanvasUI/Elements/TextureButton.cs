using System.Collections.Generic;
using System.Drawing;
using RawCanvasUI.Interfaces;
using RawCanvasUI.Mouse;

namespace RawCanvasUI.Elements
{
    /// <summary>
    /// Represents a clickable button.
    /// </summary>
    public class TextureButton : TextureElement, IButton, IText
    {
        private readonly List<IObserver> observers = new List<IObserver>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureButton"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the button.</param>
        /// <param name="textureName">The texture name for the button.</param>
        /// <param name="width">The fixed width of the button relative to the canvas.</param>
        /// <param name="height">The fixed height of the button relative to the canvas.</param>
        /// <param name="text">The texts for the label on the button..</param>
        public TextureButton(string id, string textureName, int width, int height, string text)
            : base(textureName, width, height)
        {
            this.Id = id;
            this.Text = text;
        }

        /// <inheritdoc/>
        public Color FontColor { get; set; } = Color.White;

        /// <inheritdoc/>
        public string FontFamily { get; set; } = "Lucida Console";

        /// <inheritdoc/>
        public float FontSize { get; set; } = 14f;

        /// <inheritdoc/>
        public string Id { get; }

        /// <inheritdoc/>
        public float ScaledFontSize { get; protected set; } = 14f;

        /// <inheritdoc/>
        public string Text { get; set; } = string.Empty;

        /// <inheritdoc/>
        public SizeF TextSize { get; protected set; }

        /// <summary>
        /// Gets or sets the real screen position to draw the text.
        /// </summary>
        public PointF TextPosition { get; protected set; } = default;

        /// <inheritdoc/>
        public virtual bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Adds an observer.
        /// </summary>
        /// <param name="observer">The observer to add.</param>
        public void AddObserver(IObserver observer)
        {
            this.observers.Add(observer);
        }

        /// <inheritdoc/>
        public virtual void Click(Cursor cursor)
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

        /// <summary>
        /// Draws the element onto the specified graphics object.
        /// </summary>
        /// <param name="g">The graphics object to draw onto.</param>
        public override void Draw(Rage.Graphics g)
        {
            base.Draw(g);
            if (this.Text != string.Empty)
            {
                g.DrawText(this.Text, this.FontFamily, this.ScaledFontSize, this.TextPosition, this.FontColor, this.Bounds);
            }
        }

        /// <summary>
        /// Removes an observer.
        /// </summary>
        /// <param name="observer">The observer to remove.</param>
        public void RemoveObserver(IObserver observer)
        {
            this.observers.Remove(observer);
        }

        /// <inheritdoc/>
        public override void UpdateBounds()
        {
            if (this.Parent != null)
            {
                base.UpdateBounds();
                this.UpdateText(this.Parent.Scale.Height);
            }
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
