using UnityEngine;
using UnityEngine.Rendering;
#if GO4_CORE_APP
using Shared.Signals;
using Zenject;
#endif
namespace Games.Bingo
{
    public class GameStartHandler : MonoBehaviour
    {
        [SerializeField] private GameObject sceneRoot;
        [SerializeField] private RenderPipelineAsset renderPipelineAsset;
        private RenderPipelineAsset _defaultRenderPipeline;
#if GO4_CORE_APP
        [Inject] private SignalBus _signalBus;
        private void Awake()
        {
            sceneRoot.SetActive(false);
            _signalBus.Subscribe<OnGameStart>(StartGame);
            _defaultRenderPipeline = GraphicsSettings.currentRenderPipeline;
            GraphicsSettings.defaultRenderPipeline = renderPipelineAsset;
        }
        private void StartGame()
        {
            sceneRoot.SetActive(true);
            _signalBus.Unsubscribe<OnGameStart>(StartGame);
        }
#endif
        private void OnDestroy()
        {
            if (_defaultRenderPipeline != null)
                GraphicsSettings.defaultRenderPipeline = _defaultRenderPipeline;
        }
    }
}