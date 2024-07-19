using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    private Controls _controls;
    private CellPaletteSettings _cellPaletteSettings;
    
    public override void InstallBindings()
    {
        _controls = new Controls();
        _controls.Game.Enable();

        Container.BindInstance(_controls.Game).AsSingle();
        Container.BindInstance(_cellPaletteSettings).AsSingle();
    }
}