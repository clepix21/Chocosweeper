using System;

namespace Chocosweeper.Core.Modeles
{
    /// <summary>
    /// Repr�sente une configuration de jeu avec taille de plateau et nombre de mines
    /// </summary>
    public class ConfigurationJeu
    {
        /// <summary>
        /// Niveaux de difficult� pr�d�finis
        /// </summary>
        public enum NiveauDifficulte
        {
            Debutant,
            Intermediaire,
            Expert,
            Personnalise
        }

        /// <summary>
        /// Nombre de lignes sur le plateau
        /// </summary>
        public int Lignes { get; set; }

        /// <summary>
        /// Nombre de colonnes sur le plateau
        /// </summary>
        public int Colonnes { get; set; }

        /// <summary>
        /// Nombre de mines sur le plateau
        /// </summary>
        public int NombreMines { get; set; }

        /// <summary>
        /// Niveau de difficult� de la configuration
        /// </summary>
        public NiveauDifficulte Difficulte { get; set; }

        /// <summary>
        /// Cr�e une nouvelle configuration de jeu avec les param�tres sp�cifi�s
        /// </summary>
        /// <param name="lignes">Nombre de lignes</param>
        /// <param name="colonnes">Nombre de colonnes</param>
        /// <param name="nombreMines">Nombre de mines</param>
        /// <param name="difficulte">Niveau de difficult�</param>
        public ConfigurationJeu(int lignes, int colonnes, int nombreMines, NiveauDifficulte difficulte = NiveauDifficulte.Personnalise)
        {
            Lignes = Math.Max(5, Math.Min(50, lignes));
            Colonnes = Math.Max(5, Math.Min(50, colonnes));

            // S'assurer qu'il y a au moins une cellule s�curis�e
            int maxMines = (Lignes * Colonnes) - 1;
            NombreMines = Math.Max(1, Math.Min(maxMines, nombreMines));

            Difficulte = difficulte;
        }

        /// <summary>
        /// Cr�e une configuration de jeu pr�d�finie bas�e sur le niveau de difficult�
        /// </summary>
        /// <param name="difficulte">Niveau de difficult�</param>
        /// <returns>Une nouvelle configuration de jeu</returns>
        public static ConfigurationJeu CreerDepuisDifficulte(NiveauDifficulte difficulte)
        {
            switch (difficulte)
            {
                case NiveauDifficulte.Debutant:
                    return new ConfigurationJeu(9, 9, 10, NiveauDifficulte.Debutant);
                case NiveauDifficulte.Intermediaire:
                    return new ConfigurationJeu(16, 16, 40, NiveauDifficulte.Intermediaire);
                case NiveauDifficulte.Expert:
                    return new ConfigurationJeu(16, 30, 99, NiveauDifficulte.Expert);
                default:
                    return new ConfigurationJeu(9, 9, 10, NiveauDifficulte.Personnalise);
            }
        }

        /// <summary>
        /// Obtient une repr�sentation sous forme de cha�ne de la configuration
        /// </summary>
        /// <returns>Repr�sentation sous forme de cha�ne</returns>
        public override string ToString()
        {
            return $"{Lignes}x{Colonnes} - {NombreMines} mines ({Difficulte})";
        }
    }
}

