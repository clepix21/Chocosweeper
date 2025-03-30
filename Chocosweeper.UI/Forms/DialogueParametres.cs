using System;
using System.Windows.Forms;
using Chocosweeper.Core.Modeles;

namespace Chocosweeper.UI.Formulaires
{
    /// <summary>
    /// Dialogue pour configurer les paramètres du jeu
    /// </summary>
    public partial class DialogueParametres : Form
    {
        /// <summary>
        /// Configuration actuelle du jeu
        /// </summary>
        public ConfigurationJeu Configuration { get; private set; }

        /// <summary>
        /// Bouton radio pour la difficulté débutant
        /// </summary>
        private RadioButton _boutonRadioDebutant;

        /// <summary>
        /// Bouton radio pour la difficulté intermédiaire
        /// </summary>
        private RadioButton _boutonRadioIntermediaire;

        /// <summary>
        /// Bouton radio pour la difficulté expert
        /// </summary>
        private RadioButton _boutonRadioExpert;

        /// <summary>
        /// Bouton radio pour la difficulté personnalisée
        /// </summary>
        private RadioButton _boutonRadioPersonnalise;

        /// <summary>
        /// Contrôle numérique pour les lignes
        /// </summary>
        private NumericUpDown _numeriqueUpDownLignes;

        /// <summary>
        /// Contrôle numérique pour les colonnes
        /// </summary>
        private NumericUpDown _numeriqueUpDownColonnes;

        /// <summary>
        /// Contrôle numérique pour les mines
        /// </summary>
        private NumericUpDown _numeriqueUpDownMines;

        /// <summary>
        /// Bouton OK
        /// </summary>
        private Button _boutonOK;

        /// <summary>
        /// Bouton Annuler
        /// </summary>
        private Button _boutonAnnuler;

        /// <summary>
        /// Crée un nouveau dialogue de paramètres
        /// </summary>
        /// <param name="configuration">Configuration actuelle du jeu</param>
        public DialogueParametres(ConfigurationJeu configuration)
        {
            InitializeComponent();
            Configuration = configuration;
            InitialiserUI();
            ChargerConfiguration();
        }

        /// <summary>
        /// Initialise l'interface utilisateur
        /// </summary>
        private void InitialiserUI()
        {
            // Définir les propriétés du formulaire
            Text = "Paramètres du Jeu";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new System.Drawing.Size(300, 250);

            // Créer les contrôles
            _boutonRadioDebutant = new RadioButton
            {
                Text = "Débutant (9x9, 10 mines)",
                Location = new System.Drawing.Point(20, 20),
                AutoSize = true
            };
            _boutonRadioDebutant.CheckedChanged += BoutonRadioDifficulte_CheckedChanged;

            _boutonRadioIntermediaire = new RadioButton
            {
                Text = "Intermédiaire (16x16, 40 mines)",
                Location = new System.Drawing.Point(20, 50),
                AutoSize = true
            };
            _boutonRadioIntermediaire.CheckedChanged += BoutonRadioDifficulte_CheckedChanged;

            _boutonRadioExpert = new RadioButton
            {
                Text = "Expert (16x30, 99 mines)",
                Location = new System.Drawing.Point(20, 80),
                AutoSize = true
            };
            _boutonRadioExpert.CheckedChanged += BoutonRadioDifficulte_CheckedChanged;

            _boutonRadioPersonnalise = new RadioButton
            {
                Text = "Personnalisé :",
                Location = new System.Drawing.Point(20, 110),
                AutoSize = true
            };
            _boutonRadioPersonnalise.CheckedChanged += BoutonRadioDifficulte_CheckedChanged;

            // Créer les contrôles de paramètres personnalisés
            Label etiquetteLignes = new Label
            {
                Text = "Lignes :",
                Location = new System.Drawing.Point(40, 140),
                AutoSize = true
            };

            _numeriqueUpDownLignes = new NumericUpDown
            {
                Location = new System.Drawing.Point(120, 138),
                Size = new System.Drawing.Size(60, 20),
                Minimum = 5,
                Maximum = 50,
                Value = 9
            };
            _numeriqueUpDownLignes.ValueChanged += ParametresPersonnalises_ValueChanged;

            Label etiquetteColonnes = new Label
            {
                Text = "Colonnes :",
                Location = new System.Drawing.Point(40, 170),
                AutoSize = true
            };

            _numeriqueUpDownColonnes = new NumericUpDown
            {
                Location = new System.Drawing.Point(120, 168),
                Size = new System.Drawing.Size(60, 20),
                Minimum = 5,
                Maximum = 50,
                Value = 9
            };
            _numeriqueUpDownColonnes.ValueChanged += ParametresPersonnalises_ValueChanged;

            Label etiquetteMines = new Label
            {
                Text = "Mines :",
                Location = new System.Drawing.Point(40, 200),
                AutoSize = true
            };

            _numeriqueUpDownMines = new NumericUpDown
            {
                Location = new System.Drawing.Point(120, 198),
                Size = new System.Drawing.Size(60, 20),
                Minimum = 1,
                Maximum = 500,
                Value = 10
            };

            // Créer les boutons
            _boutonOK = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new System.Drawing.Point(120, 230),
                Size = new System.Drawing.Size(75, 23)
            };
            _boutonOK.Click += BoutonOK_Click;

            _boutonAnnuler = new Button
            {
                Text = "Annuler",
                DialogResult = DialogResult.Cancel,
                Location = new System.Drawing.Point(205, 230),
                Size = new System.Drawing.Size(75, 23)
            };

            // Ajouter les contrôles au formulaire
            Controls.Add(_boutonRadioDebutant);
            Controls.Add(_boutonRadioIntermediaire);
            Controls.Add(_boutonRadioExpert);
            Controls.Add(_boutonRadioPersonnalise);
            Controls.Add(etiquetteLignes);
            Controls.Add(_numeriqueUpDownLignes);
            Controls.Add(etiquetteColonnes);
            Controls.Add(_numeriqueUpDownColonnes);
            Controls.Add(etiquetteMines);
            Controls.Add(_numeriqueUpDownMines);
            Controls.Add(_boutonOK);
            Controls.Add(_boutonAnnuler);

            // Définir les boutons d'acceptation et d'annulation
            AcceptButton = _boutonOK;
            CancelButton = _boutonAnnuler;
        }

        /// <summary>
        /// Charge la configuration actuelle
        /// </summary>
        private void ChargerConfiguration()
        {
            switch (Configuration.Difficulte)
            {
                case ConfigurationJeu.NiveauDifficulte.Debutant:
                    _boutonRadioDebutant.Checked = true;
                    break;
                case ConfigurationJeu.NiveauDifficulte.Intermediaire:
                    _boutonRadioIntermediaire.Checked = true;
                    break;
                case ConfigurationJeu.NiveauDifficulte.Expert:
                    _boutonRadioExpert.Checked = true;
                    break;
                case ConfigurationJeu.NiveauDifficulte.Personnalise:
                    _boutonRadioPersonnalise.Checked = true;
                    _numeriqueUpDownLignes.Value = Configuration.Lignes;
                    _numeriqueUpDownColonnes.Value = Configuration.Colonnes;
                    _numeriqueUpDownMines.Value = Configuration.NombreMines;
                    break;
            }

            MettreAJourEtatParametresPersonnalises();
        }

        /// <summary>
        /// Met à jour l'état activé des contrôles de paramètres personnalisés
        /// </summary>
        private void MettreAJourEtatParametresPersonnalises()
        {
            bool active = _boutonRadioPersonnalise.Checked;
            _numeriqueUpDownLignes.Enabled = active;
            _numeriqueUpDownColonnes.Enabled = active;
            _numeriqueUpDownMines.Enabled = active;
        }

        /// <summary>
        /// Met à jour le nombre maximum de mines en fonction de la taille du plateau
        /// </summary>
        private void MettreAJourMaxMines()
        {
            int maxMines = (int)(_numeriqueUpDownLignes.Value * _numeriqueUpDownColonnes.Value) - 1;
            _numeriqueUpDownMines.Maximum = maxMines;

            if (_numeriqueUpDownMines.Value > maxMines)
            {
                _numeriqueUpDownMines.Value = maxMines;
            }
        }

        /// <summary>
        /// Gère l'événement de changement de sélection du bouton radio de difficulté
        /// </summary>
        private void BoutonRadioDifficulte_CheckedChanged(object sender, EventArgs e)
        {
            MettreAJourEtatParametresPersonnalises();
        }

        /// <summary>
        /// Gère l'événement de changement de valeur des paramètres personnalisés
        /// </summary>
        private void ParametresPersonnalises_ValueChanged(object sender, EventArgs e)
        {
            MettreAJourMaxMines();
        }

        /// <summary>
        /// Gère l'événement de clic sur le bouton OK
        /// </summary>
        private void BoutonOK_Click(object sender, EventArgs e)
        {
            // Créer la configuration en fonction des options sélectionnées
            if (_boutonRadioDebutant.Checked)
            {
                Configuration = ConfigurationJeu.CreerDepuisDifficulte(ConfigurationJeu.NiveauDifficulte.Debutant);
            }
            else if (_boutonRadioIntermediaire.Checked)
            {
                Configuration = ConfigurationJeu.CreerDepuisDifficulte(ConfigurationJeu.NiveauDifficulte.Intermediaire);
            }
            else if (_boutonRadioExpert.Checked)
            {
                Configuration = ConfigurationJeu.CreerDepuisDifficulte(ConfigurationJeu.NiveauDifficulte.Expert);
            }
            else
            {
                Configuration = new ConfigurationJeu(
                    (int)_numeriqueUpDownLignes.Value,
                    (int)_numeriqueUpDownColonnes.Value,
                    (int)_numeriqueUpDownMines.Value,
                    ConfigurationJeu.NiveauDifficulte.Personnalise);
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
            this.ClientSize = new System.Drawing.Size(300, 250);
            this.Text = "Paramètres du Jeu";
        }
    }
}

