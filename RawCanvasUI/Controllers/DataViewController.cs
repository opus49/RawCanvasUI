using RawCanvasUI.Elements;
using RawCanvasUI.Interfaces;
using RawCanvasUI.Models;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace RawCanvasUI.Controllers
{
    public abstract class DataViewController<T>
        where T : class, INotifyPropertyChanged
    {
        private readonly DataModel<T> model;
        private readonly List<IDataView<T>> views = new List<IDataView<T>>();

        public DataViewController(DataModel<T> model)
        {
            this.model = model;
            this.model.CollectionChanged += this.OnModelChanged;
            this.model.PropertyChanged += this.OnPropertyChanged;
        }

        public DataModel<T> Model { get => this.model; }

        public void AddView(IDataView<T> view)
        {
            this.views.Add(view);
            if (view is DataListView<T> dataListView)
            {
                dataListView.OnSelection += this.OnDataListViewSelection;
            }
        }

        protected void OnDataListViewSelection(DataListView<T> view) 
        {
            if (view.SelectedIndex >= 0 && view.SelectedIndex < this.model.Items.Count)
            {
                view.SelectedItem = this.model.Items[view.SelectedIndex];
            }
        }

        protected virtual void OnModelChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (T item in e.NewItems)
                    {
                        int index = this.model.Items.IndexOf(item);
                        this.views.ForEach(x => x.NewItem(index, item));
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    this.views.ForEach(x => x.Reset());
                    break;
            }
        }

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var item = (T)sender;
            var index = this.Model.Items.IndexOf(item);
            this.views.ForEach(x => x.UpdateItem(index, item));
        }
    }
}
