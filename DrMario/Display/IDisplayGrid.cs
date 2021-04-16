using System.Windows.Forms;

namespace DrMario.Display
{
    /// <summary>
    /// Игровая сетка
    /// </summary>
    public interface IDisplayGrid
    {
        Control this[int row, int col] { get; set; }
    }
}
