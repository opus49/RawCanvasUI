using System;
using System.ComponentModel;
using System.Drawing;
using RawCanvasUI.Interfaces;
using RawCanvasUI.Style;

namespace RawCanvasUI.Elements
{
    /// <summary>
    /// Represents a drawable element.
    /// </summary>
    public abstract class BaseElement : IDrawable
    {
        /// <inheritdoc/>
        public RectangleF Bounds { get; protected set; } = default;

        /// <inheritdoc/>
        public virtual bool IsVisible { get; set; } = true;

        /// <inheritdoc/>
        public IParent Parent { get; set; }

        /// <inheritdoc/>
        public Point Position { get; protected set; } = default;

        /// <inheritdoc/>
        public string StyleName { get; set; } = string.Empty;

        /// <inheritdoc/>
        public void ApplyStyle(Stylesheet stylesheet)
        {
            if (string.IsNullOrEmpty(StyleName))
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

        /// <summary>
        /// Draws the element onto the specified graphics object.
        /// </summary>
        /// <param name="g">The graphics object to draw onto.</param>
        public abstract void Draw(Rage.Graphics g);

        /// <inheritdoc/>
        public void MoveTo(Point position)
        {
            this.Position = position;
            this.UpdateBounds();
        }

        /// <inheritdoc/>
        public abstract void UpdateBounds();
    }
}
