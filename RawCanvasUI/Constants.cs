using System.Collections.Generic;

namespace RawCanvasUI
{
    /// <summary>
    /// A list of project wide constant values.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The width of the canvas used for GUI elements.
        /// </summary>
        public const float CanvasWidth = 1920f;

        /// <summary>
        /// The height of the canvas used for GUI elements.
        /// </summary>
        public const float CanvasHeight = 1080f;

        /// <summary>
        /// A lookup table for font leadings.
        /// </summary>
        public static readonly Dictionary<string, float> Leading = new Dictionary<string, float>()
        {
            { "Arial", 0.20f },
            { "Bahnschrift", 0.16f },
            { "Calibri", 0.3f },
            { "Candara", 0.21f },
            { "Comic Sans MS", 0.23f },
            { "Consolas", 0.24f },
            { "Constantia", 0.23f },
            { "Corbel", 0.24f },
            { "Courier New", 0.24f },
            { "Franklin Gothic Medium", 0.3f },
            { "Georgia", 0.15f },
            { "Impact", 0.20f },
            { "Lucida Console", 0f },
            { "Microsoft Sans Serif", 0.19f },
            { "Nirmala UI", 0.31f },
            { "Segoe UI", 0.31f },
            { "SimSun", 0.25f },
            { "Tahoma", 0.2f },
            { "Times New Roman", 0.27f },
            { "Trebuchet MS", 0.17f },
            { "Verdana", 0.21f },
        };

        /// <summary>
        /// The length of time in milliseconds that constitutes a long click for dragging.
        /// </summary>
        public const long LongClickDuration = 150;

        /// <summary>
        /// The minimum allowable scaling factor (as a percentage).
        /// </summary>
        public const float MinScale = 0.1f;

        /// <summary>
        /// The maximum allowable scaling factor (as a percentage).
        /// </summary>
        public const float MaxScale = 2f;

        /// <summary>
        /// How much to rescale when resizing widgets with the scrollwheel.
        /// </summary>
        public const float RescaleIncrement = 0.01f;
    }
}
