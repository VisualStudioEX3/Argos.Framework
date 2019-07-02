using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework;

public class CustomObjectPool : ObjectPool<RotateObject>
{
    [Button("Create new instance", GUIButtonSize.Normal, GUIButtonDisableEvents.EditorMode)]
    public string button = "CreateNewObject";

    [DynamicLabel]
    public string label;

    void CreateNewObject()
    {
        this.GetNewInstance();
    }

    private void Update()
    {
        this.label = $"Total: {this.Total}, actives: {this.Actives}, availables: {this.Availables}";
    }

    public override void OnNewInstance(RotateObject instance)
    {
        instance.transform.position = Random.insideUnitSphere * Random.Range(1, 5);
    }

    public override IEnumerator TerminateInstance(RotateObject instance)
    {
        yield return new WaitForSeconds(5f);
        instance.gameObject.SetActive(false);
    }
}
