using A_Code.Health;
using UnityEngine;

namespace A_Code.Attack.Type.ProjectileAttack
{
    public sealed class ProjectileAttackSystem : MonoBehaviour
    {
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private GameObject _projectilePrefab;

        [SerializeField] private LayerMask _enemyLayer;

        [SerializeField] private int _damage = 10;
        [SerializeField] private float _attackRate = 1f;
        [SerializeField] private float _attackRadius = 5f;

        private float _timer;
        private Collider2D[] _hits = new Collider2D[16];

        private bool _isEnabled;

        private void Awake()
        {
            _isEnabled = false;
        }

        private void Update()
        {
            if (!_isEnabled)
                return;

            _timer += Time.deltaTime;

            if (_timer < _attackRate) return;
            _timer = 0f;

            var target = FindTargetInRadius();
            if (target == null) return;

            var projectile = Instantiate(_projectilePrefab, _shootPoint.position, Quaternion.identity);
            projectile.GetComponent<Projectile>()?.Init(_damage, target);
        }

        public void Enable()
        {
            if (_isEnabled) return;
            _isEnabled = true;
        }

        public void Disable()
        {
            if (!_isEnabled) return;
            _isEnabled = false;
        }

        private IHealthProvider FindTargetInRadius()
        {
            int count = Physics2D.OverlapCircleNonAlloc(transform.position, _attackRadius, _hits, _enemyLayer);

            IHealthProvider bestTarget = null;
            float bestDistanceSqr = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                var hit = _hits[i];
                if (hit == null) continue;

                var provider = hit.GetComponent<IHealthProvider>();
                if (provider == null || provider.Health == null) continue;

                float distSqr = (hit.transform.position - transform.position).sqrMagnitude;
                if (distSqr < bestDistanceSqr)
                {
                    bestDistanceSqr = distSqr;
                    bestTarget = provider;
                }
            }

            return bestTarget;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRadius);
        }
    }
}
