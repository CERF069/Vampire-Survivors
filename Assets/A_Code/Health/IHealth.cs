using UniRx;

namespace A_Code.Health
{
    public interface IHealth
    {
        IReadOnlyReactiveProperty<int> CurrentHp { get; }
        int MaxHp { get; }
        bool IsDead { get; }

        void TakeDamage(int amount);
        void Heal(int amount);
        void Kill();
    }
}
