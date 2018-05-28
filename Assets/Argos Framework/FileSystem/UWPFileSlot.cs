using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Argos.Framework.FileSystem
{
    [CreateAssetMenu(fileName = "Universal Windows Platform File Slot", menuName = "Argos.Framework/File System/Universal Windows Platform File Slot")]
    public class UWPFileSlot : FileSlot
    {

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(UWPFileSlot))]
    public class UWPFileSlotEditor : FileSlotEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
#endif
}