namespace RawCanvasUI.Interfaces
{
    /// <summary>
    /// Represents an observable element.
    /// </summary>
    public interface IObservable
    {
        /// <summary>
        /// Gets the unique identifer for the observable object.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Adds an observer to the observable.
        /// </summary>
        /// <param name="observer">The observer to add.</param>
        void AddObserver(IObserver observer);

        /// <summary>
        /// Called when the observers should be notified of something.
        /// </summary>
        void NotifyObservers();

        /// <summary>
        /// Removes an observer from the observable.
        /// </summary>
        /// <param name="observer">The observer to remove.</param>
        void RemoveObserver(IObserver observer);
    }
}
