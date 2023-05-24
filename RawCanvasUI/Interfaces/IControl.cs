using RawCanvasUI.Mouse;

namespace RawCanvasUI.Interfaces
{
    /// <summary>
    /// Represents an object that can be used for controlling things like a button.
    /// </summary>
    public interface IControl : IDrawable
    {
        /// <summary>
        /// Gets or sets a value indicating whether the element is enabled and can respond to user interaction.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets a value indicating whether or not the cursor resides within the bounds of the widget.
        /// </summary>
        /// <param name="cursor">The cursor object.</param>
        /// <returns>True if the cursor resides within the bounds of the widget, otherwise false.</returns>
        bool Contains(Cursor cursor);
    }
}
