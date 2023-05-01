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
