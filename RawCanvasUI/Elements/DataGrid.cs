using RawCanvasUI.Interfaces;
using RawCanvasUI.Mouse;
using System.Collections.Generic;
using System.Drawing;

namespace RawCanvasUI.Elements
{
    public class DataGrid<T> : TextArea, ISelectable<T>
        where T : IDataItem
    {
        private readonly List<IObserver> observers = new List<IObserver>();

        public DataGrid(string id, int x, int y, int width, int height) 
            : base(x, y, width, height)
        {
            this.Id = id;
        }

        /// <inheritdoc/>
        public string Id { get; protected set; } = string.Empty;

        /// <summary>
        /// <summary>
        /// Gets or sets the background color of the selected item.
        /// </summary>
        public Color HighlightBackgroundColor { get; set; } = Color.Black;

        /// <summary>
        /// Gets or sets the font color of the selected item.
        /// </summary>
        public Color HighlightFontColor { get; set; } = Color.White;

        /// <inheritdoc/>
        public List<T> Items { get; protected set; } = new List<T>();

        /// <inheritdoc/>
        public int SelectedIndex { get; protected set; } = -1;

        /// <inheritdoc/>
        public T SelectedItem { get; protected set; } = default;

        public override void Add(string text)
        {
            throw new System.Exception("you cannot add text to a data grid!");
        }

        /// <inheritdoc/>
        public virtual void Add(T item)
        {
            this.Items.Add(item);
            this.Lines.Add(item.ToString());
        }

        /// <inheritdoc/>
        public void AddObserver(IObserver observer)
        {
            this.observers.Add(observer);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            this.Items.Clear();
            this.Lines.Clear();
        }

        /// <inheritdoc/>
        public virtual void Click(Cursor cursor)
        {
            for (int i = this.FirstLineIndex; i < this.Lines.Count; i++)
            {
                var lineBounds = this.GetLineBounds(i - this.FirstLineIndex);
                if (lineBounds.Contains(new PointF(cursor.Bounds.X, cursor.Bounds.Y)))
                {
                    this.SelectedIndex = i;
                    this.SelectedItem = this.Items[i];
                    this.observers.ForEach(x => x.OnUpdated(this));
                    return;
                }
            }
        }

        /// <inheritdoc/>
        public override void Draw(Rage.Graphics g)
        {
            g.DrawRectangle(this.BorderBounds, this.BorderColor);
            g.DrawRectangle(this.Bounds, this.BackgroundColor);
            for (int i = this.FirstLineIndex; i < this.Lines.Count; i++)
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

                g.DrawText(this.Lines[i].ToString(), this.FontFamily, this.ScaledFontSize, linePosition, i == this.SelectedIndex ? this.HighlightFontColor : this.FontColor);
            }

            if (this.ScrollbarWidth > 0)
            {
                this.DrawScrollbar(g);
            }
        }

        public void Remove(T item)
        {
            this.Items.Remove(item);
        }

        /// <inheritdoc/>
        public void RemoveObserver(IObserver observer)
        {
            this.observers.Remove(observer);
        }

        protected RectangleF GetLineBounds(int index)
        {
            PointF linePosition = this.GetLinePosition(index);
            float y = linePosition.Y - (this.TextSize.Height * this.ScaledLineGap / 2);
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
