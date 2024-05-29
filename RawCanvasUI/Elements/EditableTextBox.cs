using RawCanvasUI.Interfaces;
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
            this.observers.Add(observer);
        }

        public void Click(Cursor cursor)
        {
            Logging.Debug("EditableTextBox clicked!");
        }

        public bool Contains(Cursor cursor)
        {
            return this.Bounds.Contains(cursor.Bounds.Location);
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
        }
    }
}
