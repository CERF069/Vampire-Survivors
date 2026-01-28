using A_Code.Enemies.Data;
using A_Code.Enemies.Data.Fast;
using A_Code.Enemies.Data.Normal;
using A_Code.Enemies.Interface;
using A_Code.Enemies.MoveDirection;
using A_Code.Enemies.Spawner;
using A_Code.Systems.LoadConfig;
using UnityEngine;
using Zenject;

namespace A_Code.Enemies
{
    public sealed class EnemyInstaller : MonoInstaller
    {
        [SerializeField] private Transform _player;

        public override void InstallBindings()
        {
            Container.Bind<IEnemyTargetProvider>()
                .To<FollowPlayerDirection>()
                .AsSingle()
                .WithArguments(_player);

            Container.Bind<NormalEnemyMoveConfig>()
                .FromMethod(ctx => ctx.Container.Resolve<ConfigStorage>().Get<NormalEnemyMoveConfig>())
                .AsSingle();

            Container.Bind<FastEnemyMoveConfig>()
                .FromMethod(ctx => ctx.Container.Resolve<ConfigStorage>().Get<FastEnemyMoveConfig>())
                .AsSingle();

            Container.Bind<EnemySpawnConfig>()
                .FromMethod(ctx => ctx.Container.Resolve<ConfigStorage>().Get<EnemySpawnConfig>())
                .AsSingle();
            
            Container.Bind<FastEnemyHealthConfig>()
                .FromMethod(ctx => ctx.Container.Resolve<ConfigStorage>().Get<FastEnemyHealthConfig>())
                .AsSingle();
            
            Container.Bind<EnemyHealthConfig>()
                .FromMethod(ctx => ctx.Container.Resolve<ConfigStorage>().Get<EnemyHealthConfig>())
                .AsSingle();
            
            
            
            
            Container.Bind<FastEnemyAttackConfig>()
                .FromMethod(ctx => ctx.Container.Resolve<ConfigStorage>().Get<FastEnemyAttackConfig>())
                .AsSingle();
            
            Container.Bind<EnemyAttackConfig>()
                .FromMethod(ctx => ctx.Container.Resolve<ConfigStorage>().Get<EnemyAttackConfig>())
                .AsSingle();
            
            
            

            Container.Bind<EnemyPool>().AsSingle();
            Container.Bind<EnemySpawner>()
                .FromComponentInHierarchy()
                .AsSingle();
        }
    }
}