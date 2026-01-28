using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace A_Code.Attack.Type.OrbitingAttack
{
    public sealed class OrbitAttackSystem : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private GameObject _weaponPrefab;

        [SerializeField] private int _damage = 10;
        [SerializeField] private int _count = 3;
        [SerializeField] private float _radius = 2.8f;
        [SerializeField] private float _rotationSpeed = 120f;


        private readonly List<Transform> _weapons = new();
        private float _angleOffset;
        private bool _isEnabled;
        
        private void Awake()
        {
            gameObject.SetActive(false);
            _isEnabled = false;
        }

        private void Update()
        {
            if (!_isEnabled)
                return;

            RotateWeapons();
        }

        public void Enable()
        {
            if (_isEnabled)
                return;

            _isEnabled = true;
            gameObject.SetActive(true);
            SpawnWeapons();
        }

        public void Disable()
        {
            if (!_isEnabled)
                return;

            _isEnabled = false;
            ClearWeapons();
            gameObject.SetActive(false);
        }

        private void SpawnWeapons()
        {
            ClearWeapons();

            for (int i = 0; i < _count; i++)
            {
                var weapon = Instantiate(_weaponPrefab, transform);
                
                var angle = 360f / _count * i;
                var rad = angle * Mathf.Deg2Rad;

                var x = Mathf.Cos(rad) * _radius;
                var y = Mathf.Sin(rad) * _radius;

                weapon.transform.localPosition = new Vector3(x, y, 0);

                weapon.GetComponent<IOrbitingWeapon>()?.Init(_damage);
                _weapons.Add(weapon.transform);
            }
        }

        private void RotateWeapons()
        {
            _angleOffset += _rotationSpeed * Time.deltaTime;

            for (int i = 0; i < _weapons.Count; i++)
            {
                var angle = 360f / _weapons.Count * i + _angleOffset;
                var rad = angle * Mathf.Deg2Rad;

                var x = Mathf.Cos(rad) * _radius;
                var y = Mathf.Sin(rad) * _radius;

                _weapons[i].localPosition = new Vector3(x, y, 0);
            }
        }

        private void ClearWeapons()
        {
            foreach (var w in _weapons)
            {
                if (w != null)
                    Destroy(w.gameObject);
            }
            _weapons.Clear();
        }
    }
}
