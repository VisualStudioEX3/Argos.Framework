using UnityEditor;
using UnityEngine;

namespace Argos.Framework
{
    /// Source: https://gist.github.com/matheuslessarodrigues/13d08f49977a828b6565a76a2e8967e5
    [CustomEditor(typeof(MonoBehaviour), true, isFallback = true), CanEditMultipleObjects]
    public class BehaviourButtonsEditor : Editor
    {
        #region Internal vars
        ButtonAttributeHelper helper = new ButtonAttributeHelper();
        #endregion

        #region Events
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            helper.DrawButtons();
        }

        private void OnEnable()
        {
            helper.Init(target);
        } 
        #endregion
    }
}