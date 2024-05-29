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
        /// The length of time in milliseconds to blink the caret on and off.
        /// </summary>
        public const long CaretBlinkRate = 500;

        /// <summary>
        /// A lookup table for font leadings.
        /// </summary>
        public static readonly Dictionary<string, float> Leading = new Dictionary<string, float>()
        {
            { "Arial", 0.21f },
            { "Bahnschrift", 0.22f },
            { "Calibri", 0.31f },
            { "Candara", 0.24f },
            { "Comic Sans MS", 0.22f },
            { "Consolas", 0.24f },
            { "Constantia", 0.23f },
            { "Corbel", 0.25f },
            { "Courier New", 0.23f },
            { "Franklin Gothic Medium", 0.31f },
            { "Georgia", 0.17f },
            { "Impact", 0.20f },
            { "Lucida Console", 0.1f },
            { "Microsoft Sans Serif", 0.18f },
            { "Nirmala UI", 0.31f },
            { "Segoe UI", 0.32f },
            { "SimSun", 0.26f },
            { "Tahoma", 0.21f },
            { "Times New Roman", 0.26f },
            { "Trebuchet MS", 0.175f },
            { "Verdana", 0.22f },
        };

        /// <summary>
        /// The length of time in milliseconds that constitutes a long click for dragging.
        /// </summary>
        public const long LongClickDuration = 150;

        /// <summary>
        /// The length of time in milliseconds that constitutes a long keypress.
        /// </summary>
        public const long LongKeypressDuration = 500;

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
