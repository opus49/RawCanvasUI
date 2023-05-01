using Rage;
using RawCanvasUI.Util;
using RawCanvasUI.Widgets;

namespace RawCanvasUI.Widgets
{
    /// <summary>
    /// Represents a texture based widget.
    /// </summary>
    public class TextureWidget : BaseWidget
    {
        private readonly bool isDynamic;
        private Texture texture = null;
        private string textureName;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureWidget"/> class with fixed dimensions relative to the canvas.
        /// </summary>
        /// <param name="textureName">The name of the texture.</param>
        /// <param name="width">The fixed width relative to the canvas.</param>
        /// <param name="height">The fixed height relative to the canvas.</param>
        public TextureWidget(string textureName, int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.textureName = textureName;
            this.isDynamic = height == 0 && width == 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureWidget"/> class with dimensions based on the underyling texture.
        /// </summary>
        /// <param name="textureName">The name of the texture.</param>
        public TextureWidget(string textureName)
            : this(textureName, 0, 0)
        {
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
        public override void Draw(Graphics g)
        {
            if (this.Texture != null)
            {
                g.DrawTexture(this.Texture, this.Bounds);
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

        /// <inheritdoc/>
        public override void UpdateBounds()
        {
            if (this.isDynamic && this.Texture != null)
            {
                this.Height = this.Texture.Size.Height;
                this.Width = this.Texture.Size.Width;
            }

            base.UpdateBounds();
        }
    }
}