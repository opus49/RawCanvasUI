using System.Drawing;

namespace RawCanvasUI.Interfaces
{
    public  interface IEditable : IClickable
    {
        RectangleF GetCaretBounds();

        void HandleInput(string input);
    }
}
