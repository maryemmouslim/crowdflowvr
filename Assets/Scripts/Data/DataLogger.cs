using System;
using System.IO;
using System.Text;
using UnityEngine;

public class DataLogger : MonoBehaviour
{
    private string filePath;
    private float logInterval = 0.5f; // Intervalle de 500ms
    private float timer = 0f;

    // Compteur de frames requis par le dashboard de Meryem
    private int currentFrame = 0;

    void Start()
    {
        // 1. Détermination du chemin persistant d'Unity (s'adapte au dossier "projet5")
        string folderPath = Application.persistentDataPath;

        // 2. Génération d'un nom de fichier par session unique (ex: session_183015.csv)
        // indispensable pour que la commande glob.glob de Meryem trouve le fichier
        string timeStamp = DateTime.Now.ToString("HHmmss");
        filePath = Path.Combine(folderPath, $"session_{timeStamp}.csv");

        Debug.Log($"[DataLogger] Nouveau fichier de session créé ici : {filePath}");

        // 3. Initialisation du fichier CSV avec les en-têtes exacts de Meryem
        if (!File.Exists(filePath))
        {
            string header = "frame,ped_id,x,z,speed,state\n";
            File.WriteAllText(filePath, header, Encoding.UTF8);
        }
    }

    void Update()
    {
        // 4. Compteur pour exécuter le log toutes les 500ms
        timer += Time.deltaTime;
        if (timer >= logInterval)
        {
            LogData();
            timer = 0f; // Réinitialise le chrono
        }
    }

    void LogData()
    {
        // 5. Récupération de tous les agents de la scène
        // Note : Remplacement par la version récente FindObjectsByType pour éviter le warning obsolète
        CrowdAgent[] agents = FindObjectsByType<CrowdAgent>(FindObjectsSortMode.None);

        if (agents.Length == 0) return;

        // On incrémente l'index de frame globale pour marquer cette étape temporelle
        currentFrame++;
        StringBuilder sb = new StringBuilder();

        // 6. Boucle sur chaque agent pour extraire et formater les données
        foreach (var agent in agents)
        {
            if (agent == null) continue;

            int pedId = agent.GetInstanceID(); // Correspond à "ped_id"
            float x = agent.transform.position.x; // Correspond à "x"
            float z = agent.transform.position.z; // Correspond à "z"

            // --- Simulation temporaire (En attente d'Imane) ---
            float speed = UnityEngine.Random.Range(0.5f, 2.5f); // Vitesse aléatoire réaliste
            string state = "Walking"; // Statut par défaut ("Walking" ou "Evacuation" pour tester les compteurs)

            // Ajout de la ligne au format CSV exact
            sb.AppendLine($"{currentFrame},{pedId},{x.ToString("F2")},{z.ToString("F2")},{speed.ToString("F2")},{state}");
        }

        // 7. Écriture sécurisée dans le fichier
        try
        {
            File.AppendAllText(filePath, sb.ToString(), Encoding.UTF8);
        }
        catch (IOException ex)
        {
            // Évite le crash si Python tente de lire le fichier au même moment
            Debug.LogWarning($"[DataLogger] Fichier temporairement verrouillé par Python : {ex.Message}");
        }
    }
}