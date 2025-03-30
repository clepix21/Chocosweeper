using System;
using System.Windows.Forms;
using Chocosweeper.UI.Formulaires;

namespace Chocosweeper.UI
{
    /// <summary>
    /// Classe principale du programme
    /// </summary>
    static class Programme
    {
        /// <summary>
        /// Point d'entrée principal de l'application
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormulairePrincipal());
        }
    }
}

