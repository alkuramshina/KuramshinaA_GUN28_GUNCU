using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private MeshRenderer _focus; 
    [SerializeField] private MeshRenderer _select;

    private Action<Cell> OnPointerClickEvent;
    
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
        _select.material = material;
    }
    
    private void ResetSelect()
    {
        _select.enabled = false;
    }
}
