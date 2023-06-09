﻿using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace RawCanvasUI.Models
{
    public abstract class DataModel<T> : INotifyCollectionChanged, INotifyPropertyChanged
        where T : INotifyPropertyChanged
    {
        private readonly List<T> items = new List<T>();

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

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
            this.items.ForEach(x => x.PropertyChanged -= this.OnItemPropertyChanged);
            this.items.Clear();
            this.RaiseCollectionChangedEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public virtual bool Remove(T item)
        {
            int oldIndex = this.items.IndexOf(item);
            if (oldIndex == -1)
            {
                return false;

            }

            bool removed = this.items.Remove(item);
            if (removed)
            {
                this.RaiseCollectionChangedEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, oldIndex));
                item.PropertyChanged -= this.OnItemPropertyChanged;
            }

            return removed;
        }

        protected virtual void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.PropertyChanged?.Invoke(sender, e);
        }

        protected virtual void RaiseCollectionChangedEvent(NotifyCollectionChangedEventArgs e)
        {
            this.CollectionChanged?.Invoke(this, e);
        }
    }
}
