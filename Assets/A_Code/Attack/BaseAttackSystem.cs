using System;
using System.Collections.Generic;
using A_Code.Health;
using UniRx;

namespace A_Code.Attack
{
    public sealed class BaseAttackSystem
    {
        private readonly int _damage;
        private readonly float _cooldown;

        private bool _canAttack = true;
        private IDisposable _cooldownDisposable;

        public BaseAttackSystem(int damage, float cooldown)
        {
            _damage = damage;
            _cooldown = cooldown;
        }

        public void TryAttack(IReadOnlyList<IHealth> targets)
        {
            if (!_canAttack)
                return;

            if (targets == null || targets.Count == 0)
                return;

            var target = targets[0];
            if (target == null)
                return;

            _canAttack = false;
            target.TakeDamage(_damage);

            StartCooldown();
        }


        private void StartCooldown()
        {
            _cooldownDisposable?.Dispose();
            _cooldownDisposable = Observable.Timer(TimeSpan.FromSeconds(_cooldown))
                .Subscribe(_ => _canAttack = true);
        }
    }
}