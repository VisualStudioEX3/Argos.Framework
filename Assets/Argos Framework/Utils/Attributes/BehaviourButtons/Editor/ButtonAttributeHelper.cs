using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Argos.Framework
{
    /// Author: (Twitter) @matheuslrod
    /// Source: https://gist.github.com/matheuslessarodrigues/13d08f49977a828b6565a76a2e8967e5
    /// 
    /// Searches through a target class in order to find all button attributes (<see cref="ButtonAttribute"/>).
    /// 
    /// Modification:
    /// Author: José Miguel Sánchez Fernandez (Twitter) @ex3_tlsa
    /// 
    /// Adapted for admit custom labels (if the label is empty, uses method name) and tooltip messages (if the tooltip message is empty, not tooltip box showed).
    public class ButtonAttributeHelper
    {
        private static object[] emptyParamList = new object[0];

        private IList<MethodInfo> methods = new List<MethodInfo>();
        private Object targetObject;

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

        private void ShowMethodButtons()
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
    }
}