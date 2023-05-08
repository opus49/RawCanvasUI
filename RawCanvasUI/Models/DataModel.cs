using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace RawCanvasUI.Models
{
    public abstract class DataModel<T> : INotifyCollectionChanged
        where T : INotifyPropertyChanged
    {
        private readonly List<T> items = new List<T>();

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public List<T> Items
        {
            get { return this.items; }
        }

        public virtual void Add(T item)
        {
            this.items.Add(item);
            this.RaiseCollectionChangedEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            item.PropertyChanged += this.OnItemPropertyChanged;
        }

        public virtual void Clear()
        {
            this.items.Clear();
            this.RaiseCollectionChangedEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected virtual void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var item = (T)sender;
            var index = this.items.IndexOf(item);
            this.RaiseCollectionChangedEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, index));
        }

        protected virtual void RaiseCollectionChangedEvent(NotifyCollectionChangedEventArgs e)
        {
            this.CollectionChanged?.Invoke(this, e);
        }
    }
}
