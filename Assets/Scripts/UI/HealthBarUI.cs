namespace Game.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using Game.Core.Events;

    /// <summary>
    /// Updates a slider or image based on player health events.
    /// Handles both Health Bar and visual updates.
    /// </summary>
    public class HealthBarUI : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private Image _fillImage;
        [SerializeField] private Color _fullColor = Color.green;
        [SerializeField] private Color _lowColor = Color.red;

        private int _maxHealth = 100;

        private void OnEnable()
        {
            GameEvents.OnPlayerDamaged += UpdateHealth;
        }

        private void OnDisable()
        {
            GameEvents.OnPlayerDamaged -= UpdateHealth;
        }

        public void Initialize(int currentHealth, int maxHealth)
        {
            _maxHealth = maxHealth;
            if (_healthSlider != null)
            {
                _healthSlider.maxValue = _maxHealth;
                _healthSlider.value = currentHealth;
            }
            UpdateVisuals(currentHealth);
        }

        private void UpdateHealth(int currentHealth, int maxHealth)
        {
            _maxHealth = maxHealth;
            if (_healthSlider != null)
            {
                _healthSlider.maxValue = _maxHealth;
                _healthSlider.value = currentHealth;
            }
            UpdateVisuals(currentHealth);
        }

        private void UpdateVisuals(int currentHealth)
        {
            if (_fillImage != null)
            {
                float ratio = (float)currentHealth / _maxHealth;
                _fillImage.color = Color.Lerp(_lowColor, _fullColor, ratio);
            }
        }
    }
}
