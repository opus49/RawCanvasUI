using RawCanvasUI.Interfaces;
using RawCanvasUI.Mouse;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Remoting.Services;

namespace RawCanvasUI.Elements
{
    public class EditableTextBox : TextBox, IEditable
    {
        private readonly List<IObserver> observers = new List<IObserver>();
        private int caretIndex = 0;

        public EditableTextBox(string id, int x, int y, int width, int height) : base(x, y, width, height)
        {
            this.Id = id;
            this.Position = new Point(x, y);
            this.Height = height;
            this.Width = width;
        }

        public EditableTextBox(string id, string text, int x, int y, int width, int height) : base(text, x, y, width, height)
        {
            this.Id = id;
            this.Text = text;
            this.Position = new Point(x, y);
            this.Height = height;
            this.Width = width;
        }

        public bool IsEnabled { get; set; } = true;

        public string Id { get; }

        public void AddObserver(IObserver observer)
        {
            if (!this.observers.Contains(observer))
            {
                this.observers.Add(observer);
            }
        }

        public void Click(Cursor cursor)
        {
            Logging.Debug("EditableTextBox clicked!");
            this.UpdateCaretIndex(cursor);
            this.NotifyObservers();
        }

        public bool Contains(Cursor cursor)
        {
            return this.Bounds.Contains(cursor.Bounds.Location);
        }

        public RectangleF GetCaretBounds()
        {
            if (this.caretIndex > this.Text.Length)
            {
                this.caretIndex = this.Text.Length;
            }

            var text = this.caretIndex > 0 ? this.Text.Substring(0, this.caretIndex) : "";
            var textSize = Rage.Graphics.MeasureText(text, this.FontFamily, this.ScaledFontSize);
            var position = new PointF(this.TextPosition.X + textSize.Width + 2f, this.TextPosition.Y - 2f);
            return new RectangleF(position, new SizeF(this.Parent.Scale.Width, this.TextSize.Height * 1.5f));
        }

        public void HandleInput(string input)
        {
            if (input == "[Back]")
            {
                if (this.Text.Length > 0 && this.caretIndex > 0)
                {
                    this.caretIndex--;
                    this.Text = this.Text.Remove(this.caretIndex, 1);
                }
            }
            else if (input != "[Enter]")
            {
                if (this.caretIndex == this.Text.Length)
                {
                    this.Text += input;
                }
                else
                {
                    this.Text = this.Text.Insert(this.caretIndex, input);
                }
                this.caretIndex++;
            }

            this.NotifyObservers();
        }

        public void NotifyObservers()
        {
            this.observers.ForEach(x => x.OnUpdated(this));
        }

        public void RemoveObserver(IObserver observer)
        {
            this.observers.Remove(observer);
        }

        private void UpdateCaretIndex(Cursor cursor)
        {
            if (this.Text.Length == 0)
            {
                Logging.Debug("Setting caret index to zero since text length is zero");
                this.caretIndex = 0;
                return;
            }

            for (int i = 0; i <= this.Text.Length; i++)
            {
                var textSize = Rage.Graphics.MeasureText(this.Text.Substring(0, i), this.FontFamily, this.ScaledFontSize);
                if (textSize.Width + this.TextPosition.X > cursor.Bounds.X)
                {
                    this.caretIndex = i > 0 ? i - 1 : 0;
                    Logging.Debug($"Setting caret index to {this.caretIndex}");
                    return;
                }
            }

            this.caretIndex = this.Text.Length;
        }
    }
}
