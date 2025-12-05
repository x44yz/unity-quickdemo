using UnityEngine;
using UnityEditor;
using System.Reflection;
using Codice.CM.Common;

namespace NaughtyAttributes.Editor
{
    [CustomPropertyDrawer(typeof(SerializableNameAttribute))]
    public class SerializableNamePropertyDrawer : PropertyDrawerBase
    {
        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            SerializableNameAttribute snameAttribute = PropertyUtility.GetAttribute<SerializableNameAttribute>(property);
            if (snameAttribute != null)
            {
                var conditionProperty = property.serializedObject.FindProperty(property.propertyPath + "." + snameAttribute.Condition);
                if (conditionProperty != null)
                {
                    label.text = label.text + "-" + PropertyUtility.GetSerializedPropertyValueAsString(conditionProperty);
                }
                else
                {
                    object target = PropertyUtility.GetTargetObjectWithProperty(property);
                    FieldInfo conditionField = ReflectionUtility.GetField(target, snameAttribute.Condition);
                    if (conditionField != null &&
                        conditionField.FieldType == typeof(string))
                    {
                        label.text = label.text + "-" + (string)conditionField.GetValue(target);
                    }
                }
            }
            EditorGUI.PropertyField(rect, property, label, true);
            EditorGUI.EndProperty();
        }
    }
}
