using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Argos.Framework
{
    /// Source: https://gist.github.com/matheuslessarodrigues/13d08f49977a828b6565a76a2e8967e5
    /// 
    /// Searches through a target class in order to find all button attributes (<see cref="ButtonAttribute"/>).
    /// 
    /// Modification and extras:
    /// Adapted for admit custom labels (if the label is empty, uses method name) and tooltip messages (if the tooltip message is empty, not tooltip box showed).
    public class ButtonAttributeHelper
    {
        #region Static members
        static object[] emptyParamList = new object[0];
        #endregion

        #region Internal vars
        IList<MethodInfo> methods = new List<MethodInfo>();
        Object targetObject;
        #endregion

        #region Methods & Functions
        public void Init(Object targetObject)
        {
            this.targetObject = targetObject;
            methods =
                targetObject.GetType()
                    .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(m =>
                            m.GetCustomAttributes(typeof(ButtonAttribute), false).Length == 1 &&
                            m.GetParameters().Length == 0 &&
                            !m.ContainsGenericParameters
                    ).ToList();
        }

        public void DrawButtons()
        {
            if (methods.Count > 0)
            {
                ShowMethodButtons();
            }
        }

        void ShowMethodButtons()
        {
            foreach (MethodInfo method in methods)
            {
                var attribute = (ButtonAttribute)method.GetCustomAttributes(typeof(ButtonAttribute), false)[0];

                string buttonText = !string.IsNullOrEmpty(attribute.Label) ? attribute.Label : ObjectNames.NicifyVariableName(method.Name);
                string toolTip = attribute.TooltipMessage;

                if (string.IsNullOrEmpty(toolTip) ? GUILayout.Button(buttonText) : GUILayout.Button(new GUIContent(buttonText, toolTip)))
                {
                    method.Invoke(targetObject, emptyParamList);
                }
            }
        }
        #endregion
    }
}