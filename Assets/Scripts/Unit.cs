using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    private Cell _currentCell;
    public event Action OnMoveEndCallback;

    public void OnPointerEnter(PointerEventData eventData) => _currentCell.OnPointerEnter(eventData);
    public void OnPointerClick(PointerEventData eventData) => _currentCell.OnPointerClick(eventData);
    public void OnPointerExit(PointerEventData eventData) => _currentCell.OnPointerExit(eventData);

    private void Move(Cell to)
    {
        // TODO: move
        
        _currentCell = to;
        to.CurrentUnit = this;
        
        OnMoveEndCallback?.Invoke();
    }
}
