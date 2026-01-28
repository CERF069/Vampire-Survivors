using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace A_Code.Systems.ChoiceAbility
{
    public abstract class CardBase : MonoBehaviour
    {
        public string CardId;
        public ReactiveCommand<CardBase> OnSelected { get; } = new ReactiveCommand<CardBase>();

        protected virtual void Awake()
        {
            var button = GetComponent<Button>();

            if (button == null)
            {
                Debug.LogError("CardBase: Button не найден на карточке!");
                return;
            }

            button.onClick.AddListener(() =>
            {
                OnSelected.Execute(this);
                OnSelect();
            });
        }

        protected abstract void OnSelect();
    }
}