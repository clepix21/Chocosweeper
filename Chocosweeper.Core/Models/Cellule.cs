using System;

namespace Chocosweeper.Core.Modeles
{
    /// <summary>
    /// Représente une cellule unique sur le plateau de jeu
    /// </summary>
    public class Cellule
    {
        /// <summary>
        /// Position de la ligne sur le plateau (base zéro)
        /// </summary>
        public int Ligne { get; }

        /// <summary>
        /// Position de la colonne sur le plateau (base zéro)
        /// </summary>
        public int Colonne { get; }

        /// <summary>
        /// Indique si cette cellule contient une mine
        /// </summary>
        public bool ContientMine { get; set; }

        /// <summary>
        /// Indique si cette cellule a été révélée par le joueur
        /// </summary>
        public bool EstRevelee { get; set; }

        /// <summary>
        /// Indique si cette cellule a été marquée d'un drapeau par le joueur
        /// </summary>
        public bool EstMarquee { get; set; }

        /// <summary>
        /// Nombre de mines adjacentes (0-8)
        /// </summary>
        public int MinesAdjacentes { get; set; }

        /// <summary>
        /// Crée une nouvelle cellule à la position spécifiée
        /// </summary>
        /// <param name="ligne">Position de la ligne (base zéro)</param>
        /// <param name="colonne">Position de la colonne (base zéro)</param>
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

