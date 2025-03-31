using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Chocosweeper.Core.Modeles;
using Chocosweeper.Data.Depots;

namespace Chocosweeper.UI.Forms
{
    /// <summary>
    /// Dialogue pour afficher les meilleurs scores
    /// </summary>
    public partial class frmMilleursScores : Form
    {
        /// <summary>
        /// Dépôt de scores
        /// </summary>
        private readonly DepotScores _depotScores;

        /// <summary>
        /// Configuration actuelle du jeu
        /// </summary>
        private readonly ConfigurationJeu _configuration;

        /// <summary>
        /// Vue en liste pour afficher les scores
        /// </summary>
        private ListView _vueListeScores;

        /// <summary>
        /// Bouton Fermer
        /// </summary>
        private Button _boutonFermer;

        /// <summary>
        /// Crée un nouveau dialogue de meilleurs scores
        /// </summary>
        /// <param name="depotScores">Dépôt de scores</param>
        /// <param name="configuration">Configuration actuelle du jeu</param>
        public frmMilleursScores(DepotScores depotScores, ConfigurationJeu configuration)
        {
            InitializeComponent();
            _depotScores = depotScores;
            _configuration = configuration;
            InitialiserUI();
            ChargerScores();
        }

        /// <summary>
        /// Initialise l'interface utilisateur
        /// </summary>
        private void InitialiserUI()
        {
            // Définir les propriétés du formulaire
            Text = "Meilleurs Scores";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(400, 300);

            // Créer la vue en liste
            _vueListeScores = new ListView
            {
                Location = new Point(20, 20),
                Size = new Size(360, 220),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };

            // Ajouter les colonnes
            _vueListeScores.Columns.Add("Rang", 40);
            _vueListeScores.Columns.Add("Nom", 120);
            _vueListeScores.Columns.Add("Temps", 60);
            _vueListeScores.Columns.Add("Date", 140);

            // Créer le bouton Fermer
            _boutonFermer = new Button
            {
                Text = "Fermer",
                DialogResult = DialogResult.OK,
                Location = new Point(305, 250),
                Size = new Size(75, 23)
            };

            // Ajouter les contrôles au formulaire
            Controls.Add(_vueListeScores);
            Controls.Add(_boutonFermer);

            // Définir le bouton d'acceptation
            AcceptButton = _boutonFermer;
        }

        /// <summary>
        /// Charge les meilleurs scores
        /// </summary>
        private void ChargerScores()
        {
            // Effacer la vue en liste
            _vueListeScores.Items.Clear();

            // Obtenir les meilleurs scores pour la configuration actuelle
            List<Score> scores = _depotScores.ObtenirMeilleursScores(_configuration);

            // Ajouter les scores à la vue en liste
            for (int i = 0; i < scores.Count; i++)
            {
                Score score = scores[i];

                ListViewItem item = new ListViewItem((i + 1).ToString());
                item.SubItems.Add(score.NomJoueur);
                item.SubItems.Add(score.Temps.ToString());
                item.SubItems.Add(score.Date.ToString("g"));

                _vueListeScores.Items.Add(item);
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
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Text = "Meilleurs Scores";
        }
    }
}

