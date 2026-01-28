using UniRx;

namespace A_Code.Systems.Experience
{
    public interface IExperienceSystem
    {
        ReactiveProperty<int> CurrentExperience { get; }
        ReactiveProperty<int> CurrentLevel { get; }

        int ExperienceForNextLevel { get; }
        
        Subject<int> OnLevelUp { get; }

        void AddExperience(int amount);
    }
}