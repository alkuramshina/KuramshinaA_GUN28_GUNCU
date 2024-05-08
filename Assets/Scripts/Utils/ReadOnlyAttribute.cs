using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace.Utils
{
    public class ReadOnlyAttribute : PropertyAttribute
    {
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyPropertyDrawer: PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var element = base.CreatePropertyGUI(property) ?? new UnityEditor.UIElements.PropertyField(property);
            
            element.SetEnabled(false);

            return element;
        }
    }
#endif
}