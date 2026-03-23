namespace Game.Systems.Inventory
{
    using UnityEngine;
    using System.Collections.Generic;
    using Game.Core.Events;

    /// <summary>
    /// Central manager for the player's inventory.
    /// It is decoupled from Player Controller and UI by using events.
    /// </summary>
    public class InventoryManager : MonoBehaviour
    {
        // Visible in inspector for debug
        [SerializeField] private List<ItemData> _items = new List<ItemData>();

        private void OnEnable()
        {
            GameEvents.OnItemPickedUp += AddItem;
        }

        private void OnDisable()
        {
            GameEvents.OnItemPickedUp -= AddItem;
        }

        public void AddItem(ItemData item)
        {
            _items.Add(item);
            Debug.Log($"[Inventory] Added {item.ItemName}. Total items: {_items.Count}");

            // Show hint if it's a health item
            if (item is HealthItemData)
            {
                GameEvents.OnShowHint?.Invoke("Press U to heal", 3f);
            }
        }

        public void RemoveItem(ItemData item)
        {
            if (_items.Contains(item))
            {
                _items.Remove(item);
                Debug.Log($"[Inventory] Removed {item.ItemName}.");
            }
        }

        public void UseItem(ItemData item)
        {
            if (_items.Contains(item))
            {
                item.Use();       // Execute item logic (e.g. heal, buff)
                RemoveItem(item); // Consume it
                Debug.Log($"[Inventory] Used {item.ItemName}. Remaining items: {_items.Count}");
            }
        }

        // --- Basic Debug Support without UI ---
        private void Update()
        {
            // Press U to test item usage from inventory
            if (Input.GetKeyDown(KeyCode.U) && _items.Count > 0)
            {
                UseItem(_items[0]);
            }
        }
        // --- Save/Load Integration ---

        /// <summary>
        /// Returns a list of unique identifiers (Internal Name) for all current items in inventory.
        /// Falls back to Asset name if ItemName is missing.
        /// </summary>
        public List<string> GetItemNames()
        {
            List<string> names = new List<string>();
            foreach (var item in _items)
            {
                if (item == null) continue;
                
                // Priority: ItemName field > Asset Name
                string identifier = !string.IsNullOrEmpty(item.ItemName) ? item.ItemName : item.name;
                names.Add(identifier);
            }
            return names;
        }

        /// <summary>
        /// Clears and reloads the inventory from a list of item names.
        /// Requires the ItemData assets to be in a Resources folder or found via search.
        /// For simplicity in this interview project, we'll use Resources.Load or a simple lookup.
        /// </summary>
        public void LoadInventory(List<string> itemNames)
        {
            _items.Clear();
            foreach (string name in itemNames)
            {
                // In a real project, you'd use a more robust ID system or Addressables.
                // Here we assume items are named consistently.
                ItemData data = Resources.Load<ItemData>($"Items/{name}");
                if (data != null)
                {
                    _items.Add(data);
                }
                else
                {
                    // Fallback: search all ItemData in project (slow, but works for small projects)
                    // In this context, we'll just log a warning if not found in Resources.
                    Debug.LogWarning($"[Inventory] Could not find ItemData for {name}. Make sure it's in Resources/Items/");
                }
            }
        }
    }
}
