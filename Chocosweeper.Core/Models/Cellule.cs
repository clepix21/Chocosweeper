using System;

namespace Chocosweeper.Core.Modeles
{
    /// <summary>
    /// Repr�sente une cellule unique sur le plateau de jeu
    /// </summary>
    public class Cellule
    {
        /// <summary>
        /// Position de la ligne sur le plateau (base z�ro)
        /// </summary>
        public int Ligne { get; }

        /// <summary>
        /// Position de la colonne sur le plateau (base z�ro)
        /// </summary>
        public int Colonne { get; }

        /// <summary>
        /// Indique si cette cellule contient une mine
        /// </summary>
        public bool ContientMine { get; set; }

        /// <summary>
        /// Indique si cette cellule a �t� r�v�l�e par le joueur
        /// </summary>
        public bool EstRevelee { get; set; }

        /// <summary>
        /// Indique si cette cellule a �t� marqu�e d'un drapeau par le joueur
        /// </summary>
        public bool EstMarquee { get; set; }

        /// <summary>
        /// Nombre de mines adjacentes (0-8)
        /// </summary>
        public int MinesAdjacentes { get; set; }

        /// <summary>
        /// Cr�e une nouvelle cellule � la position sp�cifi�e
        /// </summary>
        /// <param name="ligne">Position de la ligne (base z�ro)</param>
        /// <param name="colonne">Position de la colonne (base z�ro)</param>
        public Cellule(int ligne, int colonne)
        {
            Ligne = ligne;
            Colonne = colonne;
            ContientMine = false;
            EstRevelee = false;
            EstMarquee = false;
            MinesAdjacentes = 0;
        }
    }
}

