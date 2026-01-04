using Argos.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionListTest : MonoBehaviour
{
    [OptionList(true)]
    public PrimitiveType optionList;

    [OptionList(true)]
    public RuntimePlatform splitOptionList;
}
