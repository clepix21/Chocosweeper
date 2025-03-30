using System;

namespace Chocosweeper.Core.Modeles
{
    /// <summary>
    /// Repr�sente le score d'un joueur
    /// </summary>
    public class Score
    {
        /// <summary>
        /// Identifiant unique pour le score
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nom du joueur
        /// </summary>
        public string NomJoueur { get; set; }

        /// <summary>
        /// Temps pris pour terminer le jeu (en secondes)
        /// </summary>
        public int Temps { get; set; }

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
        /// Date et heure � laquelle le score a �t� r�alis�
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Cr�e un nouveau score
        /// </summary>
        public Score()
        {
            Id = Guid.NewGuid();
            Date = DateTime.Now;
            NomJoueur = "Joueur";
        }

        /// <summary>
        /// Cr�e un nouveau score avec les param�tres sp�cifi�s
        /// </summary>
        /// <param name="nomJoueur">Nom du joueur</param>
        /// <param name="temps">Temps pris (en secondes)</param>
        /// <param name="config">Configuration du jeu</param>
        public Score(string nomJoueur, int temps, ConfigurationJeu config)
        {
            Id = Guid.NewGuid();
            NomJoueur = nomJoueur;
            Temps = temps;
            Lignes = config.Lignes;
            Colonnes = config.Colonnes;
            NombreMines = config.NombreMines;
            Date = DateTime.Now;
        }
    }
}

