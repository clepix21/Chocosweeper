using System;

namespace Chocosweeper.Core.Modeles
{
    /// <summary>
    /// Repr�sente l'�tat actuel du jeu
    /// </summary>
    public enum StatutJeu
    {
        NonCommence,
        EnCours,
        Gagne,
        Perdu
    }

    /// <summary>
    /// Repr�sente l'�tat actuel du jeu
    /// </summary>
    public class EtatJeu
    {
        /// <summary>
        /// Statut actuel du jeu
        /// </summary>
        public StatutJeu Statut { get; set; }

        /// <summary>
        /// Heure � laquelle le jeu a commenc�
        /// </summary>
        public DateTime HeureDebut { get; private set; }

        /// <summary>
        /// Heure � laquelle le jeu s'est termin� (gagn� ou perdu)
        /// </summary>
        public DateTime? HeureFin { get; private set; }

        /// <summary>
        /// Nombre de cellules qui ont �t� r�v�l�es
        /// </summary>
        public int NombreCellulesRevelees { get; set; }

        /// <summary>
        /// Nombre de drapeaux plac�s
        /// </summary>
        public int NombreDrapeaux { get; set; }

        /// <summary>
        /// Cr�e un nouvel �tat de jeu
        /// </summary>
        public EtatJeu()
        {
            Statut = StatutJeu.NonCommence;
            NombreCellulesRevelees = 0;
            NombreDrapeaux = 0;
        }

        /// <summary>
        /// D�marre le jeu
        /// </summary>
        public void DemarrerJeu()
        {
            Statut = StatutJeu.EnCours;
            HeureDebut = DateTime.Now;
            HeureFin = null;
        }

        /// <summary>
        /// Termine le jeu avec le statut sp�cifi�
        /// </summary>
        /// <param name="statut">Statut final du jeu (Gagne ou Perdu)</param>
        public void TerminerJeu(StatutJeu statut)
        {
            if (statut != StatutJeu.Gagne && statut != StatutJeu.Perdu)
            {
                throw new ArgumentException("Le jeu ne peut se terminer qu'avec le statut Gagne ou Perdu");
            }

            Statut = statut;
            HeureFin = DateTime.Now;
        }

        /// <summary>
        /// Obtient le temps �coul� depuis le d�but du jeu
        /// </summary>
        /// <returns>Temps �coul� en secondes</returns>
        public int ObtenirSecondesEcoulees()
        {
            if (Statut == StatutJeu.NonCommence)
            {
                return 0;
            }

            DateTime heureFinOuMaintenant = HeureFin ?? DateTime.Now;
            TimeSpan tempsEcoule = heureFinOuMaintenant - HeureDebut;
            return (int)tempsEcoule.TotalSeconds;
        }
    }
}

