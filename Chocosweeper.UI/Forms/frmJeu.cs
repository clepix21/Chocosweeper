using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Chocosweeper.Core.Controlleurs;
using Chocosweeper.Core.Modeles;
using Chocosweeper.Data.Depots;

namespace Chocosweeper.UI.Formulaires
{
    /// <summary>
    /// Formulaire principal de l'application
    /// </summary>
    public partial class frmJeu : Form
    {
        /// <summary>
        /// Contrôleur de jeu
        /// </summary>
        private ControlleurJeu _controlleurJeu;

        /// <summary>
        /// Dépôt de scores
        /// </summary>
        private DepotScores _depotScores;

        /// <summary>
        /// Dépôt de configurations
        /// </summary>
        private DepotConfigurations _depotConfigs;

        /// <summary>
        /// Minuteur pour mettre à jour le temps de jeu
        /// </summary>
        private Timer _minuteurJeu;

        /// <summary>
        /// Panneau contenant le plateau de jeu
        /// </summary>
        private Panel _panneauPlateau;

        /// <summary>
        /// Étiquette affichant le nombre de mines
        /// </summary>
        private Label _etiquetteMines;

        /// <summary>
        /// Étiquette affichant le temps écoulé
        /// </summary>
        private Label _etiquetteTemps;

        /// <summary>
        /// Bouton pour démarrer une nouvelle partie
        /// </summary>
        private Button _boutonNouvellePartie;

        /// <summary>
        /// Bouton pour afficher les meilleurs scores
        /// </summary>
        private Button _boutonMeilleursScores;

        /// <summary>
        /// Bouton pour afficher les paramètres
        /// </summary>
        private Button _boutonParametres;

        /// <summary>
        /// Taille de chaque cellule en pixels
        /// </summary>
        private const int TailleCellule = 30;

        /// <summary>
        /// Images pour le jeu
        /// </summary>
        private Dictionary<string, Image> _images;

        /// <summary>
        /// Crée un nouveau formulaire principal
        /// </summary>
        public frmJeu()
        {
            InitializeComponent();
            InitialiserDepots();
            ChargerImages();
            InitialiserControlleurJeu();
            InitialiserUI();
            InitialiserMinuteur();
        }

        /// <summary>
        /// Initialise les dépôts
        /// </summary>
        private void InitialiserDepots()
        {
            _depotScores = new DepotScores();
            _depotConfigs = new DepotConfigurations();
        }

        /// <summary>
        /// Charge les images pour le jeu
        /// </summary>
        private void ChargerImages()
        {
            _images = new Dictionary<string, Image>();

            // Charger les images depuis les ressources
            _images["drapeau"] = Properties.Resources.Flag;
            _images["mine"] = Properties.Resources.Flag;
            _images["explosion"] = Properties.Resources.Flag;

            // Créer des images pour les nombres
            for (int i = 0; i <= 8; i++)
            {
                _images[i.ToString()] = CreerImageNombre(i);
            }
        }

        /// <summary>
        /// Crée une image pour un nombre
        /// </summary>
        /// <param name="nombre">Nombre pour lequel créer une image</param>
        /// <returns>Image pour le nombre</returns>
        private Image CreerImageNombre(int nombre)
        {
            // Couleurs pour différents nombres
            Color[] couleurs = {
                Color.Transparent,  // 0
                Color.Blue,         // 1
                Color.Green,        // 2
                Color.Red,          // 3
                Color.DarkBlue,     // 4
                Color.Brown,        // 5
                Color.Teal,         // 6
                Color.Black,        // 7
                Color.Gray          // 8
            };

            Bitmap bitmap = new Bitmap(TailleCellule, TailleCellule);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.LightGray);

                if (nombre > 0)
                {
                    using (Font police = new Font("Arial", 14, FontStyle.Bold))
                    using (StringFormat format = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    })
                    {
                        g.DrawString(
                            nombre.ToString(),
                            police,
                            new SolidBrush(couleurs[nombre]),
                            new RectangleF(0, 0, TailleCellule, TailleCellule),
                            format);
                    }
                }
            }

            return bitmap;
        }

        /// <summary>
        /// Initialise le contrôleur de jeu
        /// </summary>
        private void InitialiserControlleurJeu()
        {
            ConfigurationJeu config = _depotConfigs.ObtenirDerniereConfiguration();
            _controlleurJeu = new ControlleurJeu(config);

            // S'abonner aux événements
            _controlleurJeu.CelluleRevelee += ControlleurJeu_CelluleRevelee;
            _controlleurJeu.CelluleMarquee += ControlleurJeu_CelluleMarquee;
            _controlleurJeu.JeuGagne += ControlleurJeu_JeuGagne;
            _controlleurJeu.JeuPerdu += ControlleurJeu_JeuPerdu;
            _controlleurJeu.EtatJeuChange += ControlleurJeu_EtatJeuChange;
        }

        /// <summary>
        /// Initialise l'interface utilisateur
        /// </summary>
        private void InitialiserUI()
        {
            // Définir les propriétés du formulaire
            Text = "Chocosweeper";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            // Créer les contrôles
            _panneauPlateau = new Panel
            {
                BorderStyle = BorderStyle.Fixed3D,
                Location = new Point(20, 60),
                AutoSize = true
            };

            _etiquetteMines = new Label
            {
                AutoSize = true,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(20, 20),
                Text = $"Mines: {_controlleurJeu.Configuration.NombreMines}"
            };

            _etiquetteTemps = new Label
            {
                AutoSize = true,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(150, 20),
                Text = "Temps: 0"
            };

            _boutonNouvellePartie = new Button
            {
                Text = "Nouvelle Partie",
                Location = new Point(250, 15),
                Size = new Size(100, 30)
            };
            _boutonNouvellePartie.Click += BoutonNouvellePartie_Click;

            _boutonMeilleursScores = new Button
            {
                Text = "Meilleurs Scores",
                Location = new Point(360, 15),
                Size = new Size(100, 30)
            };
            _boutonMeilleursScores.Click += BoutonMeilleursScores_Click;

            _boutonParametres = new Button
            {
                Text = "Paramètres",
                Location = new Point(470, 15),
                Size = new Size(100, 30)
            };
            _boutonParametres.Click += BoutonParametres_Click;

            // Ajouter les contrôles au formulaire
            Controls.Add(_panneauPlateau);
            Controls.Add(_etiquetteMines);
            Controls.Add(_etiquetteTemps);
            Controls.Add(_boutonNouvellePartie);
            Controls.Add(_boutonMeilleursScores);
            Controls.Add(_boutonParametres);

            // Créer le plateau de jeu
            CreerPlateauJeu();

            // Ajuster la taille du formulaire
            ClientSize = new Size(
                Math.Max(600, _panneauPlateau.Right + 20),
                _panneauPlateau.Bottom + 20);
        }

        /// <summary>
        /// Initialise le minuteur
        /// </summary>
        private void InitialiserMinuteur()
        {
            _minuteurJeu = new Timer
            {
                Interval = 1000 // 1 seconde
            };
            _minuteurJeu.Tick += MinuteurJeu_Tick;
        }

        /// <summary>
        /// Crée le plateau de jeu
        /// </summary>
        private void CreerPlateauJeu()
        {
            // Effacer les contrôles existants
            _panneauPlateau.Controls.Clear();

            // Créer les cellules
            for (int ligne = 0; ligne < _controlleurJeu.Plateau.Lignes; ligne++)
            {
                for (int col = 0; col < _controlleurJeu.Plateau.Colonnes; col++)
                {
                    Button boutonCellule = new Button
                    {
                        Size = new Size(TailleCellule, TailleCellule),
                        Location = new Point(col * TailleCellule, ligne * TailleCellule),
                        Tag = new Point(ligne, col),
                        BackColor = Color.White,
                        FlatStyle = FlatStyle.Popup
                    };

                    // Ajouter les gestionnaires d'événements
                    boutonCellule.MouseDown += BoutonCellule_MouseDown;

                    // Ajouter au panneau
                    _panneauPlateau.Controls.Add(boutonCellule);
                }
            }

            // Ajuster la taille du panneau
            _panneauPlateau.Size = new Size(
                _controlleurJeu.Plateau.Colonnes * TailleCellule,
                _controlleurJeu.Plateau.Lignes * TailleCellule);
        }

        /// <summary>
        /// Met à jour l'interface utilisateur en fonction de l'état du jeu
        /// </summary>
        private void MettreAJourUI()
        {
            // Mettre à jour l'étiquette des mines
            int minesRestantes = _controlleurJeu.Configuration.NombreMines - _controlleurJeu.Etat.NombreDrapeaux;
            _etiquetteMines.Text = $"Mines: {minesRestantes}";

            // Mettre à jour l'étiquette du temps
            _etiquetteTemps.Text = $"Temps: {_controlleurJeu.Etat.ObtenirSecondesEcoulees()}";
        }

        /// <summary>
        /// Met à jour un bouton de cellule en fonction de l'état de la cellule
        /// </summary>
        /// <param name="ligne">Ligne de la cellule</param>
        /// <param name="col">Colonne de la cellule</param>
        private void MettreAJourBoutonCellule(int ligne, int col)
        {
            // Trouver le bouton pour cette cellule
            Button boutonCellule = TrouverBoutonCellule(ligne, col);
            if (boutonCellule == null)
            {
                return;
            }

            Cellule cellule = _controlleurJeu.Plateau.Cellules[ligne, col];

            if (cellule.EstRevelee)
            {
                // La cellule est révélée
                boutonCellule.Enabled = false;
                boutonCellule.BackColor = Color.White;

                if (cellule.ContientMine)
                {
                    // Afficher la mine
                    boutonCellule.Image = _images["mine"];

                    // Si c'est la mine qui a causé la fin du jeu, afficher l'explosion
                    if (_controlleurJeu.Etat.Statut == StatutJeu.Perdu &&
                        _controlleurJeu.Plateau.Cellules[ligne, col] == _controlleurJeu.Plateau.Cellules[ligne, col])
                    {
                        boutonCellule.Image = _images["explosion"];
                        boutonCellule.BackColor = Color.Red;
                    }
                }
                else
                {
                    // Afficher le nombre
                    boutonCellule.Image = _images[cellule.MinesAdjacentes.ToString()];
                }
            }
            else if (cellule.EstMarquee)
            {
                // La cellule est marquée
                boutonCellule.Image = _images["drapeau"];
            }
            else
            {
                // La cellule n'est ni révélée ni marquée
                boutonCellule.Image = null;
                boutonCellule.BackColor = Color.White;
            }
        }

        /// <summary>
        /// Trouve le bouton pour une cellule
        /// </summary>
        /// <param name="ligne">Ligne de la cellule</param>
        /// <param name="col">Colonne de la cellule</param>
        /// <returns>Bouton pour la cellule</returns>
        private Button TrouverBoutonCellule(int ligne, int col)
        {
            foreach (Control controle in _panneauPlateau.Controls)
            {
                if (controle is Button bouton && bouton.Tag is Point point &&
                    point.X == ligne && point.Y == col)
                {
                    return bouton;
                }
            }

            return null;
        }

        /// <summary>
        /// Gère l'événement de cellule révélée
        /// </summary>
        private void ControlleurJeu_CelluleRevelee(object sender, EvenementJeuArgs e)
        {
            if (e.Cellule != null)
            {
                MettreAJourBoutonCellule(e.Cellule.Ligne, e.Cellule.Colonne);
            }
        }

        /// <summary>
        /// Gère l'événement de cellule marquée
        /// </summary>
        private void ControlleurJeu_CelluleMarquee(object sender, EvenementJeuArgs e)
        {
            if (e.Cellule != null)
            {
                MettreAJourBoutonCellule(e.Cellule.Ligne, e.Cellule.Colonne);
                MettreAJourUI();
            }
        }

        /// <summary>
        /// Gère l'événement de jeu gagné
        /// </summary>
        private void ControlleurJeu_JeuGagne(object sender, EvenementJeuArgs e)
        {
            _minuteurJeu.Stop();

            // Afficher un message de félicitations
            int temps = _controlleurJeu.Etat.ObtenirSecondesEcoulees();
            MessageBox.Show(
                $"Félicitations ! Vous avez gagné en {temps} secondes !",
                "Partie Gagnée",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            // Vérifier si c'est un meilleur score
            if (_depotScores.EstMeilleurScore(temps, _controlleurJeu.Configuration))
            {
                // Demander le nom du joueur
                string nomJoueur = "Joueur";
                using (var dialogue = new frmSaisie("Meilleur Score", "Entrez votre nom :"))
                {
                    if (dialogue.ShowDialog() == DialogResult.OK)
                    {
                        nomJoueur = dialogue.TexteSaisi;
                    }
                }

                // Enregistrer le score
                Score score = new Score(nomJoueur, temps, _controlleurJeu.Configuration);
                _depotScores.AjouterScore(score);

                // Afficher les meilleurs scores
                AfficherMeilleursScores();
            }
        }

        /// <summary>
        /// Gère l'événement de jeu perdu
        /// </summary>
        private void ControlleurJeu_JeuPerdu(object sender, EvenementJeuArgs e)
        {
            _minuteurJeu.Stop();

            // Révéler toutes les mines
            for (int ligne = 0; ligne < _controlleurJeu.Plateau.Lignes; ligne++)
            {
                for (int col = 0; col < _controlleurJeu.Plateau.Colonnes; col++)
                {
                    MettreAJourBoutonCellule(ligne, col);
                }
            }

            // Afficher un message
            MessageBox.Show(
                "Partie terminée ! Vous avez touché une mine !",
                "Partie Perdue",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// Gère l'événement de changement d'état du jeu
        /// </summary>
        private void ControlleurJeu_EtatJeuChange(object sender, EvenementJeuArgs e)
        {
            // Mettre à jour l'interface utilisateur
            MettreAJourUI();

            // Démarrer ou arrêter le minuteur
            if (_controlleurJeu.Etat.Statut == StatutJeu.EnCours)
            {
                _minuteurJeu.Start();
            }
            else
            {
                _minuteurJeu.Stop();
            }
        }

        /// <summary>
        /// Gère l'événement de tic du minuteur
        /// </summary>
        private void MinuteurJeu_Tick(object sender, EventArgs e)
        {
            // Mettre à jour l'étiquette du temps
            _etiquetteTemps.Text = $"Temps: {_controlleurJeu.Etat.ObtenirSecondesEcoulees()}";
        }

        /// <summary>
        /// Gère l'événement de clic de souris sur un bouton de cellule
        /// </summary>
        private void BoutonCellule_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Button bouton && bouton.Tag is Point point)
            {
                int ligne = point.X;
                int col = point.Y;

                if (e.Button == MouseButtons.Left)
                {
                    // Clic gauche - révéler la cellule
                    _controlleurJeu.RevelerCellule(ligne, col);
                }
                else if (e.Button == MouseButtons.Right)
                {
                    // Clic droit - basculer le drapeau
                    _controlleurJeu.BasculerDrapeau(ligne, col);
                }
            }
        }

        /// <summary>
        /// Gère l'événement de clic sur le bouton Nouvelle Partie
        /// </summary>
        private void BoutonNouvellePartie_Click(object sender, EventArgs e)
        {
            // Démarrer une nouvelle partie avec la configuration actuelle
            _controlleurJeu.InitialiserJeu();
            CreerPlateauJeu();
            MettreAJourUI();
        }

        /// <summary>
        /// Gère l'événement de clic sur le bouton Meilleurs Scores
        /// </summary>
        private void BoutonMeilleursScores_Click(object sender, EventArgs e)
        {
            AfficherMeilleursScores();
        }

        /// <summary>
        /// Gère l'événement de clic sur le bouton Paramètres
        /// </summary>
        private void BoutonParametres_Click(object sender, EventArgs e)
        {
            using (var dialogue = new frmParametres(_controlleurJeu.Configuration))
            {
                if (dialogue.ShowDialog() == DialogResult.OK)
                {
                    // Enregistrer la configuration
                    _depotConfigs.EnregistrerConfiguration(dialogue.Configuration);

                    // Démarrer une nouvelle partie avec la nouvelle configuration
                    _controlleurJeu.NouveauJeu(dialogue.Configuration);
                    CreerPlateauJeu();
                    MettreAJourUI();

                    // Ajuster la taille du formulaire
                    ClientSize = new Size(
                        Math.Max(600, _panneauPlateau.Right + 20),
                        _panneauPlateau.Bottom + 20);
                }
            }
        }

        /// <summary>
        /// Affiche le dialogue des meilleurs scores
        /// </summary>
        private void AfficherMeilleursScores()
        {
            using (var dialogue = new DialogueMeilleursScores(_depotScores, _controlleurJeu.Configuration))
            {
                dialogue.ShowDialog();
            }
        }

        /// <summary>
        /// Variable de concepteur requise
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoie les ressources utilisées
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Chocosweeper";
        }
    }
}

