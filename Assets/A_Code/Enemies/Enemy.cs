using System.Collections.Generic;
using A_Code.Attack;
using A_Code.Enemies.Data;
using A_Code.Enemies.Data.Normal;
using A_Code.Enemies.Interface;
using A_Code.Health;
using A_Code.Player.Interface;
using UniRx;
using UnityEngine;
using Zenject;

namespace A_Code.Enemies
{
    public class Enemy : EnemyBase, IHealthProvider
    {
        [SerializeField] private Rigidbody2D _rigidbody;

        private IMoveble _moveble;
        private NormalEnemyMoveConfig _moveConfig;
        private IEnemyTargetProvider _targetProvider;
        
        private EnemyHealthConfig _healthConfig;
        private Health.Health _health;
        public IHealth Health => _health;
        
        private readonly HashSet<IHealth> _targets = new HashSet<IHealth>();
        private readonly List<IHealth> _targetsCopy = new List<IHealth>();
        private EnemyAttackConfig _attackConfig;
        [SerializeField] private LayerMask _attackLayer;
        private BaseAttackSystem _attackSystem;
        
        [Inject]
        public void Construct(
            IMoveble moveble,
            NormalEnemyMoveConfig moveConfig,
            IEnemyTargetProvider targetProvider,
            EnemyHealthConfig healthConfig, 
            EnemyAttackConfig attackConfig)
        {
            _moveble = moveble;
            _moveConfig = moveConfig;
            _targetProvider = targetProvider;
            _healthConfig = healthConfig;
            _attackConfig = attackConfig;
        }
        private void Awake()
        {
            _health = new Health.Health(_healthConfig.maxHP);
            _attackSystem = new BaseAttackSystem(_attackConfig.damage, _attackConfig.cooldown);
            
            Health.CurrentHp
                .Where(hp => hp <= 0)
                .Delay(System.TimeSpan.FromSeconds(0.2f))
                .Subscribe(_ => OnDie())
                .AddTo(this);
            
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & _attackLayer) == 0) return;

            if (other.isTrigger) return;

            if (other.TryGetComponent<IHealthProvider>(out var provider))
            {
                _targets.Add(provider.Health);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & _attackLayer) == 0) return;

            if (other.isTrigger) return;

            if (other.TryGetComponent<IHealthProvider>(out var provider))
            {
                _targets.Remove(provider.Health);
            }
        }
        public override void OnSpawn(Vector2 position)
        {
            _health.Reset();
            base.OnSpawn(position);
        }
        private void FixedUpdate()
        {
            _moveble.Move(_targetProvider.GetDirection(transform), _moveConfig, _rigidbody);
            
            _targetsCopy.Clear();
            _targetsCopy.AddRange(_targets);

            _attackSystem.TryAttack(_targetsCopy);
        }

    }
}