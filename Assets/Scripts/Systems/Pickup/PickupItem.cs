namespace Game.Systems.Pickup
{
    using UnityEngine;
    using Game.Systems.Inventory;
    using Game.Core.Events;

    /// <summary>
    /// Attachable component for physical items dropped in world.
    /// Not coupled to Player. Interacts purely via global event.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class PickupItem : MonoBehaviour
    {
        [SerializeField] private ItemData _itemData;

        // Visual or interaction settings (can play FX if needed later)
        private void OnTriggerEnter(Collider other)
        {
            // Rely on Player tag as implemented in earlier days.
            if (other.CompareTag("Player"))
            {
                if (_itemData != null)
                {
                    GameEvents.OnItemPickedUp?.Invoke(_itemData);
                    Debug.Log($"[Pickup] {other.gameObject.name} picked up {_itemData.ItemName}");
                }
                else
                {
                    Debug.LogWarning("[Pickup] PickupItem has no ItemData assigned!", this);
                }

                // Destroy or pool the object
                Destroy(gameObject);
            }
        }
    }
}
