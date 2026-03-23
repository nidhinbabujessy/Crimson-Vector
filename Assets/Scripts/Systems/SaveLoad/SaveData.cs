namespace Game.Systems.SaveLoad
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Serializable data structure for game state persistence.
    /// </summary>
    [Serializable]
    public class SaveData
    {
        // Player State
        public Vector3 PlayerPosition;
        public int CurrentHealth;
        public int MaxHealth;
        public int CurrentAmmo;

        // Inventory State
        public List<string> InventoryItemNames = new List<string>();

        // Progress State
        public string CurrentLevelName;
        public long SaveTimestamp;

        public SaveData()
        {
            SaveTimestamp = DateTime.Now.Ticks;
        }
    }
}
