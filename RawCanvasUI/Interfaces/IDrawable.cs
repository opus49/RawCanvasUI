namespace RawCanvasUI.Interfaces
{
    /// <summary>
    /// Represents an item that can be drawn to the screen.
    /// </summary>
    public interface IDrawable : ISpatial
    {
        /// <summary>
        /// Gets or sets a value indicating whether this item is visible.
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the parent container of this item.
        /// </summary>
        IParent Parent { get; set; }

        /// <summary>
        /// Draws the item to the specified graphics object.
        /// </summary>
        /// <param name="g">The graphics object to draw to.</param>
        void Draw(Rage.Graphics g);

        /// <summary>
        /// Updates the bounds based on the internal rules of the drawable.
        /// </summary>
        void UpdateBounds();
    }
}
