namespace RawCanvasUI.Elements
{
    /// <summary>
    /// An implementation of a drawable texture.
    /// </summary>
    public class Sprite : TextureElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> class.
        /// </summary>
        /// <param name="textureName">The name of the texture.</param>
        public Sprite(string textureName)
            : base(textureName, 0, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> class.
        /// </summary>
        /// <param name="textureName">The name of the texture.</param>
        /// <param name="width">The width of the sprite relative to the canvas.</param>
        /// <param name="height">The height of the sprite relative to the canvas.</param>
        public Sprite(string textureName, int width, int height)
            : base(textureName, width, height)
        {
        }
    }
}
