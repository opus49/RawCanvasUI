using System.Drawing;
using RawCanvasUI.Interfaces;
using RawCanvasUI;
using RawCanvasUI.Mouse;

namespace RawCanvasUI.Interfaces
{
    /// <summary>
    /// Represents a container that can be moved and resized.
    /// </summary>
    public interface IWidget : IContainer, IGeometry
    {
        /// <summary>
        /// Gets the offset between the mouse cursor and the top-left corner of the element when being dragged.
        /// </summary>
        PointF DragOffset { get; }

        /// <summary>
        /// Gets a value indicating whether the element is currently being dragged.
        /// </summary>
        bool IsDragging { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the element is currently being hovered over by the mouse cursor.
        /// </summary>
        bool IsHovered { get; set; }

        /// <summary>
        /// Gets a value for the widget specific scale.
        /// </summary>
        float WidgetScale { get; }

        /// <summary>
        /// Gets a value indicating whether or not the cursor resides within the bounds of the widget.
        /// </summary>
        /// <param name="cursor">The cursor object.</param>
        /// <returns>True if the cursor resides within the bounds of the widget, otherwise false.</returns>
        bool Contains(Cursor cursor);

        /// <summary>
        /// Called when the element is being dragged.
        /// </summary>
        /// <param name="mousePosition">The current position of the mouse cursor.</param>
        void Drag(PointF mousePosition);

        /// <summary>
        /// Safely sets the widget's scale.
        /// </summary>
        /// <param name="scale">The widget specific scale where 1.0 is native.</param>
        void SetWidgetScale(float scale);

        /// <summary>
        /// Called when the element starts being dragged.
        /// </summary>
        /// <param name="mousePosition">The position of the mouse cursor when the drag started.</param>
        void StartDrag(PointF mousePosition);

        /// <summary>
        /// Called when the element stops being dragged.
        /// </summary>
        void StopDrag();
    }
}