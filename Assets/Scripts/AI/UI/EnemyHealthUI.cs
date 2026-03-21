using UnityEngine;
using UnityEngine.UI;
using Game.Systems.Health;

namespace Game.AI.UI
{
    /// <summary>
    /// Updates a world-space Slider based on an entity's Health component.
    /// </summary>
    public class EnemyHealthUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private Health _health;

        private void Awake()
        {
            // Attempt to auto-find if not assigned
            if (_health == null)
            {
                _health = GetComponentInParent<Health>();
            }

            if (_hpSlider == null)
            {
                _hpSlider = GetComponentInChildren<Slider>();
            }
        }

        private void Start()
        {
            if (_health != null && _hpSlider != null)
            {
                _hpSlider.maxValue = _health.MaxHealth;
                _hpSlider.value = _health.CurrentHealth;
                
                // Subscribe to health changes
                _health.OnDamaged.AddListener(HandleHealthChanged);
            }
        }

        private void OnDestroy()
        {
            if (_health != null)
            {
                _health.OnDamaged.RemoveListener(HandleHealthChanged);
            }
        }

        private void HandleHealthChanged(int currentHealth)
        {
            if (_hpSlider != null)
            {
                _hpSlider.value = currentHealth;
            }
        }
    }
}
