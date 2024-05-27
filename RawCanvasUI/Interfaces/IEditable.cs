using RawCanvasUI.Mouse;

namespace RawCanvasUI.Interfaces
{
    public  interface IEditable : IFocusable
    {
        void HandleInput(string input);
    }
}
