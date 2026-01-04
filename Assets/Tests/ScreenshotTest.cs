using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework;

public class ScreenshotTest : MonoBehaviour
{
    [TexturePreview]
    public Texture2D capture;
    [Button]
    public string takeScreenshot = "TakeScreenshot";
    [Button]
    public string saveToPng = "SaveToPNG";

    void TakeScreenshot()
    {
        this.TakeScreenshot(this.OnEndOfFrame);
    }

    void SaveToPNG()
    {
        string capturePath = $"{Application.dataPath}/capture.png";
        this.capture.SaveToPNGFile(capturePath);
        Application.OpenURL($"file://{capturePath}");
    }

    void OnEndOfFrame(Texture2D capture)
    {
        this.capture = capture;
    }
}
