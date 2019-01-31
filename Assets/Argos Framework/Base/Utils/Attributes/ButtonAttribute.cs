using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAttribute : PropertyAttribute
{
    public readonly string MethodName;

    public ButtonAttribute(string methodName)
    {
        this.MethodName = methodName;
    }
}
