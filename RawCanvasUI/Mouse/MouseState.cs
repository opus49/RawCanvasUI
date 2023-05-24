using RawCanvasUI.Util;

namespace RawCanvasUI.Mouse
{
    /// <summary>
    /// Represents the state of the mouse.
    /// </summary>
    internal abstract class MouseState
    {
        /// <summary>
        /// Updates the widget manager based on the mouse state.
        /// </summary>
        /// <param name="cursor">The cursor containing the mouse data.</param>
        /// <param name="widgetManager">The widget manager containing the widgets.</param>
        public abstract void UpdateWidgets(Cursor cursor, WidgetManager widgetManager);
    }
}
