using Board;
using Controllers;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class SceneInstaller : MonoInstaller
    {
        private Controls _controls;
        
        [SerializeField] private Battlefield boardGenerator;
        [SerializeField] private CameraMover cameraMover;
        [SerializeField] private RoundCounter roundCounter;
        
        public override void InstallBindings()
        {
            _controls = new Controls();
            _controls.Game.Enable();
            
            Container.BindInstance(_controls.Game).AsSingle();
            Container.BindInstance(boardGenerator).AsSingle();
            Container.BindInstance(cameraMover).AsSingle();
            Container.BindInstance(roundCounter).AsSingle();
            Container.Bind<PlayerController>().AsSingle();
        }
    }
}