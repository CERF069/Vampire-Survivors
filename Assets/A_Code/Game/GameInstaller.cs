using Zenject;
using A_Code.Player.Interface;
using A_Code.Player.Move;
using A_Code.Systems.Experience;

namespace A_Code.Game
{
    public sealed class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        { 
       
            Container
                .Bind<IExperienceSystem>()
                .To<ExperienceSystem>()
                .AsSingle();
            
            Container.Bind<IMoveble>()
                .To<MovebleSystem>()
                .AsTransient();
        }
    }
}