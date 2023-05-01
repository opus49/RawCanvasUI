namespace RawCanvasUI.Interfaces
{
    /// <summary>
    /// Represents a scrollable control.
    /// </summary>
    public interface IScrollable : IControl
    {
        /// <summary>
        /// Called when a control is potentially being scrolled.
        /// </summary>
        /// <param name="status">The status of the scroll wheel.</param>
        void Scroll(ScrollWheelStatus status);
    }
}
