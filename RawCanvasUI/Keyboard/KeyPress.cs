using System;
using System.Windows.Forms;

namespace RawCanvasUI.Keyboard
{
    internal class KeyPress
    {
        public Keys Key { get; private set; }
        public long PressedTime { get; private set; }
        public int RepeatCount { get; private set; }
        public bool IsShiftDown { get; private set; }

        public KeyPress(Keys key, long pressedTime, bool isShiftDown)
        {
            Key = key;
            PressedTime = pressedTime;
            RepeatCount = 1;
            IsShiftDown = isShiftDown;
        }

        public void IncrementRepeatCount()
        {
            this.RepeatCount++;
        }

        public bool ShouldProcessKey(long currentTime)
        {
            return this.RepeatCount > 1 || (currentTime - this.PressedTime) > Constants.LongKeypressDuration;
        }

        public override String ToString()
        {
            return this.GetKeyString();
        }

        private string GetKeyString()
        {
            // Handle alphanumeric and special characters with Shift state
            switch (this.Key)
            {
                case Keys.A:
                case Keys.B:
                case Keys.C:
                case Keys.D:
                case Keys.E:
                case Keys.F:
                case Keys.G:
                case Keys.H:
                case Keys.I:
                case Keys.J:
                case Keys.K:
                case Keys.L:
                case Keys.M:
                case Keys.N:
                case Keys.O:
                case Keys.P:
                case Keys.Q:
                case Keys.R:
                case Keys.S:
                case Keys.T:
                case Keys.U:
                case Keys.V:
                case Keys.W:
                case Keys.X:
                case Keys.Y:
                case Keys.Z:
                    return this.IsShiftDown ? this.Key.ToString() : this.Key.ToString().ToLower();

                case Keys.D0: return this.IsShiftDown ? ")" : "0";
                case Keys.D1: return this.IsShiftDown ? "!" : "1";
                case Keys.D2: return this.IsShiftDown ? "@" : "2";
                case Keys.D3: return this.IsShiftDown ? "#" : "3";
                case Keys.D4: return this.IsShiftDown ? "$" : "4";
                case Keys.D5: return this.IsShiftDown ? "%" : "5";
                case Keys.D6: return this.IsShiftDown ? "^" : "6";
                case Keys.D7: return this.IsShiftDown ? "&" : "7";
                case Keys.D8: return this.IsShiftDown ? "*" : "8";
                case Keys.D9: return this.IsShiftDown ? "(" : "9";

                case Keys.Space: return " ";
                case Keys.Back: return "[Back]";
                case Keys.Enter: return "[Enter]";
                case Keys.Escape: return "[Esc]";
                case Keys.Tab: return "[Tab]";

                case Keys.Oem1: return this.IsShiftDown ? ":" : ";";
                case Keys.Oem2: return this.IsShiftDown ? "?" : "/";
                case Keys.Oem3: return this.IsShiftDown ? "~" : "`";
                case Keys.Oem4: return this.IsShiftDown ? "{" : "[";
                case Keys.Oem5: return this.IsShiftDown ? "|" : "\\";
                case Keys.Oem6: return this.IsShiftDown ? "}" : "]";
                case Keys.Oem7: return this.IsShiftDown ? "\"" : "'";
                case Keys.OemMinus: return this.IsShiftDown ? "_" : "-";
                case Keys.Oemplus: return this.IsShiftDown ? "+" : "=";
                case Keys.Oemcomma: return this.IsShiftDown ? "<" : ",";
                case Keys.OemPeriod: return this.IsShiftDown ? ">" : ".";

                default: return "?";
            }
        }
    }
}
