using UnityEngine;
using Game.Systems.Health;

namespace Game.AI.Common
{
    /// <summary>
    /// Simple projectile script for both player and enemies.
    /// Moves forward and deals damage on collision with a target on the specified layer.
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _lifeTime = 5f;
        
        private int _damage;
        private LayerMask _targetLayer;
        private GameObject _owner;

        /// <summary>
        /// Initializes the projectile with damage, target layer, and the owner (to ignore self-hits).
        /// </summary>
        public void Initialize(int damage, LayerMask targetLayer, GameObject owner)
        {
            _damage = damage;
            _targetLayer = targetLayer;
            _owner = owner;

            // Robust Physics Ignore: Ignore all colliders on the owner to prevent self-hitting
            Collider myCollider = GetComponent<Collider>();
            if (myCollider != null && _owner != null)
            {
                Collider[] ownerColliders = _owner.GetComponentsInChildren<Collider>();
                foreach (var col in ownerColliders)
                {
                    Physics.IgnoreCollision(myCollider, col);
                }
            }

            Destroy(gameObject, _lifeTime);
        }
 
        private void Update()
        {
            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"[Projectile] OnTriggerEnter with: {other.gameObject.name} (Layer: {LayerMask.LayerToName(other.gameObject.layer)})");

            // Ignore the person who shot the bullet
            if (_owner != null && (other.gameObject == _owner || other.transform.IsChildOf(_owner.transform))) 
            {
                Debug.Log($"[Projectile] Ignoring hit with owner: {other.gameObject.name}");
                return;
            }

            // 1. Check if the hit object is a damageable target
            bool isTarget = ((1 << other.gameObject.layer) & _targetLayer) != 0;
            Debug.Log($"[Projectile] Layer Match: {isTarget} (Object: {other.gameObject.layer}, Mask: {_targetLayer.value})");

            if (isTarget)
            {
                Health health = other.GetComponentInParent<Health>();
                if (health != null)
                {
                    health.TakeDamage(_damage);
                    Debug.Log("enemy chaththu"); // Requested message
                    Debug.Log($"[Projectile Hit] {other.gameObject.name} hit! Damage: {_damage}, Target: {health.gameObject.name}, Remaining Health: {health.CurrentHealth}/{health.MaxHealth}");
                    
                    if (_owner.CompareTag("Player"))
                    {
                        Debug.Log($"[Projectile] Player hit {health.gameObject.name}. Disabling enemy.");
                       // health.gameObject.SetActive(false);
                    }
                }
            }

            // 2. ALWAYS destroy the bullet on any impact (walls, enemies, etc.)
            Debug.Log($"[Projectile] Destroying bullet on impact with {other.gameObject.name}");
            Destroy(gameObject);
        }
    }
}
