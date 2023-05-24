using System.Drawing;

namespace RawCanvasUI.Interfaces
{
    /// <summary>
    /// Represents text.
    /// </summary>
    public interface IText
    {
        /// <summary>
        /// Gets or sets the disabled font color.
        /// </summary>
        Color DisabledFontColor { get; set; }

        /// <summary>
        /// Gets or sets the font color.
        /// </summary>
        Color FontColor { get; set; }

        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        string FontFamily { get; set; }

        /// <summary>
        /// Gets or sets the font size.
        /// </summary>
        float FontSize { get; set; }

        /// <summary>
        /// Gets the font size scaled to the parent widget.
        /// </summary>
        float ScaledFontSize { get; }

        /// <summary>
        /// Gets or sets the text to be drawn.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets the adjusted text size on the screen based on font size and scale.
        /// </summary>
        SizeF TextSize { get; }
    }
}