using UnityEditor;
using UnityEngine;

namespace Argos.Framework
{
    /// Author: (Twitter) @matheuslrod
    /// Source: https://gist.github.com/matheuslessarodrigues/13d08f49977a828b6565a76a2e8967e5
    /// 
    /// <summary>
    /// Custom editor for all MonoBehaviour scripts in order to draw buttons for all button attributes (<see cref="ButtonAttribute"/>).
    /// </summary>
    [CustomEditor(typeof(MonoBehaviour), true, isFallback = true)]
    [CanEditMultipleObjects]
    public class BehaviourButtonsEditor : Editor
    {
        private ButtonAttributeHelper helper = new ButtonAttributeHelper();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            helper.DrawButtons();
        }

        private void OnEnable()
        {
            helper.Init(target);
        }
    }
}