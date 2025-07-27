
using UnityEditor;
using UnityEngine;

public class Screenshot: MonoBehaviour
{
    [MenuItem("Tools/截图")]
    public static void ScreenShot()
    {
        ScreenCapture.CaptureScreenshot("Assets/Editor/截图.png");
    }

}