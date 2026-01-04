using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GetAngleTest : MonoBehaviour
{
    public Transform a;
    public Transform b;

    public float angle;
    public float unityAngle;

    // Update is called once per frame
    void Update()
    {
        this.angle = Argos.Framework.Utils.MathUtility.GetAngle(this.a.position, this.b.position);//, this.a.forward);
        //this.unityAngle = Vector3.Angle(a.forward, b.position - a.position);
        //this.unityAngle = Vector3.SignedAngle(this.a.position, this.b.position, Vector3.forward);
        this.unityAngle = Vector2.Angle(this.a.position, this.b.position);
    }
}
