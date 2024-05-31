using RawCanvasUI.Interfaces;
using RawCanvasUI.Mouse;
using System.Collections.Generic;
using System.Drawing;

namespace RawCanvasUI.Elements
{
    public class EditableTextBox : TextBox, IEditable
    {
        private readonly List<IObserver> observers = new List<IObserver>();
        private int caretIndex = 0;
        private int maxVisibleTextLength = -1;
        private int startingVisibleIndex = 0;

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

        public string VisibleText
        {
            get
            {
                if (this.maxVisibleTextLength < 0)
                {
                    this.UpdateMaxVisibleTextLength();
                }

                var length = System.Math.Min(this.maxVisibleTextLength, this.Text.Length - this.startingVisibleIndex);
                return length == 0 ? "" : this.Text.Substring(this.startingVisibleIndex, length);
            }
        }

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

        /// <inheritdoc/>
        public override void Draw(Rage.Graphics g)
        {
            g.DrawRectangle(this.BorderBounds, this.BorderColor);
            g.DrawRectangle(this.Bounds, this.BackgroundColor);
            g.DrawText(this.VisibleText, this.FontFamily, this.ScaledFontSize, this.TextPosition, this.FontColor, this.Bounds);
        }

        public RectangleF GetCaretBounds()
        {
            if (this.caretIndex > this.VisibleText.Length)
            {
                this.caretIndex = this.VisibleText.Length;
            }

            var text = this.caretIndex > 0 ? this.VisibleText.Substring(0, this.caretIndex) : "";
            var textSize = Rage.Graphics.MeasureText(text, this.FontFamily, this.ScaledFontSize);
            var position = new PointF(this.TextPosition.X + textSize.Width + 2f, this.TextPosition.Y - 2f);
            return new RectangleF(position, new SizeF(this.Parent.Scale.Width, this.TextSize.Height * 1.5f));
        }

        public void HandleInput(string input)
        {
            switch (input)
            {
                case "[Back]":
                    if (this.caretIndex > 0)
                    {
                        this.caretIndex--;
                        this.Text = this.Text.Remove(this.startingVisibleIndex + this.caretIndex, 1);
                    }
                    else
                    {
                        if (this.startingVisibleIndex > 0)
                        {
                            this.startingVisibleIndex--;
                            this.Text = this.Text.Remove(this.startingVisibleIndex, 1);
                        }
                    }
                    break;
                case "[Left]":
                    if (this.caretIndex > 0)
                    {
                        this.caretIndex--;
                    }
                    else if (this.startingVisibleIndex > 0)
                    {
                        this.startingVisibleIndex--;
                    }
                    break;
                case "[Right]":
                    if (this.caretIndex < this.VisibleText.Length)
                    {
                        this.caretIndex++;
                    }
                    else if (this.Text.Length - (this.maxVisibleTextLength + this.startingVisibleIndex) > 0)
                    {
                        this.startingVisibleIndex++;
                    }
                    break;
                case "[Delete]":
                    if (this.Text.Length > this.startingVisibleIndex + this.caretIndex)
                    {
                        this.Text = this.Text.Remove(this.startingVisibleIndex + this.caretIndex, 1);
                    }
                    break;
                case "[Enter]":
                    break;
                default:
                    var index = this.caretIndex + this.startingVisibleIndex;
                    this.Text = index < this.Text.Length ? this.Text.Insert(index, input) : this.Text + input;

                    if (this.caretIndex < this.maxVisibleTextLength)
                    {
                        this.caretIndex++;
                    }
                    else
                    {
                        this.startingVisibleIndex++;
                    }
                    break;
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

        private void UpdateMaxVisibleTextLength()
        {
            var text = "M";
            while (this.TextPosition.X + Rage.Graphics.MeasureText(text, this.FontFamily, this.ScaledFontSize).Width + (2 * this.Parent.Scale.Width) < this.Bounds.X + this.Bounds.Width)
            {
                text += "M";
            }

            this.maxVisibleTextLength = text.Length > 0 ? text.Length - 1 : 0;
        }

        private void UpdateCaretIndex(Cursor cursor)
        {
            if (cursor == null)
            {
                this.caretIndex = this.Text.Length;
                return;
            }

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
