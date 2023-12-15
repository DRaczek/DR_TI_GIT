using System;
using System.Collections.Generic;
using System.Text;

namespace DR_TI_GIT
{
    internal class GameSave
    {
        public int EmptyRow { get; set; }
        public int EmptyCol { get; set; }
        public int Size { get; set; }
        public List<Tile> tiles{ get; set; }
    }
}
