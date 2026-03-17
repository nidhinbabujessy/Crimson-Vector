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
        public static Action<int> OnPlayerDamaged;
        public static Action OnPlayerDied;

        // Add more events here as needed...
    }
}
