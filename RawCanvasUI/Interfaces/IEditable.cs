using RawCanvasUI.Mouse;

namespace RawCanvasUI.Interfaces
{
    public  interface IEditable : IControl, IObservable
    {
        /// <summary>
        /// Executed when the user edits the control.
        /// </summary>
        void Edit(Cursor cursor);
    }
}
