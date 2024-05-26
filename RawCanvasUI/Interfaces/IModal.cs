namespace RawCanvasUI.Interfaces
{
    public interface IModal : IWidget
    {
        string CanvasUUID { get; }

        string Result { get; }

        void Center();

        void Dispose();

        void Hide();

        void Show();
    }
}
