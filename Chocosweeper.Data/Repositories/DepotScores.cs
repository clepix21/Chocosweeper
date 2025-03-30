using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Chocosweeper.Core.Modeles;

namespace Chocosweeper.Data.Depots
{
    /// <summary>
    /// Dépôt pour gérer les scores du jeu
    /// </summary>
    public class DepotScores
    {
        /// <summary>
        /// Chemin vers le fichier des scores
        /// </summary>
        private readonly string _cheminFichierScores;

        /// <summary>
        /// Liste de tous les scores
        /// </summary>
        private List<Score> _scores;

        /// <summary>
        /// Crée un nouveau dépôt de scores
        /// </summary>
        public DepotScores()
        {
            string cheminDonneesApp = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Chocosweeper");

            // Créer le répertoire s'il n'existe pas
            if (!Directory.Exists(cheminDonneesApp))
            {
                Directory.CreateDirectory(cheminDonneesApp);
            }

            _cheminFichierScores = Path.Combine(cheminDonneesApp, "scores.json");
            ChargerScores();
        }

        /// <summary>
        /// Charge les scores depuis le fichier
        /// </summary>
        private void ChargerScores()
        {
            if (File.Exists(_cheminFichierScores))
            {
                try
                {
                    string json = File.ReadAllText(_cheminFichierScores);
                    _scores = JsonSerializer.Deserialize<List<Score>>(json) ?? new List<Score>();
                }
                catch (Exception)
                {
                    _scores = new List<Score>();
                }
            }
            else
            {
                _scores = new List<Score>();
            }
        }

        /// <summary>
        /// Enregistre les scores dans le fichier
        /// </summary>
        private void EnregistrerScores()
        {
            try
            {
                string json = JsonSerializer.Serialize(_scores, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_cheminFichierScores, json);
            }
            catch (Exception)
            {
                // Journaliser l'erreur ou gérer l'exception
            }
        }

        /// <summary>
        /// Ajoute un nouveau score
        /// </summary>
        /// <param name="score">Score à ajouter</param>
        public void AjouterScore(Score score)
        {
            _scores.Add(score);
            EnregistrerScores();
        }

        /// <summary>
        /// Obtient tous les scores
        /// </summary>
        /// <returns>Liste de tous les scores</returns>
        public List<Score> ObtenirTousLesScores()
        {
            return _scores.ToList();
        }

        /// <summary>
        /// Obtient les meilleurs scores pour une configuration spécifique
        /// </summary>
        /// <param name="lignes">Nombre de lignes</param>
        /// <param name="colonnes">Nombre de colonnes</param>
        /// <param name="nombreMines">Nombre de mines</param>
        /// <param name="nombre">Nombre maximum de scores à retourner</param>
        /// <returns>Liste des meilleurs scores</returns>
        public List<Score> ObtenirMeilleursScores(int lignes, int colonnes, int nombreMines, int nombre = 10)
        {
            return _scores
                .Where(s => s.Lignes == lignes && s.Colonnes == colonnes && s.NombreMines == nombreMines)
                .OrderBy(s => s.Temps)
                .Take(nombre)
                .ToList();
        }

        /// <summary>
        /// Obtient les meilleurs scores pour une configuration spécifique
        /// </summary>
        /// <param name="config">Configuration du jeu</param>
        /// <param name="nombre">Nombre maximum de scores à retourner</param>
        /// <returns>Liste des meilleurs scores</returns>
        public List<Score> ObtenirMeilleursScores(ConfigurationJeu config, int nombre = 10)
        {
            return ObtenirMeilleursScores(config.Lignes, config.Colonnes, config.NombreMines, nombre);
        }

        /// <summary>
        /// Vérifie si un score est un meilleur score pour une configuration spécifique
        /// </summary>
        /// <param name="temps">Temps en secondes</param>
        /// <param name="config">Configuration du jeu</param>
        /// <param name="nombre">Nombre de meilleurs scores à considérer</param>
        /// <returns>Vrai si le score est un meilleur score</returns>
        public bool EstMeilleurScore(int temps, ConfigurationJeu config, int nombre = 10)
        {
            List<Score> meilleursScores = ObtenirMeilleursScores(config, nombre);

            // S'il y a moins de 'nombre' scores, c'est automatiquement un meilleur score
            if (meilleursScores.Count < nombre)
            {
                return true;
            }

            // Sinon, vérifier si le temps est meilleur que le pire meilleur score
            return temps < meilleursScores.Max(s => s.Temps);
        }
    }
}

