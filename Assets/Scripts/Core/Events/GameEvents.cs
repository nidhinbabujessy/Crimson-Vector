namespace Game.Core.Events
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Static event system for handling cross-system communication without tight coupling.
    /// </summary>
    public static class GameEvents
    {
        // Player events
        public static Action<int, int> OnPlayerDamaged; // current, max
        public static Action OnPlayerDied;

        // Inventory & Pickup Events
        public static Action<Game.Systems.Inventory.ItemData> OnItemPickedUp;
        public static Action<int> OnPlayerHealed;
        public static Action<float, float> OnPlayerSpeedBuffed; // multiplier, duration
    }
}
