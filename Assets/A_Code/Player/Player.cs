using System.Collections.Generic;
using A_Code.Attack;
using A_Code.Health;
using A_Code.Player.Data;
using A_Code.Player.Interface;
using UnityEngine;
using Zenject;
using UniRx;
using UnityEngine.SceneManagement;

namespace A_Code.Player
{
    public sealed class Player : MonoBehaviour, IHealthProvider
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        private PlayerHealthConfig _configHealth;
      
        private Health.Health _health;
        public IHealth Health => _health;

        private IPlayerInput _input;
        private IMoveble _moveble;
        private PlayerMoveConfig _moveConfig;

        private CompositeDisposable _disposables;

        private readonly HashSet<IHealth> _targets = new HashSet<IHealth>();
        private readonly List<IHealth> _targetsCopy = new List<IHealth>();
        private PlayerAttackConfig _attackConfig;
        [SerializeField] private LayerMask _attackLayer;
        private BaseAttackSystem _attackSystem;

        [Inject]
        public void Construct(
            IPlayerInput input,
            IMoveble moveble,
            PlayerMoveConfig moveConfig,
            PlayerAttackConfig attackConfig, 
            PlayerHealthConfig healthConfig)
        {
            _input = input;
            _moveble = moveble;
            _moveConfig = moveConfig;
            _attackConfig = attackConfig;
            _configHealth = healthConfig;
        }

        private void Awake()
        {
            _health = new Health.Health(_configHealth.maxHp);
            _disposables = new CompositeDisposable();

            Health.CurrentHp
                .Subscribe(hp =>
                {
                    if (hp <= 0)
                        OnDie();
                })
                .AddTo(_disposables);

            _attackSystem = new BaseAttackSystem(_attackConfig.damage, _attackConfig.cooldown);
        }

        private void OnEnable()
        {
            _input?.Enable();
        }

        private void OnDisable()
        {
            _input?.Disable();
        }

        private void FixedUpdate()
        {
            _moveble.Move(_input.MoveDirection(), _moveConfig, _rigidbody);

            _targetsCopy.Clear();
            _targetsCopy.AddRange(_targets);

            _attackSystem.TryAttack(_targetsCopy);
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

        private void OnDie()
        {
            _disposables.Dispose();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
