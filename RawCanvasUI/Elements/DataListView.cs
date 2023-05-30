using RawCanvasUI.Interfaces;
using RawCanvasUI.Mouse;
using RawCanvasUI.Style;
using System.Collections.Generic;
using System.Drawing;

namespace RawCanvasUI.Elements
{
    public class DataListView<T> : TextArea, IObservable, IDataView<T>, ISelectable<T>
        where T : class
    {
        private readonly List<IObserver> observers = new List<IObserver>();

        internal delegate void OnSelectionEventHandler(DataListView<T> view);
        internal event OnSelectionEventHandler OnSelection;

        private T selectedItem = null;

        public DataListView(string id, int x, int y, int width, int height) 
            : base(x, y, width, height)
        {
            this.Id = id;
        }

        /// <inheritdoc/>
        public string Id { get; }

        /// <summary>
        /// Gets or sets the background color of the selected item.
        /// </summary>
        public Color HighlightBackgroundColor { get; set; } = Defaults.HighlightBackgroundColor;

        /// <summary>
        /// Gets or sets the font color of the selected item.
        /// </summary>
        public Color HighlightFontColor { get; set; } = Defaults.HighlightFontColor;

        /// <inheritdoc/>
        public void NotifyObservers()
        {
            this.observers.ForEach(x => x.OnUpdated(this));
        }

        /// <inheritdoc/>
        public void Reset()
        {
            this.ClearText();
        }

        /// <inheritdoc/>
        public int SelectedIndex { get; protected set; } = -1;

        /// <inheritdoc/>
        public T SelectedItem
        {
            get => this.selectedItem;
            set
            {
                if (this.selectedItem != value)
                {
                    this.selectedItem = value;
                    this.NotifyObservers();
                }
            }
        }

        /// <inheritdoc/>
        public override void Add(string text)
        {
            base.Add(text);
            this.NotifyObservers();
        }

        /// <inheritdoc/>
        public void AddObserver(IObserver observer)
        {
            this.observers.Add(observer);
        }

        /// <inheritdoc/>
        public override void Draw(Rage.Graphics g)
        {
            if (this.BorderWidth > 0)
            {
                g.DrawRectangle(this.BorderBounds, this.BorderColor);
            }

            g.DrawRectangle(this.Bounds, this.BackgroundColor);
            List<string> snapshot = new List<string>(this.Lines);
            for (int i = this.FirstLineIndex; i < snapshot.Count; i++)
            {
                if (i >= this.FirstLineIndex + this.MaxLines)
                {
                    break;
                }

                PointF linePosition = this.GetLinePosition(i - this.FirstLineIndex);
                if (i == this.SelectedIndex)
                {
                    g.DrawRectangle(this.GetLineBounds(i - this.FirstLineIndex), this.HighlightBackgroundColor);
                }

                g.DrawText(snapshot[i].ToString(), this.FontFamily, this.ScaledFontSize, linePosition, i == this.SelectedIndex ? this.HighlightFontColor : this.FontColor, this.Bounds);
            }

            if (this.ScrollbarWidth > 0)
            {
                this.DrawScrollbar(g);
            }
        }

        /// <inheritdoc/>
        public void NewItem(int index, T item)
        {
            this.Add(item.ToString());
        }

        /// <summary>
        /// Removes an item from the list view.
        /// </summary>
        /// <param name="index">The index of the item to remove.</param>
        /// <param name="item">The item to remove.</param>
        public void RemoveItem(int index, T item)
        {
            base.Remove(index);
            if (this.SelectedIndex != -1)
            {
                if (index < this.SelectedIndex)
                {
                    this.SelectedIndex--;
                }
                else if (index == this.SelectedIndex)
                {
                    this.SelectedIndex = -1;
                }

                this.OnSelection(this);
            }

            this.NotifyObservers();
        }

        /// <inheritdoc/>
        public void RemoveObserver(IObserver observer)
        {
            this.observers.Remove(observer);
        }

        /// <inheritdoc/>
        public virtual void Select(Cursor cursor)
        {
            for (int i = this.FirstLineIndex; i < this.Lines.Count; i++)
            {
                var lineBounds = this.GetLineBounds(i - this.FirstLineIndex);
                if (lineBounds.Contains(new PointF(cursor.Bounds.X, cursor.Bounds.Y)))
                {
                    this.SelectedIndex = i;
                    this.OnSelection(this);
                    return;
                }
            }
        }

        /// <inheritdoc/>
        public virtual void UpdateItem(int index, T item)
        {
            if (index >= 0 && index < this.Lines.Count)
            {
                this.Lines[index] = item.ToString();
            }
        }

        protected RectangleF GetLineBounds(int index)
        {
            float leading = Constants.Leading.TryGetValue(this.FontFamily, out float result) ? result : 0.25f;
            PointF linePosition = this.GetLinePosition(index);
            float y = linePosition.Y - (this.TextSize.Height * this.ScaledLineGap / 2) + (this.TextSize.Height * leading);
            float height = this.TextSize.Height + (this.TextSize.Height * this.ScaledLineGap);
            float scrollbarOffset = this.Lines.Count > this.MaxLines ? this.ScrollbarWidth : 0;
            return new RectangleF(this.Bounds.X, y, this.Bounds.Width - scrollbarOffset, height);
        }

        protected PointF GetLinePosition(int index)
        {
            return new PointF(this.TextPosition.X, this.TextPosition.Y + (index * (this.TextSize.Height + (this.TextSize.Height * this.ScaledLineGap))));
        }
    }
}
