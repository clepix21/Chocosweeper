using System;
using System.Drawing;
using System.Windows.Forms;
using Chocosweeper.System; 
using System.ComponentModel; 

namespace Chocosweeper.UI
{
    public partial class MainForm : Form
    {
        private Tableau plateauDeJeu;
        private Button[,] boutons;
        private const int TailleCaseule = 30;
        private const int Marge = 10;

        private Image imageMine;
        private Image imageDrapeau;

        private int largeurPlateau = 9;
        private int hauteurPlateau = 9;
        private int nombreMines = 10;

        public MainForm()
        {
            InitializeComponent();

            // Charger les images
            imageMine = CreerImagePlaceholder(Color.Black, "M");
            imageDrapeau = CreerImagePlaceholder(Color.Red, "F");

            InitialiserJeu();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (composants != null)
                {
                    composants.Dispose();
                }
                if (imageMine != null)
                {
                    imageMine.Dispose();
                }
                if (imageDrapeau != null)
                {
                    imageDrapeau.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void InitialiserJeu()
        {
            plateauDeJeu = new Tableau(largeurPlateau, hauteurPlateau, nombreMines);

            this.ClientSize = new Size(
                largeurPlateau * TailleCaseule + 2 * Marge,
                hauteurPlateau * TailleCaseule + 2 * Marge + menuStrip1.Height
            );

            CreerBoutons();
        }

        private void CreerBoutons()
        {
            if (boutons != null)
            {
                foreach (Button bouton in boutons)
                {
                    if (bouton != null)
                    {
                        this.Controls.Remove(bouton);
                        bouton.Dispose();
                    }
                }
            }

            boutons = new Button[largeurPlateau, hauteurPlateau];

            for (int x = 0; x < largeurPlateau; x++)
            {
                for (int y = 0; y < hauteurPlateau; y++)
                {
                    Button bouton = new Button
                    {
                        Location = new Point(x * TailleCaseule + Marge, y * TailleCaseule + Marge + menuStrip1.Height),
                        Size = new Size(TailleCaseule, TailleCaseule),
                        Tag = new Point(x, y),
                        UseVisualStyleBackColor = true,
                        Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold)
                    };

                    bouton.MouseUp += Bouton_MouseUp;
                    boutons[x, y] = bouton;
                    this.Controls.Add(bouton);
                }
            }
        }

        private void Bouton_MouseUp(object sender, MouseEventArgs e)
        {
            Button bouton = (Button)sender;
            Point location = (Point)bouton.Tag;
            int x = location.X;
            int y = location.Y;

            if (e.Button == MouseButtons.Left)
            {
                plateauDeJeu.RévélerCase(x, y);
            }
            else if (e.Button == MouseButtons.Right)
            {
                plateauDeJeu.ToggleDrapeau(x, y);
            }

            MettreAJourUI();

            if (plateauDeJeu.GameOver)
            {
                MessageBox.Show("Game Over ! Vous avez heurté une mine.", "Game Over",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (plateauDeJeu.Victoire)
            {
                MessageBox.Show("Félicitations ! Vous avez gagné !", "Victoire",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MettreAJourUI()
        {
            for (int x = 0; x < largeurPlateau; x++)
            {
                for (int y = 0; y < hauteurPlateau; y++)
                {
                    Case Caseule = plateauDeJeu.Cases[x, y];
                    Button bouton = boutons[x, y];

                    if (Caseule.Révélé)
                    {
                        if (Caseule.Mine)
                        {
                            bouton.BackgroundImage = imageMine;
                            bouton.BackgroundImageLayout = ImageLayout.Stretch;
                            bouton.Text = "";
                        }
                        else
                        {
                            bouton.BackgroundImage = null;
                            bouton.Enabled = false;

                            if (Caseule.Mines_Adjacentes > 0)
                            {
                                bouton.Text = Caseule.Mines_Adjacentes.ToString();
                                bouton.ForeColor = ObtenirCouleurNombre(Caseule.Mines_Adjacentes);
                            }
                            else
                            {
                                bouton.Text = "";
                            }
                        }
                    }
                    else if (Caseule.Drapeau)
                    {
                        bouton.BackgroundImage = imageDrapeau;
                        bouton.BackgroundImageLayout = ImageLayout.Stretch;
                        bouton.Text = "";
                    }
                    else
                    {
                        bouton.BackgroundImage = null;
                        bouton.Text = "";
                        bouton.Enabled = true;
                    }
                }
            }
        }

        private Color ObtenirCouleurNombre(int number)
        {
            switch (number)
            {
                case 1:
                    return Color.Blue;
                case 2:
                    return Color.Green;
                case 3:
                    return Color.Red;
                case 4:
                    return Color.DarkBlue;
                case 5:
                    return Color.DarkRed;
                case 6:
                    return Color.Teal;
                case 7:
                    return Color.Black;
                case 8:
                    return Color.Gray;
                default:
                    return Color.Black;
            }
        }

        private Image CreerImagePlaceholder(Color color, string text)
        {
            Bitmap bitmap = new Bitmap(TailleCaseule, TailleCaseule);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
                using (Brush brush = new SolidBrush(color))
                {
                    g.FillEllipse(brush, 5, 5, TailleCaseule - 10, TailleCaseule - 10);
                }

                using (Font font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold))
                using (Brush textBrush = new SolidBrush(Color.White))
                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    g.DrawString(text, font, textBrush, new RectangleF(0, 0, TailleCaseule, TailleCaseule), sf);
                }
            }
            return bitmap;
        }

        private void nouvellePartieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitialiserJeu();
            MettreAJourUI();
        }

        private void debutantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            largeurPlateau = 9;
            hauteurPlateau = 9;
            nombreMines = 10;
            InitialiserJeu();
            MettreAJourUI();
        }

        private void intermediaireToolStripMenuItem_Click(object sender, EventArgs e)
        {
            largeurPlateau = 16;
            hauteurPlateau = 16;
            nombreMines = 40;
            InitialiserJeu();
            MettreAJourUI();
        }

        private void expertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            largeurPlateau = 30;
            hauteurPlateau = 16;
            nombreMines = 99;
            InitialiserJeu();
            MettreAJourUI();
        }

        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}