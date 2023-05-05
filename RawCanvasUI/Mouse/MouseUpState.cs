using RawCanvasUI.Interfaces;
using RawCanvasUI.Util;

namespace RawCanvasUI.Mouse
{
    /// <summary>
    /// Represents the mouse in an up state.
    /// </summary>
    internal class MouseUpState : MouseState
    {
        /// <inheritdoc/>
        public override void UpdateWidgets(Cursor cursor, WidgetManager widgetManager)
        {
            if (cursor.MouseStatus != MouseStatus.Up)
            {
                this.PressMouse(cursor, widgetManager);
            }
            else
            {
                this.HandleMouse(cursor, widgetManager);
            }
        }

        private void PressMouse(Cursor cursor, WidgetManager widgetManager)
        {
            widgetManager.UpdateHoveredWidget(cursor);
            widgetManager.UpdateHoveredControl(cursor);
            widgetManager.PressedWidget = widgetManager.HoveredWidget;

            if (widgetManager.HoveredControl is IClickable clickable)
            {
                Logging.Debug($"user pressed mouse on IClickable: {clickable.GetType()}");
                clickable.Click(cursor);
                widgetManager.PressedControl = widgetManager.HoveredControl;
            }
            else if (widgetManager.HoveredControl is IScrollable scrollable && scrollable.ScrollbarContains(cursor))
            {
                Logging.Debug($"user pressed mouse on IScrollable that contains cursor: {scrollable.GetType()}");
                scrollable.ScrollbarClick(cursor);
                widgetManager.PressedControl = widgetManager.HoveredControl;
            }
            else if (widgetManager.HoveredControl is ISelectable selectable)
            {
                Logging.Debug($"user pressed on ISelectable: {selectable.GetType()}");
                selectable.Select(cursor);
                widgetManager.PressedControl = null;
            }
            else
            {
                if (widgetManager.HoveredControl != null)
                {
                    Logging.Debug($"user pressed mouse on: {widgetManager.HoveredControl.GetType()}");
                }
                else
                {
                    Logging.Debug("user pressed mouse but it wasn't over a control");
                }

                widgetManager.PressedControl = null;
            }

            widgetManager.SetMouseState(new MouseDownState());
        }

        private void HandleMouse(Cursor cursor, WidgetManager widgetManager)
        {
            widgetManager.UpdateHoveredWidget(cursor);
            widgetManager.UpdateHoveredControl(cursor);

            if (widgetManager.HoveredControl != null)
            {
                if (widgetManager.HoveredControl is IClickable)
                {
                    cursor.SetCursorType(CursorType.Pointing);
                }
                else if (widgetManager.HoveredControl is IScrollable scrollable)
                {
                    scrollable.Scroll(cursor.ScrollWheelStatus);
                }
            }
            else
            {
                cursor.SetCursorType(CursorType.Default);
            }
        }
    }
}
