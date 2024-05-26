using Rage;
using RawCanvasUI.Interfaces;
using System;

namespace RawCanvasUI.Events
{
    public static class ModalEventHandler
    {
        public static event EventHandler<ModalEventArgs> OnDisposeModal;
        public static event EventHandler<ModalEventArgs> OnShowModal;

        public static void RaiseDisposeModal(IModal modal)
        {
            OnDisposeModal?.Invoke(null, new ModalEventArgs(modal));
        }

        public static void RaiseShowModal(IModal modal)
        {
            Logging.Debug("ModalEventHandler - RaiseShowModal called");
            OnShowModal?.Invoke(null, new ModalEventArgs(modal));
        }
    }
}
