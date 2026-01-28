using System;
using System.Collections.Generic;
using A_Code.Health;
using UniRx;
using UnityEngine;

namespace A_Code.D_Debug
{
    public sealed class DamageZone : MonoBehaviour
    {
        [SerializeField] private int _damagePerSecond = 10;

        private readonly Dictionary<IHealth, IDisposable> _active = new Dictionary<IHealth, IDisposable>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IHealthProvider>(out var provider))
            {
                var health = provider.Health;

                if (_active.ContainsKey(health))
                    return;

                var sub = Observable.Interval(System.TimeSpan.FromSeconds(1))
                    .Subscribe(_ => health.TakeDamage(_damagePerSecond));

                _active.Add(health, sub);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<IHealthProvider>(out var provider))
            {
                var health = provider.Health;

                if (_active.TryGetValue(health, out var sub))
                {
                    sub.Dispose();
                    _active.Remove(health);
                }
            }
        }

        private void OnDisable()
        {
            foreach (var sub in _active.Values)
                sub.Dispose();

            _active.Clear();
        }
    }
}