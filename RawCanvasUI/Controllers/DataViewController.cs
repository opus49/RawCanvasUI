using RawCanvasUI.Elements;
using RawCanvasUI.Interfaces;
using RawCanvasUI.Models;
using System.Collections.Specialized;
using System.ComponentModel;

namespace RawCanvasUI.Controllers
{
    public abstract class DataViewController<T> : IObserver
        where T : INotifyPropertyChanged
    {
        private readonly DataModel<T> model;
        private readonly DataView<T> view;

        public DataViewController(DataModel<T> model, DataView<T> view)
        {
            this.model = model;
            this.view = view;
            this.model.CollectionChanged += this.OnModelChanged;
            this.view.AddObserver(this);
        }

        public virtual void OnUpdated(IObservable observable)
        {
            if (observable == this.view)
            {
                int selectedIndex = this.view.SelectedIndex;
                if (selectedIndex >= 0 && selectedIndex < this.model.Items.Count)
                {
                    this.view.SelectedItem = this.model.Items[selectedIndex];
                }
            }
        }

        protected virtual void OnModelChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (T item in e.NewItems)
                    {
                        this.view.Add(item.ToString());
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    this.view.Lines[e.NewStartingIndex] = e.NewItems[0].ToString();
                    break;

                case NotifyCollectionChangedAction.Reset:
                    this.view.ClearText();
                    foreach (T item in this.model.Items)
                    {
                        this.view.Add(item.ToString());
                    }
                    break;
            }
        }
    }
}
