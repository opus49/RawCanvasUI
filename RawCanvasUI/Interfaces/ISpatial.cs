using System.Drawing;

namespace RawCanvasUI.Interfaces
{
    /// <summary>
    /// Represents a spatial object that has a position on a fixed canvas and bounds on a screen of variable resolution.
    /// </summary>
    public interface ISpatial
    {
        /// <summary>
        /// Gets the real screen area where the object is located.
        /// </summary>
        RectangleF Bounds { get; }

        /// <summary>
        /// Gets the canvas position of the item.
        /// </summary>
        Point Position { get; }

        /// <summary>
        ///  Moves the item to the given coordinates on the canvas.
        /// </summary>
        /// <param name="position">The base canvas coordinates.</param>
        void MoveTo(Point position);
    }
}
