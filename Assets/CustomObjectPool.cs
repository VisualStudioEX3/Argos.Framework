using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework;

public class CustomObjectPool : ObjectPool<RotateObject>
{
    [Space, Button("Create new instance", GUIButtonSize.Normal, GUIButtonDisableEvents.EditorMode)]
    public string button = "CreateNewObject";

    [DynamicLabel]
    public string label;

    void CreateNewObject()
    {
        this.GetNewInstance(5f).transform.position = Random.insideUnitSphere * Random.Range(1f, 5f);
    }

    private void Update()
    {
        this.label = $"Total: {this.Total}, actives: {this.Actives}, availables: {this.Availables}";
    }
}
