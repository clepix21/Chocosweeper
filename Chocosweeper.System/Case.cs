namespace Chocosweeper.System
{
    public class Case
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool Mine { get; set; }
        public bool R�v�l� { get; set; }
        public bool Drapeau { get; set; }
        public int Mines_Adjacentes { get; set; }
        
        public Case(int x, int y)
        {
            X = x;
            Y = y;
            Mine = false;
            R�v�l� = false;
            Drapeau = false;
            Mines_Adjacentes = 0;
        }
    }
}

