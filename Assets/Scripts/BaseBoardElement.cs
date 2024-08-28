using System;
using Settings;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseBoardElement: MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private MeshRenderer focus; 
    [SerializeField] private MeshRenderer select;
    [SerializeField] private MeshRenderer defaultMesh;
    
    [SerializeField] private CellPaletteSettings cellPaletteSettings;

    public event Action<BaseBoardElement> OnPointerClickEvent;

    private ColorType _colorType;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        focus.enabled = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPointerClickEvent?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        focus.enabled = false;
    }

    public void SetColor(ColorType color)
    {
        _colorType = color;
        defaultMesh.sharedMaterial = _colorType switch
        {
            ColorType.Black => cellPaletteSettings.blackCell,
            ColorType.White => cellPaletteSettings.whiteCell,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void SetSelect(Material material)
    {
        select.enabled = true;
        select.sharedMaterial = material;
    }
    
    private void ResetSelect()
    {
        select.enabled = false;
    }
}