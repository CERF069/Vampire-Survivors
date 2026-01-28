using A_Code.Attack.Type.OrbitingAttack;
using A_Code.Attack.Type.ProjectileAttack;
using A_Code.Player.Data;
using A_Code.Player.Input;
using A_Code.Player.Interface;
using A_Code.Systems.ChoiceAbility;
using A_Code.Systems.LoadConfig;
using UnityEngine;
using Zenject;

namespace A_Code.Player
{
    public sealed class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private OrbitAttackSystem _orbitAttackSystem;
        [SerializeField] private ProjectileAttackSystem _projectileSystem;
        [SerializeField] private LevelUpCardUI _levelUpCardUI;

        public override void InstallBindings()
        {
            Container.Bind<LevelUpCardUI>().FromInstance(_levelUpCardUI).AsSingle().Lazy();
            
            Container.Bind<OrbitAttackSystem>().FromInstance(_orbitAttackSystem).AsSingle();
            Container.Bind<ProjectileAttackSystem>().FromInstance(_projectileSystem).AsSingle();
            
            
            Container.BindInterfacesAndSelfTo<PlayerMoveConfig>()
                .FromMethod(ctx =>
                    ctx.Container.Resolve<ConfigStorage>()
                        .Get<PlayerMoveConfig>())
                .AsSingle();

            Container.Bind<IPlayerInput>()
                .To<PlayerInputSystem>()
                .AsSingle();

           
            
          

            Container.Bind<PlayerAttackConfig>()
                .FromMethod(ctx => ctx.Container.Resolve<ConfigStorage>().Get<PlayerAttackConfig>())
                .AsSingle();
            
            
                
            Container.Bind<PlayerHealthConfig>()
                .FromMethod(ctx => ctx.Container.Resolve<ConfigStorage>().Get<PlayerHealthConfig>())
                .AsSingle();
            
          
        }
    }
}