using RawCanvasUI.Mouse;

namespace RawCanvasUI.Interfaces
{
    /// <summary>
    /// Represents a scrollable control.
    /// </summary>
    public interface IScrollable : IControl
    {
        /// <summary>
        /// Gets or sets a value indicating whether or not auto scroll is enabled.
        /// </summary>
        bool IsAutoScrollEnabled { get; set; }

        /// <summary>
        /// Gets a value indicating whether or not the IScrollable is being drag scrolled.
        /// </summary>
        bool IsDragScrolling { get; }

        /// <summary>
        /// Tells the IScrollable to drag scroll to the cursor.
        /// </summary>
        /// <param name="cursor"></param>
        void DragScroll(Cursor cursor);

        /// <summary>
        /// Called when a control is potentially being scrolled.
        /// </summary>
        /// <param name="status">The status of the scroll wheel.</param>
        void Scroll(ScrollWheelStatus status);

        /// <summary>
        /// Returns a value indicating whether or not the IScrollable contains the cursor.
        /// </summary>
        /// <param name="cursor"></param>
        /// <returns></returns>
        bool ScrollbarContains(Cursor cursor);

        /// <summary>
        /// Called when the user clicks on the IScrollable.
        /// </summary>
        /// <param name="cursor"></param>
        void ScrollbarClick(Cursor cursor);

        /// <summary>
        /// Stops a drag scroll.
        /// </summary>
        void StopDragScrolling();
    }
}
