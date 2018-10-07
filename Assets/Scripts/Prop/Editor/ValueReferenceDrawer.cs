using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomPropertyDrawer(typeof(ValueReference))]
public class ValueReferenceDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 1;

        var referenceType= property.FindPropertyRelative("ReferenceType");
        // Calculate rects
        var typeRect = new Rect(position.x, position.y,80, position.height);
        var valueRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);
        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(typeRect, referenceType , GUIContent.none);
        if(referenceType.enumValueIndex == 0)
            EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("Value"), GUIContent.none);
        else
            EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("Amount"), GUIContent.none);
        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

}
