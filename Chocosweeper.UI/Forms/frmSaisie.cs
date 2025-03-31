using System;
using System.Drawing;
using System.Windows.Forms;

namespace Chocosweeper.UI.Forms
{
    /// <summary>
    /// Dialogue pour obtenir une saisie de l'utilisateur
    /// </summary>
    public partial class frmSaisie : Form
    {
        /// <summary>
        /// Zone de texte pour la saisie
        /// </summary>
        private TextBox _zoneTexteSaisie;

        /// <summary>
        /// Bouton OK
        /// </summary>
        private Button _boutonOK;

        /// <summary>
        /// Bouton Annuler
        /// </summary>
        private Button _boutonAnnuler;

        /// <summary>
        /// Obtient le texte saisi
        /// </summary>
        public string TexteSaisi => _zoneTexteSaisie.Text;

        /// <summary>
        /// Cr�e un nouveau dialogue de saisie
        /// </summary>
        /// <param name="titre">Titre du dialogue</param>
        /// <param name="invite">Texte d'invite</param>
        public frmSaisie(string titre, string invite)
        {
            InitializeComponent();
            InitialiserUI(titre, invite);
        }

        /// <summary>
        /// Initialise l'interface utilisateur
        /// </summary>
        /// <param name="titre">Titre du dialogue</param>
        /// <param name="invite">Texte d'invite</param>
        private void InitialiserUI(string titre, string invite)
        {
            // D�finir les propri�t�s du formulaire
            Text = titre;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(300, 120);

            // Cr�er les contr�les
            Label etiquetteInvite = new Label
            {
                Text = invite,
                Location = new Point(20, 20),
                AutoSize = true
            };

            _zoneTexteSaisie = new TextBox
            {
                Location = new Point(20, 50),
                Size = new Size(260, 20)
            };

            _boutonOK = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new Point(120, 80),
                Size = new Size(75, 23)
            };

            _boutonAnnuler = new Button
            {
                Text = "Annuler",
                DialogResult = DialogResult.Cancel,
                Location = new Point(205, 80),
                Size = new Size(75, 23)
            };

            // Ajouter les contr�les au formulaire
            Controls.Add(etiquetteInvite);
            Controls.Add(_zoneTexteSaisie);
            Controls.Add(_boutonOK);
            Controls.Add(_boutonAnnuler);

            // D�finir les boutons d'acceptation et d'annulation
            AcceptButton = _boutonOK;
            CancelButton = _boutonAnnuler;
        }

        /// <summary>
        /// Variable de concepteur requise
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoie les ressources utilis�es
        /// </summary>
        /// <param name="disposing">true si les ressources manag�es doivent �tre supprim�es ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// M�thode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette m�thode avec l'�diteur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 120);
            this.Text = "Saisie";
        }
    }
}

