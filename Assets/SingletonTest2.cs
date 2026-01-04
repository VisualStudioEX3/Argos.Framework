using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework;

public class SingletonTest2 : MonoBehaviourSingleton<SingletonTest2>
{
    public void TestMethod()
    {
        print("Singleton test 2!");
    }
}
