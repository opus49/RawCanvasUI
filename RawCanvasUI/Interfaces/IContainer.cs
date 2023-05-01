using System.Collections.Generic;

namespace RawCanvasUI.Interfaces
{
    /// <summary>
    /// Represents a container that can contain items.
    /// </summary>
    public interface IContainer : IParent, IDrawable
    {
        /// <summary>
        /// Gets the list of items contained within the container.
        /// </summary>
        List<IDrawable> Items { get; }

        /// <summary>
        /// Adds an item to the container.
        /// </summary>
        /// <param name="item">The item to add.</param>
        void Add(IDrawable item);

        /// <summary>
        /// Removes an item from the container.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        void Remove(IDrawable item);

        /// <summary>
        /// Removes all items the container.
        /// </summary>
        void Clear();
    }
}
