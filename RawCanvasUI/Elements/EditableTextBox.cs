using Rage;
using Rage.Native;
using RawCanvasUI.Interfaces;
using RawCanvasUI.Mouse;
using System.Collections.Generic;
using System.Drawing;

namespace RawCanvasUI.Elements
{
    public class EditableTextBox : TextBox, IEditable
    {
        private readonly List<IObserver> observers = new List<IObserver>();

        public EditableTextBox(string id, int x, int y, int width, int height) : base(x, y, width, height)
        {
            this.Id = id;
            this.Position = new Point(x, y);
            this.Height = height;
            this.Width = width;
        }

        public EditableTextBox(string id, string text, int x, int y, int width, int height) : base(text, x, y, width, height)
        {
            this.Id = id;
            this.Text = text;
            this.Position = new Point(x, y);
            this.Height = height;
            this.Width = width;
        }

        public bool IsEnabled { get; set; } = true;

        public string Id { get; }

        public void AddObserver(IObserver observer)
        {
            this.observers.Add(observer);
        }

        public bool Contains(Cursor cursor)
        {
            return this.Bounds.Contains(cursor.Bounds.Location);
        }

        public void Edit(Cursor cursor)
        {
            GameFiber.StartNew(delegate
            {
                NativeFunction.Natives.DISABLE_ALL_CONTROL_ACTIONS(2);
                NativeFunction.Natives.DISPLAY_ONSCREEN_KEYBOARD(true, "FMMC_KEY_TIP8", 0, this.Text, 0, 0, 0, 300);
                GameFiber.WaitWhile(() => NativeFunction.Natives.UPDATE_ONSCREEN_KEYBOARD<int>() == 0);
                NativeFunction.Natives.ENABLE_ALL_CONTROL_ACTIONS(2);
                string result = NativeFunction.Natives.GET_ONSCREEN_KEYBOARD_RESULT<string>();
                if (result != null)
                {
                    this.Text = result;
                    this.NotifyObservers();
                }
            }, "rcui-editable-text-box");
        }

        public void NotifyObservers()
        {
            this.observers.ForEach(x => x.OnUpdated(this));
        }

        public void RemoveObserver(IObserver observer)
        {
            this.observers.Remove(observer);
        }
    }
}

