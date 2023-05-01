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
        private readonly Dictionary<string, IWidget> widgets = new Dictionary<string, IWidget>();
        private readonly string activeButtonTextureName;
        private readonly string inactiveButtonTextureName;
        private readonly List<ToggledButton> tabButtons = new List<ToggledButton>();
        private string activeTabTitle = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabbedWidget"/> class.
        /// </summary>
        /// <param name="widgetTextureName">The name of the texture for the widget background.</param>
        /// <param name="activeButtonTextureName">The name of the texture for the active button.</param>
        /// <param name="inactiveButtonTextureName">The name of the texture for the inactive button.</param>
        public TabbedWidget(string widgetTextureName, string activeButtonTextureName, string inactiveButtonTextureName)
            : base(widgetTextureName)
        {
            this.activeButtonTextureName = activeButtonTextureName;
            this.inactiveButtonTextureName = inactiveButtonTextureName;
            this.Width = 0;
            this.Height = 0;
        }

        /// <summary>
        /// Properly adds a widget to the tabbed widget.
        /// </summary>
        /// <param name="widget">The widget to add.</param>
        /// <param name="tabTitle">The title to use for the tab.</param>
        public void AddWidget(IWidget widget, string tabTitle)
        {
            this.Add(widget);
            if (this.widgets.Count == 0)
            {
                this.activeTabTitle = tabTitle;
            }

            this.widgets[tabTitle] = widget;
            var button = new ToggledButton(tabTitle, this.activeButtonTextureName, this.inactiveButtonTextureName, tabTitle, true);
            this.Add(button);
            button.AddObserver(this);
            if (this.widgets.Count == 1)
            {
                button.IsActive = true;
            }

            this.tabButtons.Add(button);
        }

        /// <inheritdoc/>
        public override void Draw(Rage.Graphics g)
        {
            if (this.Texture != null)
            {
                g.DrawTexture(this.Texture, this.Bounds);
                this.tabButtons.ForEach(button => button.Draw(g));
                if (this.widgets.TryGetValue(this.activeTabTitle, out IWidget widget))
                {
                    widget.Draw(g);
                }
            }
        }

        /// <inheritdoc />
        public override void OnUpdated(IObservable obj)
        {
            this.tabButtons.Where(x => x.Id != obj.Id).ToList().ForEach(x => x.IsActive = false);
            this.activeTabTitle = obj.Id;
        }

        /// <inheritdoc/>
        public override void UpdateBounds()
        {
            if (this.widgets.Count == 0 || this.Parent == null || this.Texture == null)
            {
                return;
            }

            base.UpdateBounds();

            // update the buttons
            for (int i = 0; i < this.tabButtons.Count; i++)
            {
                var button = this.tabButtons[i];
                if (i == 0)
                {
                    button.MoveTo(new Point(0, 0));
                }
                else
                {
                    button.MoveTo(new Point(this.tabButtons[i - 1].Position.X + this.tabButtons[i - 1].Width, 0));
                }
            }

            // now reposition and reupdate the widgets
            foreach (var widget in this.Items.OfType<IWidget>())
            {
                if (this.tabButtons.Count > 0)
                {
                    widget.MoveTo(new Point(0, this.tabButtons[0].Height));
                }
            }
        }
    }
}