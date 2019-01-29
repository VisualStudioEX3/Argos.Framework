using Argos.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionListTest : MonoBehaviour
{
    [OptionList(true)]
    public PrimitiveType OptionList;

    [OptionList(true)]
    public RuntimePlatform SplitOptionList;
}
