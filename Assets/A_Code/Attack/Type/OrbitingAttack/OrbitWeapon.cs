using System.Collections.Generic;
using A_Code.Health;
using UnityEngine;

namespace A_Code.Attack.Type.OrbitingAttack
{
    public sealed class OrbitWeapon : MonoBehaviour, IOrbitingWeapon
    {
        private int _damage;
        private float _damageInterval = 0.2f;
        private float _timer;

        private readonly HashSet<IHealth> _targets = new();

        public void Init(int damage)
        {
            _damage = damage;
        }

        private void Update()
        {
            if (_targets.Count == 0)
                return;

            _timer += Time.deltaTime;

            if (_timer < _damageInterval)
                return;

            _timer = 0f;
            
            var copy = new List<IHealth>(_targets);

            foreach (var target in copy)
            {
                if (target == null) continue;
                target.TakeDamage(_damage);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IHealthProvider>(out var provider))
            {
                _targets.Add(provider.Health);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<IHealthProvider>(out var provider))
            {
                _targets.Remove(provider.Health);
            }
        }
    }
}