using Board;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
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