namespace Game.Systems.SaveLoad
{
    using UnityEngine;
    using System.IO;
    using Game.Player.Controllers;
    using Game.Systems.Inventory;
    using Game.Systems.Health;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Singleton manager for game state persistence.
    /// Uses JsonUtility for serialization to Application.persistentDataPath.
    /// </summary>
    public class SaveLoadManager : MonoBehaviour
    {
        public static SaveLoadManager Instance { get; private set; }

        private string SavePath => Path.Combine(Application.persistentDataPath, "savegame.json");

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SaveGame()
        {
            SaveData data = new SaveData();

            // Find Player and gathering data
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                data.PlayerPosition = player.transform.position;
                data.CurrentHealth = player.Health.CurrentHealth;
                data.MaxHealth = player.Health.MaxHealth;
                data.CurrentAmmo = player.CurrentAmmo;
            }

            // Find Inventory and gathering data
            InventoryManager inventory = FindObjectOfType<InventoryManager>();
            if (inventory != null)
            {
                data.InventoryItemNames = inventory.GetItemNames();
            }

            data.CurrentLevelName = SceneManager.GetActiveScene().name;

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
            Debug.Log($"[SaveLoad] Game Saved to {SavePath}");
        }

        public void LoadGame()
        {
            if (!File.Exists(SavePath))
            {
                Debug.LogWarning("[SaveLoad] No save file found!");
                return;
            }

            string json = File.ReadAllText(SavePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // Restoration logic
            // 1. Scene loading if needed (simplified for now: assume same scene or manually handled)
            
            // 2. Player recovery
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                player.transform.position = data.PlayerPosition;
                player.Health.SetHealth(data.CurrentHealth);
                player.RestoreAmmo(data.CurrentAmmo);
            }

            // 3. Inventory recovery
            InventoryManager inventory = FindObjectOfType<InventoryManager>();
            if (inventory != null)
            {
                inventory.LoadInventory(data.InventoryItemNames);
            }

            Debug.Log("[SaveLoad] Game Loaded successfully.");
        }

        public void DeleteSave()
        {
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
                Debug.Log("[SaveLoad] Save file deleted.");
            }
        }
    }
}
