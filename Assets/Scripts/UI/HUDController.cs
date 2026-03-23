namespace Game.UI
{
    using UnityEngine;
    using TMPro;
    using UnityEngine.UI;
    using Game.Core.Events;

    /// <summary>
    /// Centralized controller for the Gameplay HUD.
    /// Manages Health Bar, Ammo Display, and Objective Text.
    /// </summary>
    public class HUDController : MonoBehaviour
    {
        [Header("Health UI")]
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private TextMeshProUGUI _healthText;

        [Header("Ammo UI")]
        [SerializeField] private TextMeshProUGUI _ammoText;

        [Header("Objective UI")]
        [SerializeField] private TextMeshProUGUI _objectiveText;
        [SerializeField] private GameObject _objectivePanel;

        private void OnEnable()
        {
            GameEvents.OnPlayerDamaged += UpdateHealth;
            GameEvents.OnAmmoChanged += UpdateAmmo;
            GameEvents.OnObjectiveChanged += UpdateObjective;
            GameEvents.OnShowHint += ShowTemporaryHint;
        }

        private void OnDisable()
        {
            GameEvents.OnPlayerDamaged -= UpdateHealth;
            GameEvents.OnAmmoChanged -= UpdateAmmo;
            GameEvents.OnObjectiveChanged -= UpdateObjective;
            GameEvents.OnShowHint -= ShowTemporaryHint;
        }

        private Coroutine _hintCoroutine;

        private void ShowTemporaryHint(string message, float duration)
        {
            if (_hintCoroutine != null) StopCoroutine(_hintCoroutine);
            _hintCoroutine = StartCoroutine(HintSequence(message, duration));
        }

        private System.Collections.IEnumerator HintSequence(string message, float duration)
        {
            UpdateObjective(message);
            yield return new WaitForSeconds(duration);
            UpdateObjective(string.Empty);
            _hintCoroutine = null;
        }

        private void UpdateHealth(int current, int max)
        {
            if (_healthSlider != null)
                _healthSlider.value = (float)current / max;

            if (_healthText != null)
                _healthText.text = $"{current} / {max}";
        }

        private void UpdateAmmo(int current, int max)
        {
            if (_ammoText != null)
                _ammoText.text = $"AMMO: {current} / {max}";
        }

        private void UpdateObjective(string description)
        {
            if (_objectiveText != null)
                _objectiveText.text = description;

            if (_objectivePanel != null)
                _objectivePanel.SetActive(!string.IsNullOrEmpty(description));
        }
    }
}
