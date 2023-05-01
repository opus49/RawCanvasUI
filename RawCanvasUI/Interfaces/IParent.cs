using Rage;
using System.Drawing;

namespace RawCanvasUI.Interfaces
{
    /// <summary>
    /// Represents a Parent used by drawables to determine their positioning.
    /// </summary>
    public interface IParent : ISpatial
    {
        /// <summary>
        /// Gets the scaling factor to be applied to the parent and its children.
        /// </summary>
        SizeF Scale { get; }

        /// <summary>
        /// Gets the universally unique identifier.  The Canvas should generate this and propopate it through other parents.
        /// </summary>
        string UUID { get; }
    }
}
