using System;

namespace RawCanvasUI.Events
{
    public class CommandKeyEventArgs : EventArgs
    {
        public string Key { get; }

        public CommandKeyEventArgs(string key)
        {
            Key = key;
        }
    }
}
