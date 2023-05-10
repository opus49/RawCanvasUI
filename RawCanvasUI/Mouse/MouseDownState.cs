using RawCanvasUI.Elements;
using RawCanvasUI.Interfaces;
using RawCanvasUI.Util;

namespace RawCanvasUI.Mouse
{
    /// <summary>
    /// Represents the mouse in a down state.
    /// </summary>
    internal class MouseDownState : MouseState
    {
        /// <inheritdoc/>
        public override void UpdateWidgets(Cursor cursor, WidgetManager widgetManager)
        {
            if (cursor.MouseStatus != MouseStatus.Down)
            {
                this.ReleaseMouse(widgetManager);
            }
            else
            {
                this.PressMouse(widgetManager, cursor);
            }
        }

        private void ReleaseMouse(WidgetManager widgetManager)
        {
            if (widgetManager.PressedControl != null)
            {
                if (widgetManager.PressedControl is IScrollable scrollable && scrollable.IsDragScrolling)
                {
                    scrollable.StopDragScrolling();
                }
                else if (widgetManager.PressedControl is TextureButton button)
                {
                    button.Release();
                }

                widgetManager.PressedControl = null;
            }

            if (widgetManager.PressedWidget != null)
            {
                if (widgetManager.PressedWidget.IsDragging)
                {
                    widgetManager.PressedWidget.StopDrag();
                }

                widgetManager.PressedWidget = null;
            }

            widgetManager.SetMouseState(new MouseUpState());
        }

        private void PressMouse(WidgetManager widgetManager, Cursor cursor)
        {
            var widget = widgetManager.PressedWidget;
            if (widget == null || widgetManager.PressedControl is IClickable)
            {
                return;
            }

            if (widgetManager.PressedControl is IScrollable scrollable)
            {
                if (scrollable.IsDragScrolling)
                {
                    scrollable.DragScroll(cursor);
                }
            }
            else if (widget.IsDragging)
            {
                widget.Drag(cursor.Position);
                if (cursor.ScrollWheelStatus != ScrollWheelStatus.None)
                {
                    widget.SetWidgetScale(widget.WidgetScale + (cursor.ScrollWheelStatus == ScrollWheelStatus.Up ? Constants.RescaleIncrement : -Constants.RescaleIncrement));
                }
            }
            else
            {
                if (widget.Contains(cursor))
                {
                    if (cursor.ClickDuration > cursor.LongClickDuration)
                    {
                        widget.StartDrag(cursor.Position);
                        widgetManager.BringToFront(widget);
                    }
                }
                else
                {
                    widgetManager.PressedWidget = null;
                }
            }
        }
    }
}
