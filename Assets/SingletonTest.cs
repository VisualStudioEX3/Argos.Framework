using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework;

public class SingletonTest : MonoBehaviourSingleton<SingletonTest>
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void TestMethod()
    {
        print("Singleton test!");
    }
}
