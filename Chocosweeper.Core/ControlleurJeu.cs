using System;
using System.Collections.Generic;
using Chocosweeper.Core.Modeles;

namespace Chocosweeper.Core.Controlleurs
{
    /// <summary>
    /// Arguments d'�v�nement pour les �v�nements de jeu
    /// </summary>
    public class EvenementJeuArgs : EventArgs
    {
        public EtatJeu EtatJeu { get; }
        public Cellule Cellule { get; }

        public EvenementJeuArgs(EtatJeu etatJeu, Cellule cellule = null)
        {
            EtatJeu = etatJeu;
            Cellule = cellule;
        }
    }

    /// <summary>
    /// Contr�le la logique du jeu et g�re l'�tat du jeu
    /// </summary>
    public class ControlleurJeu
    {
        /// <summary>
        /// Plateau de jeu actuel
        /// </summary>
        public PlateauDeJeu Plateau { get; private set; }

        /// <summary>
        /// �tat du jeu actuel
        /// </summary>
        public EtatJeu Etat { get; private set; }

        /// <summary>
        /// Configuration du jeu actuelle
        /// </summary>
        public ConfigurationJeu Configuration { get; private set; }

        /// <summary>
        /// �v�nement d�clench� lorsqu'une cellule est r�v�l�e
        /// </summary>
        public event EventHandler<EvenementJeuArgs> CelluleRevelee;

        /// <summary>
        /// �v�nement d�clench� lorsqu'une cellule est marqu�e ou d�marqu�e
        /// </summary>
        public event EventHandler<EvenementJeuArgs> CelluleMarquee;

        /// <summary>
        /// �v�nement d�clench� lorsque le jeu est gagn�
        /// </summary>
        public event EventHandler<EvenementJeuArgs> JeuGagne;

        /// <summary>
        /// �v�nement d�clench� lorsque le jeu est perdu
        /// </summary>
        public event EventHandler<EvenementJeuArgs> JeuPerdu;

        /// <summary>
        /// �v�nement d�clench� lorsque l'�tat du jeu change
        /// </summary>
        public event EventHandler<EvenementJeuArgs> EtatJeuChange;

        /// <summary>
        /// Cr�e un nouveau contr�leur de jeu avec la configuration sp�cifi�e
        /// </summary>
        /// <param name="configuration">Configuration du jeu</param>
        public ControlleurJeu(ConfigurationJeu configuration)
        {
            Configuration = configuration;
            InitialiserJeu();
        }

        /// <summary>
        /// Initialise un nouveau jeu avec la configuration actuelle
        /// </summary>
        public void InitialiserJeu()
        {
            Plateau = new PlateauDeJeu(Configuration.Lignes, Configuration.Colonnes, Configuration.NombreMines);
            Etat = new EtatJeu();
            OnEtatJeuChange(null);
        }

        /// <summary>
        /// D�marre un nouveau jeu avec la configuration sp�cifi�e
        /// </summary>
        /// <param name="configuration">Configuration du jeu</param>
        public void NouveauJeu(ConfigurationJeu configuration)
        {
            Configuration = configuration;
            InitialiserJeu();
        }

        /// <summary>
        /// G�re le premier clic sur une cellule, en s'assurant qu'elle est s�curis�e
        /// </summary>
        /// <param name="ligne">Ligne de la cellule cliqu�e</param>
        /// <param name="col">Colonne de la cellule cliqu�e</param>
        private void GererPremierClic(int ligne, int col)
        {
            // Placer les mines, en s'assurant que la premi�re cellule cliqu�e est s�curis�e
            Plateau.PlacerMines(ligne, col);

            // D�marrer le jeu
            Etat.DemarrerJeu();
            OnEtatJeuChange(null);
        }

        /// <summary>
        /// R�v�le une cellule � la position sp�cifi�e
        /// </summary>
        /// <param name="ligne">Ligne de la cellule</param>
        /// <param name="col">Colonne de la cellule</param>
        public void RevelerCellule(int ligne, int col)
        {
            // Valider la position
            if (ligne < 0 || ligne >= Plateau.Lignes || col < 0 || col >= Plateau.Colonnes)
            {
                return;
            }

            Cellule cellule = Plateau.Cellules[ligne, col];

            // Impossible de r�v�ler des cellules marqu�es ou d�j� r�v�l�es
            if (cellule.EstMarquee || cellule.EstRevelee)
            {
                return;
            }

            // G�rer le premier clic
            if (Etat.Statut == StatutJeu.NonCommence)
            {
                GererPremierClic(ligne, col);
            }

            // Le jeu doit �tre en cours
            if (Etat.Statut != StatutJeu.EnCours)
            {
                return;
            }

            // R�v�ler la cellule
            cellule.EstRevelee = true;
            Etat.NombreCellulesRevelees++;
            OnCelluleRevelee(cellule);

            // V�rifier si le joueur a touch� une mine
            if (cellule.ContientMine)
            {
                // Fin de partie
                Etat.TerminerJeu(StatutJeu.Perdu);
                RevelerToutesMines();
                OnJeuPerdu(cellule);
                return;
            }

            // Si la cellule n'a pas de mines adjacentes, r�v�ler les cellules adjacentes
            if (cellule.MinesAdjacentes == 0)
            {
                RevelerCellulesAdjacentes(ligne, col);
            }

            // V�rifier si le joueur a gagn�
            VerifierVictoire();
        }

        /// <summary>
        /// R�v�le toutes les cellules adjacentes pour une cellule sans mines adjacentes
        /// </summary>
        /// <param name="ligne">Ligne de la cellule</param>
        /// <param name="col">Colonne de la cellule</param>
        private void RevelerCellulesAdjacentes(int ligne, int col)
        {
            List<Cellule> cellulesAdjacentes = Plateau.ObtenirCellulesAdjacentes(ligne, col);

            foreach (Cellule celluleAdjacente in cellulesAdjacentes)
            {
                // Ignorer les cellules d�j� r�v�l�es ou marqu�es
                if (celluleAdjacente.EstRevelee || celluleAdjacente.EstMarquee)
                {
                    continue;
                }

                // R�v�ler r�cursivement les cellules
                RevelerCellule(celluleAdjacente.Ligne, celluleAdjacente.Colonne);
            }
        }

        /// <summary>
        /// Bascule le drapeau sur une cellule � la position sp�cifi�e
        /// </summary>
        /// <param name="ligne">Ligne de la cellule</param>
        /// <param name="col">Colonne de la cellule</param>
        public void BasculerDrapeau(int ligne, int col)
        {
            // Valider la position
            if (ligne < 0 || ligne >= Plateau.Lignes || col < 0 || col >= Plateau.Colonnes)
            {
                return;
            }

            // Le jeu doit �tre en cours ou non commenc�
            if (Etat.Statut != StatutJeu.EnCours && Etat.Statut != StatutJeu.NonCommence)
            {
                return;
            }

            // D�marrer le jeu s'il n'a pas encore commenc�
            if (Etat.Statut == StatutJeu.NonCommence)
            {
                Etat.DemarrerJeu();
                OnEtatJeuChange(null);
            }

            Cellule cellule = Plateau.Cellules[ligne, col];

            // Impossible de marquer des cellules r�v�l�es
            if (cellule.EstRevelee)
            {
                return;
            }

            // Basculer le drapeau
            cellule.EstMarquee = !cellule.EstMarquee;
            Etat.NombreDrapeaux += cellule.EstMarquee ? 1 : -1;
            OnCelluleMarquee(cellule);
        }

        /// <summary>
        /// R�v�le toutes les mines sur le plateau
        /// </summary>
        private void RevelerToutesMines()
        {
            for (int ligne = 0; ligne < Plateau.Lignes; ligne++)
            {
                for (int col = 0; col < Plateau.Colonnes; col++)
                {
                    Cellule cellule = Plateau.Cellules[ligne, col];

                    if (cellule.ContientMine && !cellule.EstRevelee)
                    {
                        cellule.EstRevelee = true;
                        OnCelluleRevelee(cellule);
                    }
                }
            }
        }

        /// <summary>
        /// V�rifie si le joueur a gagn� le jeu
        /// </summary>
        private void VerifierVictoire()
        {
            // Le joueur gagne lorsque toutes les cellules sans mine sont r�v�l�es
            int totalCellules = Plateau.Lignes * Plateau.Colonnes;
            int cellulesNonMines = totalCellules - Plateau.NombreMines;

            if (Etat.NombreCellulesRevelees == cellulesNonMines)
            {
                Etat.TerminerJeu(StatutJeu.Gagne);

                // Marquer toutes les mines
                for (int ligne = 0; ligne < Plateau.Lignes; ligne++)
                {
                    for (int col = 0; col < Plateau.Colonnes; col++)
                    {
                        Cellule cellule = Plateau.Cellules[ligne, col];

                        if (cellule.ContientMine && !cellule.EstMarquee)
                        {
                            cellule.EstMarquee = true;
                            Etat.NombreDrapeaux++;
                            OnCelluleMarquee(cellule);
                        }
                    }
                }

                OnJeuGagne(null);
            }
        }

        /// <summary>
        /// D�clenche l'�v�nement CelluleRevelee
        /// </summary>
        /// <param name="cellule">Cellule r�v�l�e</param>
        protected virtual void OnCelluleRevelee(Cellule cellule)
        {
            CelluleRevelee?.Invoke(this, new EvenementJeuArgs(Etat, cellule));
        }

        /// <summary>
        /// D�clenche l'�v�nement CelluleMarquee
        /// </summary>
        /// <param name="cellule">Cellule marqu�e</param>
        protected virtual void OnCelluleMarquee(Cellule cellule)
        {
            CelluleMarquee?.Invoke(this, new EvenementJeuArgs(Etat, cellule));
        }

        /// <summary>
        /// D�clenche l'�v�nement JeuGagne
        /// </summary>
        /// <param name="cellule">Derni�re cellule r�v�l�e</param>
        protected virtual void OnJeuGagne(Cellule cellule)
        {
            JeuGagne?.Invoke(this, new EvenementJeuArgs(Etat, cellule));
            OnEtatJeuChange(cellule);
        }

        /// <summary>
        /// D�clenche l'�v�nement JeuPerdu
        /// </summary>
        /// <param name="cellule">Cellule mine qui a �t� r�v�l�e</param>
        protected virtual void OnJeuPerdu(Cellule cellule)
        {
            JeuPerdu?.Invoke(this, new EvenementJeuArgs(Etat, cellule));
            OnEtatJeuChange(cellule);
        }

        /// <summary>
        /// D�clenche l'�v�nement EtatJeuChange
        /// </summary>
        /// <param name="cellule">Cellule qui a d�clench� le changement d'�tat</param>
        protected virtual void OnEtatJeuChange(Cellule cellule)
        {
            EtatJeuChange?.Invoke(this, new EvenementJeuArgs(Etat, cellule));
        }
    }
}

