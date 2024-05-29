using RawCanvasUI.Interfaces;
using RawCanvasUI.Keyboard;
using RawCanvasUI.Mouse;
using System.Collections.Generic;
using System.Drawing;

namespace RawCanvasUI.Elements
{
    public class EditableTextBox : TextBox, IEditable
    {
        private readonly List<IObserver> observers = new List<IObserver>();

        public EditableTextBox(string id, int x, int y, int width, int height) : base(x, y, width, height)
        {
            this.Id = id;
            this.Position = new Point(x, y);
            this.Height = height;
            this.Width = width;
            this.Caret = new Caret(this);
        }

        public EditableTextBox(string id, string text, int x, int y, int width, int height) : base(text, x, y, width, height)
        {
            this.Id = id;
            this.Text = text;
            this.Position = new Point(x, y);
            this.Height = height;
            this.Width = width;
        }

        public Caret Caret { get; private set; } = null;

        public bool IsEnabled { get; set; } = true;

        public string Id { get; }

        /// <inheritdoc/>
        public override IParent Parent
        {
            get => base.Parent;
            set
            {
                base.Parent = value;
                if (value is IWidget widget)
                {
                    widget.Add(this.Caret);
                }
            }
        }

        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                this.Caret.UpdateBounds();
            }
        }

        public void AddObserver(IObserver observer)
        {
            this.observers.Add(observer);
        }

        public void Click(Cursor cursor)
        {
        }

        public bool Contains(Cursor cursor)
        {
            return this.Bounds.Contains(cursor.Bounds.Location);
        }

        public RectangleF GetCaretBounds()
        {
            return new RectangleF();
        }

        public void HandleInput(string input)
        {
            if (input == "[Back]")
            {
                if (this.Text.Length > 0)
                {
                    this.Text = this.Text.Substring(0, this.Text.Length - 1);
                }
            }
            else
            {
                this.Text += input;
            }
        }

        public void NotifyObservers()
        {
            this.observers.ForEach(x => x.OnUpdated(this));
        }

        public void RemoveObserver(IObserver observer)
        {
            this.observers.Remove(observer);
        }

        public void SetFocus(bool isFocused)
        {
            var result = isFocused ? "true" : "false";
            Logging.Debug($"EditableTextBox was told to set focus to {result}");
            this.Caret.IsVisible = isFocused;
            this.Caret.UpdateBounds();
        }

        public override void UpdateBounds()
        {
            base.UpdateBounds();
            this.Caret.UpdateBounds();
        }
    }
}
