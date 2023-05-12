using Rage;
using RawCanvasUI.Util;
using System.Drawing;

namespace RawCanvasUI.Widgets
{
    /// <summary>
    /// Represents a texture based widget.
    /// </summary>
    public class TextureWidget : BaseWidget
    {
        private Texture texture = null;
        private string textureName;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureWidget"/> class with fixed dimensions relative to the canvas.
        /// </summary>
        /// <param name="textureName">The name of the texture.</param>
        /// <param name="width">The fixed width relative to the canvas.</param>
        /// <param name="height">The fixed height relative to the canvas.</param>
        /// <param name="dragArea">The grabbable area for dragging relative to the widget.</param>
        public TextureWidget(string textureName, int width, int height, Rectangle dragArea = default)
        {
            this.Width = width;
            this.Height = height;
            this.DragArea = dragArea;
            this.textureName = textureName;
        }

        /// <summary>
        /// Gets the underlying texture for the widget.
        /// </summary>
        public Texture Texture
        {
            get
            {
                if (this.texture == null && this.textureName != string.Empty)
                {
                    this.texture = TextureHandler.Get(this.UUID, this.textureName);
                }

                return this.texture;
            }
        }

        /// <inheritdoc/>
        public override void Draw(Rage.Graphics g)
        {
            if (this.Texture != null)
            {
                g.DrawTexture(this.Texture, this.Bounds);
            }
            else
            {
                g.DrawRectangle(this.Bounds, Color.LimeGreen);
            }

            base.Draw(g);
        }

        /// <summary>
        /// Sets the texture name.
        /// </summary>
        /// <param name="textureName">The name of the texture to use.</param>
        public void SetTextureName(string textureName)
        {
            this.textureName = textureName;
            this.texture = null;
        }
    }
}