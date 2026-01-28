using A_Code.Systems.LoadConfig;
using Zenject;

namespace A_Code
{
    public sealed class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        { 
            Container.Bind<ConfigStorage>()
                .AsSingle()
                .NonLazy();


            Container.BindInterfacesTo<ConfigBootstrap>()
                .AsSingle()
                .NonLazy();
        }
    }
}