#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class ReadOnlyAttribute : PropertyAttribute
{
}

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        => EditorGUI.GetPropertyHeight(property, label, true);

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        string value;

        switch(property.propertyType)
        {
            case SerializedPropertyType.Integer:
                value = property.intValue.ToString();
                break;
            case SerializedPropertyType.Float:
                value = property.floatValue.ToString();
                break;
            case SerializedPropertyType.String:
                value = property.stringValue;
                break;
            default:
                value = "[type not supported]";
                break;
        }

        EditorGUI.LabelField(position, label.text, value);
    }
}
#endif