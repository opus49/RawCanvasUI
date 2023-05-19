using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using RawCanvasUI.Interfaces;
using RawCanvasUI.Mouse;
using RawCanvasUI.Style;

namespace RawCanvasUI.Widgets
{
    /// <summary>
    /// Represents a base widget.
    /// </summary>
    public abstract class BaseWidget : IObserver, IWidget
    {
        private float widgetScale = 1f;
        private Point position = default;
        private Point maximizedPosition = default;

        /// <inheritdoc/>
        public RectangleF Bounds { get; protected set; } = default;

        /// <inheritdoc/>
        public Rectangle DragArea { get; protected set; } = default;

        /// <inheritdoc/>
        public RectangleF DragAreaBounds { get; protected set; } = default;

        /// <inheritdoc/>
        public PointF DragOffset { get; protected set; } = new PointF(0, 0);

        /// <inheritdoc/>
        public int Height { get; protected set; }

        /// <inheritdoc/>
        public bool IsDragging { get; protected set; } = false;

        /// <inheritdoc/>
        public bool IsHovered { get; set; } = false;

        /// <inheritdoc/>
        public bool IsMaximized { get; protected set; } = false;

        /// <inheritdoc/>
        public virtual bool IsVisible { get; set; } = true;

        /// <inheritdoc/>
        public List<IDrawable> Items { get; protected set; } = new List<IDrawable>();

        /// <inheritdoc/>
        public IParent Parent { get; set; } = null;

        /// <inheritdoc/>
        public Point Position
        {
            get
            {
                return this.IsMaximized ? maximizedPosition : position;
            }
        }

        /// <inheritdoc/>
        public SizeF Scale
        {
            get
            {
                if (this.Parent != null)
                {
                    return new SizeF(this.Parent.Scale.Width * this.WidgetScale, this.Parent.Scale.Height * this.WidgetScale);
                }

                return default;
            }
        }

        /// <inheritdoc/>
        public string StyleName { get; set; } = string.Empty;

        /// <inheritdoc/>
        public string UUID
        {
            get
            {
                if (this.Parent != null)
                {
                    return this.Parent.UUID;
                }

                return System.Guid.Empty.ToString();
            }
        }

        /// <inheritdoc/>
        public float WidgetScale 
        {
            get
            {
                if (this.Parent is Canvas && this.IsMaximized)
                {
                    return Constants.MaxScale;
                }

                return this.widgetScale;
            }
        }

        /// <inheritdoc/>
        public int Width { get; protected set; }

        /// <inheritdoc/>
        public virtual void Add(IDrawable item)
        {
            item.Parent = this;
            this.Items.Add(item);
            if (item is IObservable observable)
            {
                observable.AddObserver(this);
            }
        }

        /// <inheritdoc/>
        public void ApplyStyle(Stylesheet stylesheet)
        {
            this.Items.ForEach(x => x.ApplyStyle(stylesheet));
            if (string.IsNullOrEmpty(this.StyleName))
            {
                return;
            }

            var styleProperties = stylesheet.GetStyle(this.StyleName);
            if (styleProperties != null)
            {
                foreach (var property in styleProperties)
                {
                    var propertyInfo = this.GetType().GetProperty(property.Key);
                    if (propertyInfo != null)
                    {
                        try
                        {
                            var parsedValue = TypeDescriptor.GetConverter(propertyInfo.PropertyType).ConvertFromString(property.Value);
                            propertyInfo.SetValue(this, parsedValue);
                        }
                        catch (Exception ex)
                        {
                            Logging.Error($"Failed to parse value {property.Value} for property {property.Key}", ex);
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        public virtual void Clear()
        {
            this.Items.Clear();
        }

        /// <inheritdoc/>
        public virtual bool Contains(Cursor cursor)
        {
            return this.Bounds.Contains(cursor.Bounds.Location);
        }

        /// <inheritdoc/>
        public virtual bool ContainsInDragArea(Cursor cursor)
        {
            if (this.Parent is Canvas && !this.IsMaximized)
            {
                return this.DragAreaBounds.Contains(cursor.Bounds.Location);
            }

            return false;
        }

        /// <inheritdoc/>
        public void Drag(PointF mousePosition)
        {
            this.MoveTo(new Point((int)System.Math.Round(mousePosition.X + this.DragOffset.X), (int)System.Math.Round(mousePosition.Y + this.DragOffset.Y)));
        }

        /// <inheritdoc/>
        public virtual void Draw(Rage.Graphics g)
        {
            this.Items.Where(x => x.IsVisible).ToList().ForEach(x => x.Draw(g));
        }

        /// <inheritdoc/>
        public void MoveTo(Point position)
        {
            this.position = position;
            this.UpdateBounds();
        }

        /// <inheritdoc />
        public virtual void OnUpdated(IObservable obj)
        {
        }

        /// <inheritdoc/>
        public virtual void Remove(IDrawable item)
        {
            this.Items.Remove(item);
        }

        public virtual void SetMaximized(bool isMaximized)
        {
            this.IsMaximized = isMaximized;
            this.UpdateBounds();
        }

        /// <inheritdoc/>
        public void SetWidgetScale(float scale)
        {
            if (this.Parent is Canvas && !this.IsMaximized)
            {
                this.widgetScale = scale < Constants.MinScale ? Constants.MinScale : (scale > Constants.MaxScale ? Constants.MaxScale : scale);
                this.UpdateBounds();
            }
        }

        /// <inheritdoc/>
        public void StartDrag(PointF mousePosition)
        {
            this.IsDragging = true;
            this.DragOffset = new PointF(this.Position.X - mousePosition.X, this.Position.Y - mousePosition.Y);
        }

        /// <inheritdoc/>
        public void StopDrag()
        {
            this.IsDragging = false;
            this.DragOffset = default;
        }

        /// <inheritdoc/>
        public virtual void UpdateBounds()
        {
            if (this.Parent == null)
            {
                Logging.Warning("cannot update bounds on parentless widget");
                return;
            }

            if (this.Parent is Canvas canvas)
            {
                this.maximizedPosition = new Point((1920 - (this.Width * 2)) / 2, (1080 - (this.Height * 2)) / 2);
                float x = this.Position.X * canvas.Scale.Width;
                float y = this.Position.Y * canvas.Scale.Height;
                var size = new SizeF(this.Width * this.Scale.Height, this.Height * this.Scale.Height);
                this.Bounds = new RectangleF(new PointF(x, y), size);

                float dragX = this.Bounds.X + (this.DragArea.X * this.Scale.Height);
                float dragY = this.Bounds.Y + (this.DragArea.Y * this.Scale.Height);
                var dragSize = new SizeF(this.DragArea.Width * this.Scale.Height, this.DragArea.Height * this.Scale.Height);
                this.DragAreaBounds = new RectangleF(new PointF(dragX, dragY), dragSize);
            }
            else
            {
                float x = this.Parent.Bounds.X + (this.Position.X * this.Parent.Scale.Height);
                float y = this.Parent.Bounds.Y + (this.Position.Y * this.Parent.Scale.Height);
                var size = new SizeF(this.Width * this.Parent.Scale.Height, this.Height * this.Parent.Scale.Height);
                this.Bounds = new RectangleF(new PointF(x, y), size);
            }

            foreach (IDrawable item in this.Items)
            {
                item.UpdateBounds();
            }
        }
    }
}
