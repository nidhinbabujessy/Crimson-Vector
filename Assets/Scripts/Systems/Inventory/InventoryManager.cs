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
    }
}
