using System;
using Board;
using JetBrains.Annotations;
using Settings;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Units
{
    public abstract class BaseElement: MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [SerializeField] protected MeshRenderer defaultMesh;
        [SerializeField] protected CellPaletteSettings cellPaletteSettings;
        
        public event Action<BaseElement> OnPointerClickEvent;

        [CanBeNull] public BaseElement Pair { get; private set; }
        public void SetNewPair(BaseElement pair)
        {
            if (Pair is not null)
            {
                Pair.Pair = null;
            }

            Pair = pair;
        }

        private ColorType? _color;
        public ColorType GetColor => _color ?? throw new ArgumentNullException("Color is not assigned!");
        public void SetColor(ColorType color)
        {
            _color = color;
            SetDefaultColor();
        }

        protected void SetDefaultColor()
        {
            if (_color is null) return;
            
            defaultMesh.sharedMaterial = _color switch
            {
                ColorType.Black => cellPaletteSettings.blackCell,
                ColorType.White => cellPaletteSettings.whiteCell,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public bool IsHighlighted { get; private set; }
        
        public void SetHighlighted(bool selected)
        {
            IsHighlighted = selected;
            SetFocused(selected);
        }

        public abstract void SetFocused(bool focus);
        
        public event FocusEventHandler OnFocusEventHandler;
        protected void UpdateFocus(Cell target, bool isSelect) => OnFocusEventHandler?.Invoke(target, isSelect);
        
        public abstract void OnPointerEnter(PointerEventData eventData);
        public abstract void OnPointerExit(PointerEventData eventData);

        public void OnPointerClick(PointerEventData eventData) => OnPointerClickEvent?.Invoke(this);
    }
    
    public delegate void FocusEventHandler(Cell cell, bool isSelect);
}