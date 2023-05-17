namespace RawCanvasUI
{
#pragma warning disable 1591, SA1602

    /// <summary>
    /// The cursor type.
    /// </summary>
    public enum CursorType
    {
        Default,
        Pointing,
        Scrolling,
    }

    /// <summary>
    /// The mouse status.
    /// </summary>
    public enum MouseStatus
    {
        Up,
        Down,
    }

    /// <summary>
    /// A scroll or mouse wheel status.
    /// </summary>
    public enum ScrollWheelStatus
    {
        Up,
        Down,
        None,
    }

    public enum TextAlignment
    {
        Top,
        Middle,
    }

    public enum TextJustification
    {
        Left,
        Center,
        Right,
    }

#pragma warning restore 1591, SA1602
}