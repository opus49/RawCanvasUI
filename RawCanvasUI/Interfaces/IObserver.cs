namespace RawCanvasUI.Interfaces
{
    /// <summary>
    /// Represents an observer.
    /// </summary>
    public interface IObserver
    {
        /// <summary>
        /// Called when the observable is updated.
        /// </summary>
        /// <param name="obj">The observed object.</param>
        void OnUpdated(IObservable obj);
    }
}
