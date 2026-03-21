using UnityEngine;
using System.Collections;
using Game.Systems.Health;

namespace Game.AI.Common
{
    /// <summary>
    /// Provides visual feedback when the object takes damage.
    /// Flashes the material color.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class DamageFeedback : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Color _flashColor = Color.white;
        [SerializeField] private float _flashDuration = 0.1f;

        private Color _originalColor;
        private Health _health;
        private Coroutine _flashCoroutine;

        private void Awake()
        {
            _health = GetComponent<Health>();
            if (_renderer == null)
            {
                _renderer = GetComponentInChildren<Renderer>();
            }

            if (_renderer != null)
            {
                _originalColor = _renderer.material.color;
            }
        }

        private void OnEnable()
        {
            if (_health != null)
            {
                _health.OnDamaged.AddListener(HandleDamaged);
            }
        }

        private void OnDisable()
        {
            if (_health != null)
            {
                _health.OnDamaged.RemoveListener(HandleDamaged);
            }
        }

        private void HandleDamaged(int currentHealth)
        {
            if (_flashCoroutine != null)
            {
                StopCoroutine(_flashCoroutine);
            }
            _flashCoroutine = StartCoroutine(FlashRoutine());
        }

        private IEnumerator FlashRoutine()
        {
            if (_renderer == null) yield break;

            _renderer.material.color = _flashColor;
            yield return new WaitForSeconds(_flashDuration);
            _renderer.material.color = _originalColor;
            
            _flashCoroutine = null;
        }
    }
}
