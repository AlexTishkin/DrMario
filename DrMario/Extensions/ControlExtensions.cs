using DrMario.Implementations;
using System.Windows.Forms;

namespace DrMario.Extensions
{
    public static class ControlExtensions
    {
        public static void SetColor(this Control control, GameCellColor color)
        {
            control.BackColor = GameToDrawingColorConvert.ToDrawingColor(color);
        }
    }
}
