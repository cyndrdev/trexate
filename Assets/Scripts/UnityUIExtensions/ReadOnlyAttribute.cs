using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ReadOnlyAttribute : PropertyAttribute
{
}


#if UNITY_EDITOR
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
            case SerializedPropertyType.Boolean:
                value = property.boolValue ? "true" : "false";
                break;
            default:
                value = "[type not supported]";
                break;
        }

        EditorGUI.LabelField(position, label.text, value);
    }
}
#endif