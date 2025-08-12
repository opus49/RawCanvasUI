using System;

namespace RawCanvasUI.Events
{
    public static class CommandKeyEventHandler
    {
        public static event EventHandler<CommandKeyEventArgs> OnCommandKey;

        public static void RaiseCommandKey(string key)
        {
            OnCommandKey?.Invoke(null, new CommandKeyEventArgs(key));
        }
    }
}
