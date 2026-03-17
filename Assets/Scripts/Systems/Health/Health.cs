namespace Game.Systems.Health
{
    using UnityEngine;
    using UnityEngine.Events;
    using Game.Core.Events;

    /// <summary>
    /// Reusable health component for any entity that can take damage.
    /// Event-driven to avoid direct dependencies on UI or gameplay controllers.
    /// </summary>
    public class Health : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int _maxHealth = 100;
        
        public int CurrentHealth { get; private set; }
        public int MaxHealth => _maxHealth;

        [Header("Optional Local Events")]
        public UnityEvent<int> OnDamaged;
        public UnityEvent OnDeath;

        private void Awake()
        {
            CurrentHealth = _maxHealth;
        }

        /// <summary>
        /// Reduces health and triggers events.
        /// </summary>
        /// <param name="amount">Damage amount</param>
        public void TakeDamage(int amount)
        {
            if (CurrentHealth <= 0) return;

            CurrentHealth -= amount;
            CurrentHealth = Mathf.Max(CurrentHealth, 0);

            // Optional localized event (useful for simple specific callbacks in the Inspector)
            OnDamaged?.Invoke(CurrentHealth);

            // You can also emit global events here if this specifically targets the player, 
            // but often the player script handles observing this and firing GameEvents.OnPlayerDamaged.
            // For now, this is kept generic.

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            OnDeath?.Invoke();
            // Typically followed by calling the global event if this is the player
        }
    }
}
