using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Board
{
    public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [SerializeField] private MeshRenderer _focus; 
        [SerializeField] private MeshRenderer _select;

        public event Action<Cell> OnPointerClickEvent;

        public Unit CurrentUnit;

        private Dictionary<Primitives.NeighbourType, Cell> _neighbours;
        private Primitives.ColorType _colorType;
        private MeshRenderer _defaultMesh;

        [SerializeField] private CellPaletteSettings _cellPaletteSettings;

        private void Awake()
        {
            _defaultMesh = GetComponent<MeshRenderer>();
        }

        public void Set(Dictionary<Primitives.NeighbourType, Cell> neighbours)
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

        public void SetColor(Primitives.ColorType color)
        {
            _colorType = color;
            _defaultMesh.sharedMaterial = _colorType switch
            {
                Primitives.ColorType.Black => _cellPaletteSettings.blackCell,
                Primitives.ColorType.White => _cellPaletteSettings.whiteCell,
                _ => throw new ArgumentOutOfRangeException()
            };
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
}
