using System;
using System.Collections.Generic;

namespace Chocosweeper.Core.Modeles
{
    /// <summary>
    /// Représente le plateau de jeu contenant toutes les cellules
    /// </summary>
    public class PlateauDeJeu
    {
        /// <summary>
        /// Nombre de lignes sur le plateau
        /// </summary>
        public int Lignes { get; }

        /// <summary>
        /// Nombre de colonnes sur le plateau
        /// </summary>
        public int Colonnes { get; }

        /// <summary>
        /// Nombre total de mines sur le plateau
        /// </summary>
        public int NombreMines { get; }

        /// <summary>
        /// Tableau 2D de cellules représentant le plateau
        /// </summary>
        public Cellule[,] Cellules { get; }

        /// <summary>
        /// Générateur de nombres aléatoires pour le placement des mines
        /// </summary>
        private readonly Random _aleatoire = new Random();

        /// <summary>
        /// Crée un nouveau plateau de jeu avec les dimensions et le nombre de mines spécifiés
        /// </summary>
        /// <param name="lignes">Nombre de lignes</param>
        /// <param name="colonnes">Nombre de colonnes</param>
        /// <param name="nombreMines">Nombre de mines</param>
        public PlateauDeJeu(int lignes, int colonnes, int nombreMines)
        {
            Lignes = lignes;
            Colonnes = colonnes;
            NombreMines = Math.Min(nombreMines, lignes * colonnes - 1);
            Cellules = new Cellule[lignes, colonnes];

            // Initialiser les cellules
            for (int ligne = 0; ligne < lignes; ligne++)
            {
                for (int col = 0; col < colonnes; col++)
                {
                    Cellules[ligne, col] = new Cellule(ligne, col);
                }
            }
        }

        /// <summary>
        /// Place les mines aléatoirement sur le plateau, en évitant la cellule sécurisée spécifiée
        /// </summary>
        /// <param name="ligneSure">Ligne de la cellule sécurisée (premier clic)</param>
        /// <param name="colonneSure">Colonne de la cellule sécurisée (premier clic)</param>
        public void PlacerMines(int ligneSure, int colonneSure)
        {
            int minesPlacees = 0;

            while (minesPlacees < NombreMines)
            {
                int ligne = _aleatoire.Next(Lignes);
                int col = _aleatoire.Next(Colonnes);

                // Ignorer la cellule sécurisée et les cellules qui ont déjà des mines
                if ((ligne == ligneSure && col == colonneSure) || Cellules[ligne, col].ContientMine)
                {
                    continue;
                }

                Cellules[ligne, col].ContientMine = true;
                minesPlacees++;
            }

            // Calculer les mines adjacentes pour chaque cellule
            CalculerMinesAdjacentes();
        }

        /// <summary>
        /// Calcule le nombre de mines adjacentes pour chaque cellule
        /// </summary>
        private void CalculerMinesAdjacentes()
        {
            for (int ligne = 0; ligne < Lignes; ligne++)
            {
                for (int col = 0; col < Colonnes; col++)
                {
                    if (!Cellules[ligne, col].ContientMine)
                    {
                        Cellules[ligne, col].MinesAdjacentes = CompterMinesAdjacentes(ligne, col);
                    }
                }
            }
        }

        /// <summary>
        /// Compte le nombre de mines adjacentes à la cellule spécifiée
        /// </summary>
        /// <param name="ligne">Ligne de la cellule</param>
        /// <param name="col">Colonne de la cellule</param>
        /// <returns>Nombre de mines adjacentes (0-8)</returns>
        private int CompterMinesAdjacentes(int ligne, int col)
        {
            int compte = 0;

            // Vérifier les 8 cellules adjacentes
            for (int l = Math.Max(0, ligne - 1); l <= Math.Min(Lignes - 1, ligne + 1); l++)
            {
                for (int c = Math.Max(0, col - 1); c <= Math.Min(Colonnes - 1, col + 1); c++)
                {
                    // Ignorer la cellule elle-même
                    if (l == ligne && c == col)
                    {
                        continue;
                    }

                    if (Cellules[l, c].ContientMine)
                    {
                        compte++;
                    }
                }
            }

            return compte;
        }

        /// <summary>
        /// Obtient toutes les cellules adjacentes pour une cellule donnée
        /// </summary>
        /// <param name="ligne">Ligne de la cellule</param>
        /// <param name="col">Colonne de la cellule</param>
        /// <returns>Liste des cellules adjacentes</returns>
        public List<Cellule> ObtenirCellulesAdjacentes(int ligne, int col)
        {
            List<Cellule> cellulesAdjacentes = new List<Cellule>();

            for (int l = Math.Max(0, ligne - 1); l <= Math.Min(Lignes - 1, ligne + 1); l++)
            {
                for (int c = Math.Max(0, col - 1); c <= Math.Min(Colonnes - 1, col + 1); c++)
                {
                    // Ignorer la cellule elle-même
                    if (l == ligne && c == col)
                    {
                        continue;
                    }

                    cellulesAdjacentes.Add(Cellules[l, c]);
                }
            }

            return cellulesAdjacentes;
        }
    }
}

