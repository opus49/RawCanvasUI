using System.Collections.Generic;
using System.Windows.Forms;

namespace RawCanvasUI.Keyboard
{
    internal static class KeyValidator
    {
        private static readonly HashSet<Keys> validKeys;

        static KeyValidator()
        {
            Logging.Debug("building Keyvalidator valid keys");
            validKeys = new HashSet<Keys>();

            for (char c = 'A'; c <= 'Z'; c++)
                validKeys.Add((Keys)c);

            for (char c = '0'; c <= '9'; c++)
                validKeys.Add((Keys)c);

            validKeys.Add(Keys.Space);
            validKeys.Add(Keys.Back);
            validKeys.Add(Keys.Enter);
            validKeys.Add(Keys.Escape);
            validKeys.Add(Keys.Tab);

            validKeys.Add(Keys.Oem1);      // ';:' for US
            validKeys.Add(Keys.Oem2);      // '/?' for US
            validKeys.Add(Keys.Oem3);      // '`~' for US
            validKeys.Add(Keys.Oem4);      // '[{' for US
            validKeys.Add(Keys.Oem5);      // '\|' for US
            validKeys.Add(Keys.Oem6);      // ']}' for US
            validKeys.Add(Keys.Oem7);      // ''"' for US
            validKeys.Add(Keys.Oem8);      // miscellaneous characters
            validKeys.Add(Keys.Oem102);    // '<>' or '\|' on RT 102-key keyboard
            validKeys.Add(Keys.Oemplus);   // '+'
            validKeys.Add(Keys.OemMinus);  // '-'
            validKeys.Add(Keys.Oemcomma);  // ','
            validKeys.Add(Keys.OemPeriod); // '.'

            Logging.Debug($"added {validKeys.Count} valid keys");
        }

        public static bool IsValidKey(Keys key)
        {
            return validKeys.Contains(key);
        }
    }
}
