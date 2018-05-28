using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework;
#if UNITY_EDITOR
using UnityEditor; 
#endif

namespace Argos.Framework.FileSystem
{
    [CreateAssetMenu(fileName = "Desktop File Slot", menuName = "Argos.Framework/File System/Desktop File Slot")]
    public class DesktopFileSlot : FileSlot
    {

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(DesktopFileSlot))]
    public class DesktopFileSlotEditor : FileSlotEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    } 
#endif
}