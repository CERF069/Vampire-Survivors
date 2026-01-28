using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace A_Code.Health
{
    public sealed class HealthBarView : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private MonoBehaviour _target; // сюда прокидываем Player / Enemy

        [SerializeField] private float _smoothSpeed = 10f;
    
        private float _targetValue;

        private void Start()
        {
            if (_target == null)
            {
                Debug.LogError("HealthBarView: target is not assigned!", this);
                return;
            }

            if (!(_target is IHealthProvider health))
            {
                Debug.LogError("HealthBarView: target doesn't implement IHealth!", _target);
                return;
            }

            Init(health);
        }

        public void Init(IHealthProvider target)
        {

            _slider.maxValue = target.Health.MaxHp;
            _targetValue = target.Health.MaxHp;

            target.Health.CurrentHp
                .Subscribe(hp =>
                {
                    _targetValue = hp;
                })
                .AddTo(this);

            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    _slider.value = Mathf.Lerp(_slider.value, _targetValue, Time.deltaTime * _smoothSpeed);
                })
                .AddTo(this);
        }
    }
}
