using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private MeshRenderer _focus; 
    [SerializeField] private MeshRenderer _select;

    public event Action<Cell> OnPointerClickEvent;

    public Unit CurrentUnit;

    private Dictionary<Primitives.NeighbourType, Cell> _neighbours;

    public void Init(Dictionary<Primitives.NeighbourType, Cell> neighbours)
    {
        _neighbours = neighbours;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _focus.enabled = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPointerClickEvent?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _focus.enabled = false;
    }

    private void SetSelect(Material material)
    {
        _select.enabled = true;
        _select.sharedMaterial = material;
    }
    
    private void ResetSelect()
    {
        _select.enabled = false;
    }
}
