using UniRx;
using UnityEngine;

namespace A_Code.Health
{
    public sealed class Health : IHealth
    {
        private readonly ReactiveProperty<int> _currentHp;
        public IReadOnlyReactiveProperty<int> CurrentHp => _currentHp;

        public int MaxHp { get; }
        public bool IsDead => _currentHp.Value <= 0;

        public Health(int maxHp)
        {
            MaxHp = Mathf.Max(1, maxHp); // важно
            _currentHp = new ReactiveProperty<int>(MaxHp);
        }

        public void TakeDamage(int amount)
        {
            if (IsDead) return;
            _currentHp.Value = Mathf.Clamp(_currentHp.Value - amount, 0, MaxHp);
        }

        public void Heal(int amount)
        {
            if (IsDead) return;
            _currentHp.Value = Mathf.Clamp(_currentHp.Value + amount, 0, MaxHp);
        }

        public void Kill()
        {
            _currentHp.Value = 0;
        }

        public void Reset()
        {
            _currentHp.Value = MaxHp;
        }
    }
}