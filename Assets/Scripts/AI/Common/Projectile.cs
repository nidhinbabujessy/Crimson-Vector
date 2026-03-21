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
            // Ignore the person who shot the bullet
            if (_owner != null && (other.gameObject == _owner || other.transform.IsChildOf(_owner.transform))) 
            {
                return;
            }

            // 1. Check if the hit object is a damageable target
            if (((1 << other.gameObject.layer) & _targetLayer) != 0)
            {
                Health health = other.GetComponentInParent<Health>();
                if (health != null)
                {
                    health.TakeDamage(_damage);
                    Debug.Log("enemy chaththu"); // Requested message
                    Debug.Log($"[Projectile Hit] {other.gameObject.name} hit! Damage: {_damage}, Target: {health.gameObject.name}, Remaining Health: {health.CurrentHealth}/{health.MaxHealth}");
                }
            }

            // 2. ALWAYS destroy the bullet on impact
            Destroy(gameObject);
        }
    }
}
