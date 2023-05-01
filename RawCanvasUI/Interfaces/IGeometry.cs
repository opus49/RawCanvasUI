namespace RawCanvasUI.Interfaces
{
    /// <summary>
    /// Represents a 2D geometric shape.
    /// </summary>
    public interface IGeometry
    {
        /// <summary>
        /// Gets the unscaled height relative to the canvas.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Gets the unscaled width relative to the canvas.
        /// </summary>
        int Width { get; }
    }
}
