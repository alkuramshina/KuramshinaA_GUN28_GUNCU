using Board;
using Settings;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class SceneInstaller : MonoInstaller
    {
        private Controls _controls;

        [SerializeField] private CellManager cellManager;
        [SerializeField] private CellPaletteSettings cellPaletteSettings;
    
        public override void InstallBindings()
        {
            _controls = new Controls();
            _controls.Game.Enable();

            Container.BindInstance(_controls.Game).AsSingle();
            Container.BindInstance(cellPaletteSettings).AsSingle();
        }
    }
}