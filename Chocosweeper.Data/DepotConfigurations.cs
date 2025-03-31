using System;
using System.IO;
using System.Text.Json;

namespace Chocosweeper.Data
{
    /// <summary>
    /// D�p�t pour g�rer les configurations du jeu
    /// </summary>
    public class DepotConfigurations
    {
        /// <summary>
        /// Chemin vers le fichier de configuration
        /// </summary>
        private readonly string _cheminFichierConfig;

        /// <summary>
        /// Derni�re configuration utilis�e
        /// </summary>
        private Core.Modeles.ConfigurationJeu _derniereConfiguration;

        /// <summary>
        /// Cr�e un nouveau d�p�t de configurations
        /// </summary>
        public DepotConfigurations()
        {
            string cheminDonneesApp = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Chocosweeper");

            // Cr�er le r�pertoire s'il n'existe pas
            if (!Directory.Exists(cheminDonneesApp))
            {
                Directory.CreateDirectory(cheminDonneesApp);
            }

            _cheminFichierConfig = Path.Combine(cheminDonneesApp, "config.json");
            ChargerConfiguration();
        }

        /// <summary>
        /// Charge la derni�re configuration depuis le fichier
        /// </summary>
        private void ChargerConfiguration()
        {
            if (File.Exists(_cheminFichierConfig))
            {
                try
                {
                    string json = File.ReadAllText(_cheminFichierConfig);
                    _derniereConfiguration = JsonSerializer.Deserialize<Core.Modeles.ConfigurationJeu>(json);
                }
                catch (Exception)
                {
                    // Utiliser la configuration par d�faut si le fichier est corrompu
                    _derniereConfiguration = Core.Modeles.ConfigurationJeu.CreerDepuisDifficulte(
                        Core.Modeles.ConfigurationJeu.NiveauDifficulte.Debutant);
                }
            }
            else
            {
                // Utiliser la configuration par d�faut si le fichier n'existe pas
                _derniereConfiguration = Core.Modeles.ConfigurationJeu.CreerDepuisDifficulte(
                    Core.Modeles.ConfigurationJeu.NiveauDifficulte.Debutant);
            }
        }

        /// <summary>
        /// Enregistre la configuration dans le fichier
        /// </summary>
        /// <param name="configuration">Configuration � enregistrer</param>
        public void EnregistrerConfiguration(Core.Modeles.ConfigurationJeu configuration)
        {
            _derniereConfiguration = configuration;

            try
            {
                string json = JsonSerializer.Serialize(configuration, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_cheminFichierConfig, json);
            }
            catch (Exception)
            {
                // Journaliser l'erreur ou g�rer l'exception
            }
        }

        /// <summary>
        /// Obtient la derni�re configuration utilis�e
        /// </summary>
        /// <returns>Derni�re configuration utilis�e</returns>
        public Core.Modeles.ConfigurationJeu ObtenirDerniereConfiguration()
        {
            return _derniereConfiguration;
        }
    }
}

