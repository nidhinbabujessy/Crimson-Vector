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
        public static Action<int, int> OnAmmoChanged; // current, max
        public static Action<string> OnObjectiveChanged; // description

        // Inventory & Pickup Events
        public static Action<Game.Systems.Inventory.ItemData> OnItemPickedUp;
        public static Action<int> OnPlayerHealed;
        public static Action<float, float> OnPlayerSpeedBuffed; // multiplier, duration

        // Win Condition
        public static Action OnBossDied;

        // Audio Events
        public static Action OnPlayerAttack;
        public static Action OnPlayerDash;
        public static Action OnEnemyAttack;
        public static Action OnEnemyDeath;
        public static Action OnUIClick;

        // Boss Specific Audio
        public static Action OnBossDash;
        public static Action OnBossAreaAttack;
        public static Action OnBossPhaseTransition;

        // Movement
        public static Action<bool> OnPlayerMoveChange; // isMoving
    }
}
