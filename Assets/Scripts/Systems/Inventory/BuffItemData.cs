namespace Game.Systems.Inventory
{
    using UnityEngine;
    using Game.Core.Events;

    /// <summary>
    /// Item that gives the player a temporary speed buff.
    /// </summary>
    [CreateAssetMenu(fileName = "BuffItem", menuName = "Game/Inventory/Buff Item")]
    public class BuffItemData : ItemData
    {
        [Header("Buff Settings")]
        public float SpeedMultiplier = 1.5f;
        public float Duration = 5f;

        public override void Use()
        {
            GameEvents.OnPlayerSpeedBuffed?.Invoke(SpeedMultiplier, Duration);
            Debug.Log($"[Item] Used Buff Item: {ItemName}. Speed x{SpeedMultiplier} for {Duration}s.");
        }
    }
}
