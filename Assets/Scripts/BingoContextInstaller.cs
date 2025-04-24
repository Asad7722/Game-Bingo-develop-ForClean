using Zenject;

namespace Games.Bingo
{
    public class BingoContextInstaller : MonoInstaller<BingoContextInstaller>
    {
        public override void InstallBindings()
        {
#if !GO4_CORE_APP
            SignalBusInstaller.Install(Container);
#endif
            BindInterfaces();
        }

        private void BindInterfaces()
        {
#if GO4_CORE_APP
            Container.Bind<IBeamableAPIProvider>().To<BeamableAPIProvider>().AsSingle().NonLazy();
#endif
        }
    }
}