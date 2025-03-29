using System;
using System.Collections.Generic;

namespace Chocosweeper.System
{
    public class Tableau
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int NbMine { get; private set; }
        public Case[,] Cases { get; private set; }
        public bool GameOver { get; private set; }
        public bool Victoire { get; private set; }

        private Random random = new Random();

        public Tableau(int width, int height, int nbmine)
        {
            Width = width;
            Height = height;
            NbMine = nbmine;
            Cases = new Case[width, height];

            // Initialiser toutes les cases
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Cases[x, y] = new Case(x, y);
                }
            }

            // Placer les mines
            PlaceMines();

            // Calculer les mines adjacentes
            CalculerMines_Adjacentes();
        }

        private void PlaceMines()
        {
            int minesPlaced = 0;

            // Placer les mines al�atoirement
            while (minesPlaced < NbMine)
            {
                int x = random.Next(Width);
                int y = random.Next(Height);

                if (!Cases[x, y].Mine)
                {
                    Cases[x, y].Mine = true;
                    minesPlaced++;
                }
            }
        }

        private void CalculerMines_Adjacentes()
        {
            // Calculer le nombre de mines adjacentes pour chaque case
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (!Cases[x, y].Mine)
                    {
                        int count = 0;

                        // V�rifier les 8 cases adjacentes
                        for (int dx = -1; dx <= 1; dx++)
                        {
                            for (int dy = -1; dy <= 1; dy++)
                            {
                                int nx = x + dx;
                                int ny = y + dy;

                                if (nx >= 0 && nx < Width && ny >= 0 && ny < Height && Cases[nx, ny].Mine)
                                {
                                    count++;
                                }
                            }
                        }

                        Cases[x, y].Mines_Adjacentes = count;
                    }
                }
            }
        }

        public void R�v�lerCase(int x, int y)
        {
            if (GameOver || Victoire)
                return;

            Case Case = Cases[x, y];

            if (Case.R�v�l� || Case.Drapeau)
                return;

            Case.R�v�l� = true;

            if (Case.Mine)
            {
                GameOver = true;
                R�v�lerToutesLesMines();
                return;
            }

            // Si la case n'a pas de mines adjacentes, r�v�ler les cases adjacentes
            if (Case.Mines_Adjacentes == 0)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        int nx = x + dx;
                        int ny = y + dy;

                        if (nx >= 0 && nx < Width && ny >= 0 && ny < Height && !Cases[nx, ny].R�v�l�)
                        {
                            R�v�lerCase(nx, ny);
                        }
                    }
                }
            }

            CheckVictoire();
        }

        public void ToggleDrapeau(int x, int y)
        {
            if (GameOver || Victoire)
                return;

            Case Case = Cases[x, y];

            if (!Case.R�v�l�)
            {
                Case.Drapeau = !Case.Drapeau;
                CheckVictoire();
            }
        }

        private void R�v�lerToutesLesMines()
        {
            // R�v�ler toutes les mines
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (Cases[x, y].Mine)
                    {
                        Cases[x, y].R�v�l� = true;
                    }
                }
            }
        }

        private void CheckVictoire()
        {
            // V�rifier si toutes les cases sans mine sont r�v�l�es
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Case Case = Cases[x, y];
                    if (!Case.Mine && !Case.R�v�l�)
                    {
                        return; // Toutes les cases sans mine ne sont pas r�v�l�es
                    }
                }
            }

            Victoire = true;

            // Placer un drapeau sur toutes les mines
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (Cases[x, y].Mine)
                    {
                        Cases[x, y].Drapeau = true;
                    }
                }
            }
        }
    }
}

