using RawCanvasUI.Interfaces;
using System;

namespace RawCanvasUI.Events
{
    public class ModalEventArgs : EventArgs
    {
        public IModal Modal { get; }

        public ModalEventArgs(IModal modal)
        {
            Modal = modal;
        }
    }
}

