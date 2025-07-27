using UnityEngine;
using UnityEditor;
using System.IO;

public class BatchChangeMaterialShader : EditorWindow
{
    private string folderPath = "Assets/Resources/characterSpine"; // 修改为你要处理的文件夹路径
    private string targetShaderName = "Spine/SkeletonTint"; // 目标 Shader 名称

    [MenuItem("Tools/批量修改材质Shader为SkeletonTint")]
    static void ShowWindow()
    {
        GetWindow<BatchChangeMaterialShader>("批量修改材质Shader");
    }

    private void OnGUI()
    {
        GUILayout.Label("批量修改材质Shader为SkeletonTint", EditorStyles.boldLabel);
        folderPath = EditorGUILayout.TextField("文件夹路径", folderPath);
        targetShaderName = EditorGUILayout.TextField("目标 Shader 名称", targetShaderName);

        if (GUILayout.Button("开始修改"))
        {
            ChangeMaterialsInFolder(folderPath, targetShaderName);
        }
    }

    private void ChangeMaterialsInFolder(string path, string shaderName)
    {
        // 获取指定文件夹下所有材质资源GUID
        string[] guids = AssetDatabase.FindAssets("t:Material", new string[] { path });
        int count = 0;
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
            if (mat != null)
            {
                // 如果当前材质的 Shader 名称不等于目标名称，则修改
                if (mat.shader == null || mat.shader.name != shaderName)
                {
                    Shader targetShader = Shader.Find(shaderName);
                    if (targetShader != null)
                    {
                        mat.shader = targetShader;
                        EditorUtility.SetDirty(mat);
                        Debug.Log($"修改材质: {assetPath} 使用 Shader: {shaderName}");
                        count++;
                    }
                    else
                    {
                        Debug.LogError("找不到目标Shader: " + shaderName);
                        return;
                    }
                }
            }
        }
        AssetDatabase.SaveAssets();
        Debug.Log($"完成修改，共修改 {count} 个材质。");
    }
}
