namespace RawCanvasUI.Interfaces
{
    /// <summary>
    /// Represents a control that responds to a mouse click.
    /// </summary>
    public interface IClickable : IControl, IObservable
    {
        /// <summary>
        /// Executed when the user clicks the control.
        /// </summary>
        void Click();
    }
}
