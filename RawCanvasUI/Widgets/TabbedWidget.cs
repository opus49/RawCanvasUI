using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using RawCanvasUI.Elements;
using RawCanvasUI.Interfaces;

namespace RawCanvasUI.Widgets
{
    /// <summary>
    /// Represents a widget that contains other widgets which allows the user to switch between them.
    /// </summary>
    public class TabbedWidget : TextureWidget
    {
        private readonly string activeButtonTextureName;
        private readonly string inactiveButtonTextureName;
        private readonly int tabWidth;
        private readonly int tabHeight;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabbedWidget"/> class.
        /// </summary>
        /// <param name="widgetTextureName">The name of the texture for the widget background.</param>
        /// <param name="activeButtonTextureName">The name of the texture for the active button.</param>
        /// <param name="inactiveButtonTextureName">The name of the texture for the inactive button.</param>
        public TabbedWidget(string widgetTextureName, int width, int height, int tabWidth, int tabHeight, string activeButtonTextureName, string inactiveButtonTextureName)
            : base(widgetTextureName, width, height)
        {
            this.activeButtonTextureName = activeButtonTextureName;
            this.inactiveButtonTextureName = inactiveButtonTextureName;
            this.Width = width;
            this.Height = height;
            this.tabWidth = tabWidth;
            this.tabHeight = tabHeight;
        }

        /// <summary>
        /// Gets or sets the list of toggled buttons used for the tabs.
        /// </summary>
        public List<ToggledButton> TabButtons { get; protected set; } = new List<ToggledButton>();

        /// <summary>
        /// Gets or sets the tab offset from upper-right hand corner of tabbed widget.
        /// </summary>
        public Point TabOffset { get; set; } = new Point(0, 0);

        /// <summary>
        /// Gets or sets the list of widgets contained by the tabbed widget.
        /// </summary>
        public Dictionary<string, IWidget> Widgets { get; protected set; } = new Dictionary<string, IWidget>();

        /// <summary>
        /// Properly adds a widget to the tabbed widget.
        /// </summary>
        /// <param name="widget">The widget to add.</param>
        /// <param name="tabTitle">The title to use for the tab.</param>
        public void AddWidget(IWidget widget, string tabTitle)
        {
            // TODO: this could be cleaned up
            this.Add(widget);
            if (this.Widgets.Count == 0)
            {
                widget.IsVisible = true;
            }
            else
            {
                widget.IsVisible = false;
            }

            this.Widgets[tabTitle] = widget;
            var button = new ToggledButton(tabTitle, this.tabWidth, this.tabHeight, this.activeButtonTextureName, this.inactiveButtonTextureName, tabTitle, true);
            button.StyleName = this.StyleName;
            this.Add(button);
            button.AddObserver(this);
            if (this.Widgets.Count == 1)
            {
                button.IsActive = true;
            }

            this.TabButtons.Add(button);
        }

        /// <inheritdoc/>
        public override void Draw(Rage.Graphics g)
        {
            if (this.Texture != null)
            {
                g.DrawTexture(this.Texture, this.Bounds);
            }
            else
            {
                g.DrawRectangle(this.Bounds, Color.LimeGreen);
            }

            this.TabButtons.ForEach(button => button.Draw(g));
            this.Widgets.Values.Where(x => x.IsVisible).ToList().ForEach(x => x.Draw(g));
        }

        /// <inheritdoc />
        public override void OnUpdated(IObservable obj)
        {
            if (this.TabButtons.Contains(obj))
            {
                this.TabButtons.Where(x => x.Id != obj.Id).ToList().ForEach(x => x.IsActive = false);
                foreach (KeyValuePair<string, IWidget> entry in this.Widgets)
                {
                    if (entry.Key == obj.Id)
                    {
                        entry.Value.IsVisible = true;
                    }
                    else
                    {
                        entry.Value.IsVisible = false;
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override void UpdateBounds()
        {
            if (this.Widgets.Count == 0 || this.Parent == null || this.Texture == null)
            {
                return;
            }

            base.UpdateBounds();
            this.UpdateTabButtonBounds();
            this.UpdateWidgetBounds();
        }

        protected virtual void UpdateTabButtonBounds()
        {
            for (int i = 0; i < this.TabButtons.Count; i++)
            {
                var button = this.TabButtons[i];
                if (i == 0)
                {
                    button.MoveTo(new Point(this.TabOffset.X, this.TabOffset.Y));
                }
                else
                {
                    button.MoveTo(new Point(this.TabButtons[i - 1].Position.X + this.TabButtons[i - 1].Width, this.TabOffset.Y));
                }
            }
        }

        protected virtual void UpdateWidgetBounds()
        {
            foreach (var widget in this.Items.OfType<IWidget>())
            {
                if (this.TabButtons.Count > 0)
                {
                    widget.MoveTo(new Point(0, this.TabButtons[0].Height));
                }
            }
        }
    }
}