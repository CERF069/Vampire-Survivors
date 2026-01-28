using A_Code.Systems.Experience;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;

public class ExperienceUI : MonoBehaviour
{
    [SerializeField] private Slider _experienceSlider;
    [SerializeField] private TMP_Text _levelText;
    private float _fillSpeed = 5f;

    private IExperienceSystem _experienceSystem;
    private CompositeDisposable _disposables = new CompositeDisposable();

    private float _targetValue;

    [Inject]
    public void Construct(IExperienceSystem experienceSystem)
    {
        _experienceSystem = experienceSystem;
    }

    private void Start()
    {
        _experienceSlider.maxValue = _experienceSystem.ExperienceForNextLevel;
        _experienceSlider.value = 0f;
        _targetValue = 0f;

        // Подписка на опыт
        _experienceSystem.CurrentExperience
            .Subscribe(value =>
            {
                _targetValue = value;
            })
            .AddTo(_disposables);

        // Подписка на уровень
        _experienceSystem.CurrentLevel
            .Subscribe(level =>
            {
                _levelText.text = $"Level {level}";
            })
            .AddTo(_disposables);

        // Плавное обновление слайдера каждый кадр
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                _experienceSlider.value = Mathf.Lerp(
                    _experienceSlider.value,
                    _targetValue,
                    Time.deltaTime * _fillSpeed
                );
            })
            .AddTo(_disposables);
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}