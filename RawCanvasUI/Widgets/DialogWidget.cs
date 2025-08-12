using Rage;
using RawCanvasUI.Elements;
using RawCanvasUI.Events;
using RawCanvasUI.Interfaces;
using System.Drawing;

namespace RawCanvasUI.Widgets
{
    public class DialogWidget : RectangleWidget, IModal, IObserver
    {
        private readonly Label messageLabel = new Label(string.Empty, 20, 65);
        private readonly RectangleButton okButton = new RectangleButton("ok", 60, 30, "OK");

        public DialogWidget(string canvasUUID, string message) : base(300, 150)
        {
            this.BackgroundColor = Color.FromArgb(112, 112, 112);
            this.Add(this.okButton);
            this.Add(this.messageLabel);
            this.messageLabel.Text = message;
            this.messageLabel.FontColor = Color.Black;
            this.messageLabel.FontSize = 18;
            this.okButton.BackgroundColor = Color.FromArgb(64, 64, 64);
            this.okButton.FontColor = Color.FromArgb(224, 224, 224);
            this.okButton.MoveTo(new Point(120, 115));
            this.CanvasUUID = canvasUUID;
        }

        public string Result { get; private set; } = string.Empty;

        public string CanvasUUID { get; private set; }

        public void Center()
        {
            this.MoveTo(new Point((int)Constants.CanvasWidth / 2 - this.Width / 2, (int)Constants.CanvasHeight / 2 - this.Height / 2));
        }

        public void Dispose()
        {
            ModalEventHandler.RaiseDisposeModal(this);
        }

        public void Hide()
        {
            this.IsVisible = false;
        }

        public override void OnUpdated(IObservable obj)
        {
            if (obj.Id == "ok")
            {
                this.Hide();
            }
        }

        public void Show()
        {
            this.IsVisible = true;
            Logging.Debug("DialogWidget calling RaiseShowModal");
            ModalEventHandler.RaiseShowModal(this);
        }
    }
}
