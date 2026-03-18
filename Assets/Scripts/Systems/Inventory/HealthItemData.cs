namespace Game.Systems.Inventory
{
    using UnityEngine;
    using Game.Core.Events;

    /// <summary>
    /// Item that heals the player when used.
    /// </summary>
    [CreateAssetMenu(fileName = "HealthItem", menuName = "Game/Inventory/Health Item")]
    public class HealthItemData : ItemData
    {
        [Header("Health Settings")]
        public int HealAmount = 25;

        public override void Use()
        {
            // Fully decoupled event triggering. Anywhere the player is, it listens.
            GameEvents.OnPlayerHealed?.Invoke(HealAmount);
            Debug.Log($"[Item] Used Health Item: {ItemName}. Healed for {HealAmount}.");
        }
    }
}
