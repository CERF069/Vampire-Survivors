using System.Collections.Generic;
using A_Code.Systems.Experience;
using UniRx;
using UnityEngine;
using Zenject;

namespace A_Code.Systems.ChoiceAbility
{
    public class LevelUpCardUI : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Transform _cardsContainer;
        [SerializeField] private GameObject[] _cardPrefabs;

        private IExperienceSystem _experienceSystem;
        private CompositeDisposable _disposables = new CompositeDisposable();
        private readonly HashSet<string> _pickedCards = new();

        private DiContainer _container;
        
        [Inject]
        public void Construct(IExperienceSystem experienceSystem, DiContainer container)
        {
            _experienceSystem = experienceSystem;
            _container = container;

            Debug.Log($"[Inject] experienceSystem: {_experienceSystem}");
            Debug.Log($"[Inject] container: {_container}");
        }

        private void Start()
        {
            Debug.Log("[Start] LevelUpCardUI Start called");

            if (_container == null)
                Debug.LogError("[Start] _container is null! Zenject не инжектит LevelUpCardUI");
            else
                Debug.Log("[Start] _container успешно инжектирован");

            if (_cardsContainer == null)
                Debug.LogError("[Start] _cardsContainer не назначен в инспекторе");
            else
                Debug.Log("[Start] _cardsContainer назначен");

            if (_panel == null)
                Debug.LogError("[Start] _panel не назначен в инспекторе");
            else
                Debug.Log("[Start] _panel назначен");

            _panel.SetActive(false);

            if (_experienceSystem == null)
            {
                Debug.LogError("[Start] _experienceSystem is null!");
                return;
            }

            _experienceSystem.OnLevelUp
                .Delay(System.TimeSpan.FromSeconds(0.6f))
                .Subscribe(level =>
                {
                    Debug.Log($"[OnLevelUp] Игрок достиг уровня {level}");
                    ShowLevelUpCards(level);
                })
                .AddTo(_disposables);
        }

        private void ShowLevelUpCards(int level)
        {
            Debug.Log("[ShowLevelUpCards] Вызван метод показа карточек");

            if (_cardsContainer == null)
            {
                Debug.LogError("[ShowLevelUpCards] _cardsContainer is null, нельзя показывать карты!");
                return;
            }

            foreach (Transform child in _cardsContainer)
            {
                if (child == null)
                {
                    Debug.LogWarning("[ShowLevelUpCards] Обнаружен null child в _cardsContainer");
                    continue;
                }
                Destroy(child.gameObject);
            }

            var availableCards = new List<GameObject>();

            if (_cardPrefabs == null)
            {
                Debug.LogError("[ShowLevelUpCards] _cardPrefabs массив не назначен!");
                return;
            }

            for (int i = 0; i < _cardPrefabs.Length; i++)
            {
                var cardPrefab = _cardPrefabs[i];
                if (cardPrefab == null)
                {
                    Debug.LogError($"[ShowLevelUpCards] В массиве _cardPrefabs элемент {i} null!");
                    continue;
                }

                var cardBase = cardPrefab.GetComponent<CardBase>();
                if (cardBase == null)
                {
                    Debug.LogError($"[ShowLevelUpCards] У префаба {cardPrefab.name} нет компонента CardBase!");
                    continue;
                }

                if (!_pickedCards.Contains(cardBase.CardId))
                {
                    availableCards.Add(cardPrefab);
                    Debug.Log($"[ShowLevelUpCards] Карта {cardBase.name} добавлена в список доступных карт");
                }
                else
                {
                    Debug.Log($"[ShowLevelUpCards] Карта {cardBase.name} уже выбрана, пропускаем");
                }
            }

            int cardsToShow = Mathf.Min(3, availableCards.Count);
            Debug.Log($"[ShowLevelUpCards] Доступно карт: {availableCards.Count}, показываем: {cardsToShow}");

            if (cardsToShow == 0)
            {
                Debug.Log("[ShowLevelUpCards] Нет доступных карт для показа");
                return;
            }

            Time.timeScale = 0f;
            _panel.SetActive(true);
            Debug.Log("[ShowLevelUpCards] Включена панель выбора карт, время остановлено");

            for (int i = 0; i < cardsToShow; i++)
            {
                int randomIndex = Random.Range(0, availableCards.Count);
                var prefab = availableCards[randomIndex];

                if (_container == null)
                {
                    Debug.LogError("[ShowLevelUpCards] _container null! Нельзя инстанцировать карты");
                    return;
                }

                var instantiated = _container.InstantiatePrefab(prefab, _cardsContainer);
                if (instantiated == null)
                {
                    Debug.LogError($"[ShowLevelUpCards] Не удалось создать префаб карты {prefab.name}");
                    continue;
                }

                RectTransform rt = instantiated.GetComponent<RectTransform>();
                if (rt != null)
                {
                    rt.anchoredPosition = new Vector2(i * 500, 0);
                    Debug.Log($"[ShowLevelUpCards] Карта {prefab.name} позиционирована");
                }
                else
                {
                    Debug.LogWarning($"[ShowLevelUpCards] У карты {prefab.name} нет RectTransform");
                }

                var card = instantiated.GetComponent<CardBase>();
                if (card == null)
                {
                    Debug.LogError($"[ShowLevelUpCards] У инстанциированной карты {prefab.name} нет CardBase!");
                    continue;
                }

                card.OnSelected
                    .Subscribe(selectedCard => OnCardSelected(selectedCard))
                    .AddTo(_disposables);

                availableCards.RemoveAt(randomIndex);
                Debug.Log($"[ShowLevelUpCards] Карта {card.name} подписана на событие OnSelected и удалена из списка");
            }
        }

        private void OnCardSelected(CardBase card)
        {
            if (card == null)
            {
                Debug.LogError("[OnCardSelected] Выбрана null карта!");
                return;
            }

            Debug.Log($"[OnCardSelected] Выбрана карта: {card.name}, CardId: {card.CardId}");
            _pickedCards.Add(card.CardId);

            _panel.SetActive(false);
            Debug.Log("[OnCardSelected] Панель закрыта");

            Observable.Timer(System.TimeSpan.FromSeconds(0.5f))
                .Subscribe(_ =>
                {
                    Time.timeScale = 1.0f;
                    Debug.Log("[OnCardSelected] Время возобновлено");
                })
                .AddTo(_disposables);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
            Debug.Log("[OnDestroy] Все подписки очищены");
        }
    }
}
