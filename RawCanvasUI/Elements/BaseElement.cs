﻿using System.Drawing;
using RawCanvasUI.Interfaces;

namespace RawCanvasUI.Elements
{
    /// <summary>
    /// Represents a drawable element.
    /// </summary>
    public abstract class BaseElement : IDrawable
    {
        /// <inheritdoc/>
        public RectangleF Bounds { get; protected set; } = default;

        /// <inheritdoc/>
        public virtual bool IsVisible { get; set; } = true;

        /// <inheritdoc/>
        public IParent Parent { get; set; }

        /// <inheritdoc/>
        public Point Position { get; protected set; } = default;

        /// <summary>
        /// Draws the element onto the specified graphics object.
        /// </summary>
        /// <param name="g">The graphics object to draw onto.</param>
        public abstract void Draw(Rage.Graphics g);

        /// <inheritdoc/>
        public void MoveTo(Point position)
        {
            this.Position = position;
            this.UpdateBounds();
        }

        /// <inheritdoc/>
        public abstract void UpdateBounds();
    }
}
