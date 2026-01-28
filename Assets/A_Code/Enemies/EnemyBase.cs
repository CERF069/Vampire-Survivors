using A_Code.Enemies.Interface;
using A_Code.Systems.Experience;
using UnityEngine;
using Zenject;

namespace A_Code.Enemies
{
    public abstract class EnemyBase : MonoBehaviour, IEnemyPooled
    {
        protected IExperienceSystem _experienceSystem;

        [Inject]
        public void Construct(IExperienceSystem experienceSystem)
        {
            _experienceSystem = experienceSystem;
        }

        public virtual void OnSpawn(Vector2 position)
        {
            transform.position = position;
            gameObject.SetActive(true);
        }

        public virtual void OnDespawn()
        {
            gameObject.SetActive(false);
        }

        protected virtual void OnDie()
        {
            _experienceSystem.AddExperience(100);
            OnDespawn();
        }
    }
}