using System;
using System.Collections.Generic;
using Chocosweeper.Core.Modeles;

namespace Chocosweeper.Core.Controlleurs
{
    /// <summary>
    /// Arguments d'événement pour les événements de jeu
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
    /// Contrôle la logique du jeu et gère l'état du jeu
    /// </summary>
    public class ControlleurJeu
    {
        /// <summary>
        /// Plateau de jeu actuel
        /// </summary>
        public PlateauDeJeu Plateau { get; private set; }

        /// <summary>
        /// État du jeu actuel
        /// </summary>
        public EtatJeu Etat { get; private set; }

        /// <summary>
        /// Configuration du jeu actuelle
        /// </summary>
        public ConfigurationJeu Configuration { get; private set; }

        /// <summary>
        /// Événement déclenché lorsqu'une cellule est révélée
        /// </summary>
        public event EventHandler<EvenementJeuArgs> CelluleRevelee;

        /// <summary>
        /// Événement déclenché lorsqu'une cellule est marquée ou démarquée
        /// </summary>
        public event EventHandler<EvenementJeuArgs> CelluleMarquee;

        /// <summary>
        /// Événement déclenché lorsque le jeu est gagné
        /// </summary>
        public event EventHandler<EvenementJeuArgs> JeuGagne;

        /// <summary>
        /// Événement déclenché lorsque le jeu est perdu
        /// </summary>
        public event EventHandler<EvenementJeuArgs> JeuPerdu;

        /// <summary>
        /// Événement déclenché lorsque l'état du jeu change
        /// </summary>
        public event EventHandler<EvenementJeuArgs> EtatJeuChange;

        /// <summary>
        /// Crée un nouveau contrôleur de jeu avec la configuration spécifiée
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
        /// Démarre un nouveau jeu avec la configuration spécifiée
        /// </summary>
        /// <param name="configuration">Configuration du jeu</param>
        public void NouveauJeu(ConfigurationJeu configuration)
        {
            Configuration = configuration;
            InitialiserJeu();
        }

        /// <summary>
        /// Gère le premier clic sur une cellule, en s'assurant qu'elle est sécurisée
        /// </summary>
        /// <param name="ligne">Ligne de la cellule cliquée</param>
        /// <param name="col">Colonne de la cellule cliquée</param>
        private void GererPremierClic(int ligne, int col)
        {
            // Placer les mines, en s'assurant que la première cellule cliquée est sécurisée
            Plateau.PlacerMines(ligne, col);

            // Démarrer le jeu
            Etat.DemarrerJeu();
            OnEtatJeuChange(null);
        }

        /// <summary>
        /// Révèle une cellule à la position spécifiée
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

            // Impossible de révéler des cellules marquées ou déjà révélées
            if (cellule.EstMarquee || cellule.EstRevelee)
            {
                return;
            }

            // Gérer le premier clic
            if (Etat.Statut == StatutJeu.NonCommence)
            {
                GererPremierClic(ligne, col);
            }

            // Le jeu doit être en cours
            if (Etat.Statut != StatutJeu.EnCours)
            {
                return;
            }

            // Révéler la cellule
            cellule.EstRevelee = true;
            Etat.NombreCellulesRevelees++;
            OnCelluleRevelee(cellule);

            // Vérifier si le joueur a touché une mine
            if (cellule.ContientMine)
            {
                // Fin de partie
                Etat.TerminerJeu(StatutJeu.Perdu);
                RevelerToutesMines();
                OnJeuPerdu(cellule);
                return;
            }

            // Si la cellule n'a pas de mines adjacentes, révéler les cellules adjacentes
            if (cellule.MinesAdjacentes == 0)
            {
                RevelerCellulesAdjacentes(ligne, col);
            }

            // Vérifier si le joueur a gagné
            VerifierVictoire();
        }

        /// <summary>
        /// Révèle toutes les cellules adjacentes pour une cellule sans mines adjacentes
        /// </summary>
        /// <param name="ligne">Ligne de la cellule</param>
        /// <param name="col">Colonne de la cellule</param>
        private void RevelerCellulesAdjacentes(int ligne, int col)
        {
            List<Cellule> cellulesAdjacentes = Plateau.ObtenirCellulesAdjacentes(ligne, col);

            foreach (Cellule celluleAdjacente in cellulesAdjacentes)
            {
                // Ignorer les cellules déjà révélées ou marquées
                if (celluleAdjacente.EstRevelee || celluleAdjacente.EstMarquee)
                {
                    continue;
                }

                // Révéler récursivement les cellules
                RevelerCellule(celluleAdjacente.Ligne, celluleAdjacente.Colonne);
            }
        }

        /// <summary>
        /// Bascule le drapeau sur une cellule à la position spécifiée
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

            // Le jeu doit être en cours ou non commencé
            if (Etat.Statut != StatutJeu.EnCours && Etat.Statut != StatutJeu.NonCommence)
            {
                return;
            }

            // Démarrer le jeu s'il n'a pas encore commencé
            if (Etat.Statut == StatutJeu.NonCommence)
            {
                Etat.DemarrerJeu();
                OnEtatJeuChange(null);
            }

            Cellule cellule = Plateau.Cellules[ligne, col];

            // Impossible de marquer des cellules révélées
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
        /// Révèle toutes les mines sur le plateau
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
        /// Vérifie si le joueur a gagné le jeu
        /// </summary>
        private void VerifierVictoire()
        {
            // Le joueur gagne lorsque toutes les cellules sans mine sont révélées
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
        /// Déclenche l'événement CelluleRevelee
        /// </summary>
        /// <param name="cellule">Cellule révélée</param>
        protected virtual void OnCelluleRevelee(Cellule cellule)
        {
            CelluleRevelee?.Invoke(this, new EvenementJeuArgs(Etat, cellule));
        }

        /// <summary>
        /// Déclenche l'événement CelluleMarquee
        /// </summary>
        /// <param name="cellule">Cellule marquée</param>
        protected virtual void OnCelluleMarquee(Cellule cellule)
        {
            CelluleMarquee?.Invoke(this, new EvenementJeuArgs(Etat, cellule));
        }

        /// <summary>
        /// Déclenche l'événement JeuGagne
        /// </summary>
        /// <param name="cellule">Dernière cellule révélée</param>
        protected virtual void OnJeuGagne(Cellule cellule)
        {
            JeuGagne?.Invoke(this, new EvenementJeuArgs(Etat, cellule));
            OnEtatJeuChange(cellule);
        }

        /// <summary>
        /// Déclenche l'événement JeuPerdu
        /// </summary>
        /// <param name="cellule">Cellule mine qui a été révélée</param>
        protected virtual void OnJeuPerdu(Cellule cellule)
        {
            JeuPerdu?.Invoke(this, new EvenementJeuArgs(Etat, cellule));
            OnEtatJeuChange(cellule);
        }

        /// <summary>
        /// Déclenche l'événement EtatJeuChange
        /// </summary>
        /// <param name="cellule">Cellule qui a déclenché le changement d'état</param>
        protected virtual void OnEtatJeuChange(Cellule cellule)
        {
            EtatJeuChange?.Invoke(this, new EvenementJeuArgs(Etat, cellule));
        }
    }
}

