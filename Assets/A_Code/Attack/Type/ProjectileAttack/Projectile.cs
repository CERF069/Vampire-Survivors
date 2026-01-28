using A_Code.Health;
using UnityEngine;

namespace A_Code.Attack.Type.ProjectileAttack
{
    public sealed class Projectile : MonoBehaviour
    {
        private int _damage;
        private float _speed = 10f;

        private IHealth _targetHealth;
        private Transform _targetTransform;

        public void Init(int damage, IHealthProvider targetProvider)
        {
            _damage = damage;
            _targetHealth = targetProvider.Health;
            _targetTransform = ((MonoBehaviour)targetProvider).transform;
        }

        private void Update()
        {
            if (_targetHealth == null || _targetTransform == null)
            {
                Destroy(gameObject);
                return;
            }

            var targetPos = _targetTransform.position;
            transform.position = Vector2.MoveTowards(transform.position, targetPos, _speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPos) < 0.1f)
            {
                _targetHealth.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}