using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework;

public class TransformLerpTest : MonoBehaviour
{
    public Transform a;
    public Transform b;

    public Transform player;

    [Range(0f, 1f)]
    public float time;

    void Update()
    {
        player.Lerp(a, b, time);
    }
}
