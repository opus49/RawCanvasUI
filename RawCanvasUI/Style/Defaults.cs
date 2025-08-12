using System.Drawing;
using System.Drawing.Drawing2D;

namespace RawCanvasUI.Style
{
    internal static class Defaults
    {
        public static Color BackgroundColor = Color.White;
        public static Color BorderColor = Color.Black;
        public static float BorderWidth = 1f;
        public static Color CaretColor = Color.Black;
        public static Color DisabledFontColor = Color.FromArgb(128, 128, 128);
        public static Color FontColor = Color.Black;
        public static string FontFamily = "Lucida Console";
        public static float FontSize = 14;
        public static Color HighlightBackgroundColor = Color.FromArgb(237, 237, 237);
        public static Color HighlightFontColor = Color.Black;
        public static float LeftPadding = 5f;
        public static float LineGap = 1f;
        public static Color ScrollbarInnerColor = Color.FromArgb(177, 177, 177);
        public static Color ScrollbarOuterColor = Color.FromArgb(226, 226, 226);
        public static float ScrollbarWidth = 8f;
    }
}
