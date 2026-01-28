using UniRx;


namespace A_Code.Systems.Experience
{
    public class ExperienceSystem : IExperienceSystem
    {
        public ReactiveProperty<int> CurrentExperience { get; }
        public ReactiveProperty<int> CurrentLevel { get; }

        public int ExperienceForNextLevel => 100;

        public Subject<int> OnLevelUp { get; } = new Subject<int>();

        public ExperienceSystem()
        {
            CurrentLevel = new ReactiveProperty<int>(1);
            CurrentExperience = new ReactiveProperty<int>(0);
        }

        public void AddExperience(int amount)
        {
            CurrentExperience.Value += amount;

            while (CurrentExperience.Value >= ExperienceForNextLevel)
            {
                CurrentExperience.Value -= ExperienceForNextLevel;
                CurrentLevel.Value++;
                
                OnLevelUp.OnNext(CurrentLevel.Value);
            }
        }
    }
}