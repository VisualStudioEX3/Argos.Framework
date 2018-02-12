using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    /// <summary>
    /// Custom drawer to rename vectorN fields
    /// </summary>
    /// <remarks>
    /// Code inspiration from https://gamedev.stackexchange.com/questions/122301/how-can-i-create-a-custom-propertydrawer-for-my-point-struct/123609
    /// </remarks>
    [CustomPropertyDrawer(typeof(VectorRenameAttribute))]
    public class VectorRenameDrawer : PropertyDrawer
    {
        List<SerializedProperty> Props;
        VectorRenameAttribute MyAttribute;
        string name;
        bool cache = false;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!cache)
            {
                //get attribute for new names
                MyAttribute = this.attribute as VectorRenameAttribute;
                Props = new List<SerializedProperty>();

                //get the name before it's gone
                name = property.displayName;

                //get property values
                while (property.Next(true) && (Props.Count < MyAttribute.Names.Length))
                {
                    Props.Add(property.Copy());
                } //endwhile

                cache = true;
            } //endif

            Rect contentPosition = EditorGUI.PrefixLabel(position, new GUIContent(name));

            //Check if there is enough space to put the name on the same line (to save space)
            if (position.height > EditorGUIUtility.singleLineHeight)
            {
                position.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.indentLevel += 1;
                contentPosition = EditorGUI.IndentedRect(position);
                contentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            } //endif

            // Divide the widht in proportional sections
            float step = contentPosition.width / Props.Count;
            GUI.skin.label.padding = new RectOffset(3, 3, 6, 6);

            //show the X and Y from the point
            contentPosition.width /= Props.Count;
            EditorGUI.indentLevel = 0;

            for (int i = 0; i < Props.Count; i++)
            {
                var P = Props[i];

                // Begin/end property & change check make each field
                // behave correctly when multi-object editing.
                EditorGUI.BeginProperty(contentPosition, label, P);
                {
                    // Take label from attribute
                    var propLabel = new GUIContent(MyAttribute.Names[i]);

                    // Compute label width (max 70% of content rect)
                    float w1, w2;
                    GUI.skin.label.CalcMinMaxWidth(propLabel, out w1, out w2);
                    EditorGUIUtility.labelWidth = Mathf.Min(w1, step * 0.7f);

                    // Edit property
                    EditorGUI.BeginChangeCheck();
                    float newVal = EditorGUI.FloatField(contentPosition, propLabel, P.floatValue);
                    if (EditorGUI.EndChangeCheck())
                        P.floatValue = newVal;
                }
                EditorGUI.EndProperty();

                // Move rect to the right
                contentPosition.x += step;
            } //endfor
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float rv = EditorGUIUtility.singleLineHeight;
            if (!EditorGUIUtility.wideMode)
                rv = rv * 2 + EditorGUIUtility.standardVerticalSpacing;
            return rv;
        }
    } 
}
