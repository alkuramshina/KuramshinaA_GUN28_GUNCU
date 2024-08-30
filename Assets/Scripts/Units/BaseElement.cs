using System;
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
        public virtual void SetColor(ColorType color)
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

        public bool IsSelected { get; protected set; }

        protected abstract void SetFocused(bool focused);
        public abstract void SetSelected(bool selected);
        
        public void OnPointerEnter(PointerEventData eventData) => SetFocused(true);
        public void OnPointerExit(PointerEventData eventData) => SetFocused(false);
        public void OnPointerClick(PointerEventData eventData) => OnPointerClickEvent?.Invoke(this);
    }
}