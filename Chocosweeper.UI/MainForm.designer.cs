using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System;
namespace Chocosweeper.UI
{
    partial class MainForm
    {
        private IContainer composants = null;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem jeuToolStripMenuItem;
        private ToolStripMenuItem nouvellePartieToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem debutantToolStripMenuItem;
        private ToolStripMenuItem intermediaireToolStripMenuItem;
        private ToolStripMenuItem expertToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem quitterToolStripMenuItem;

        private void InitializeComponent()
        {
            this.composants = new Container();
            this.menuStrip1 = new MenuStrip();
            this.jeuToolStripMenuItem = new ToolStripMenuItem();
            this.nouvellePartieToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.debutantToolStripMenuItem = new ToolStripMenuItem();
            this.intermediaireToolStripMenuItem = new ToolStripMenuItem();
            this.expertToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.quitterToolStripMenuItem = new ToolStripMenuItem();

            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();

            // menuStrip1
            this.menuStrip1.Items.AddRange(new ToolStripItem[] { this.jeuToolStripMenuItem });
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new Size(284, 24);
            this.menuStrip1.TabIndex = 0;

            // jeuToolStripMenuItem
            this.jeuToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                    this.nouvellePartieToolStripMenuItem,
                    this.toolStripSeparator1,
                    this.debutantToolStripMenuItem,
                    this.intermediaireToolStripMenuItem,
                    this.expertToolStripMenuItem,
                    this.toolStripSeparator2,
                    this.quitterToolStripMenuItem
                });
            this.jeuToolStripMenuItem.Name = "jeuToolStripMenuItem";
            this.jeuToolStripMenuItem.Size = new Size(50, 20);
            this.jeuToolStripMenuItem.Text = "Jeu";

            // nouvellePartieToolStripMenuItem
            this.nouvellePartieToolStripMenuItem.Name = "nouvellePartieToolStripMenuItem";
            this.nouvellePartieToolStripMenuItem.Size = new Size(152, 22);
            this.nouvellePartieToolStripMenuItem.Text = "Nouvelle Partie";
            this.nouvellePartieToolStripMenuItem.Click += new EventHandler(this.nouvellePartieToolStripMenuItem_Click);

            // toolStripSeparator1
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(149, 6);

            // debutantToolStripMenuItem
            this.debutantToolStripMenuItem.Name = "debutantToolStripMenuItem";
            this.debutantToolStripMenuItem.Size = new Size(152, 22);
            this.debutantToolStripMenuItem.Text = "Débutant";
            this.debutantToolStripMenuItem.Click += new EventHandler(this.debutantToolStripMenuItem_Click);

            // intermediaireToolStripMenuItem
            this.intermediaireToolStripMenuItem.Name = "intermediaireToolStripMenuItem";
            this.intermediaireToolStripMenuItem.Size = new Size(152, 22);
            this.intermediaireToolStripMenuItem.Text = "Intermédiaire";
            this.intermediaireToolStripMenuItem.Click += new EventHandler(this.intermediaireToolStripMenuItem_Click);

            // expertToolStripMenuItem
            this.expertToolStripMenuItem.Name = "expertToolStripMenuItem";
            this.expertToolStripMenuItem.Size = new Size(152, 22);
            this.expertToolStripMenuItem.Text = "Expert";
            this.expertToolStripMenuItem.Click += new EventHandler(this.expertToolStripMenuItem_Click);

            // toolStripSeparator2
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(149, 6);

            // quitterToolStripMenuItem
            this.quitterToolStripMenuItem.Name = "quitterToolStripMenuItem";
            this.quitterToolStripMenuItem.Size = new Size(152, 22);
            this.quitterToolStripMenuItem.Text = "Quitter";
            this.quitterToolStripMenuItem.Click += new EventHandler(this.quitterToolStripMenuItem_Click);

            // MainForm
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(284, 261);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Chocosweeper";

            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
