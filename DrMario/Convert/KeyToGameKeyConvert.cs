using System.Windows.Forms;

namespace DrMario.Implementations
{
    public static class KeyToGameKeyConvert
    {
        public static GameKey ToGameKey(Keys keyboardKey)
        {
            switch (keyboardKey)
            {
                case Keys.A: return GameKey.LEFT;
                case Keys.D: return GameKey.RIGHT;
                case Keys.S: return GameKey.DOWN;
                case Keys.Space: return GameKey.SPACE;
            }

            return GameKey.NONE;
        }
    }
}
