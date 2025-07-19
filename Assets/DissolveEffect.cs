using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GroupDissolveEffect : MonoBehaviour
{
    [Tooltip("溶解过渡时长")]
    public float duration = 2f;

    [Tooltip("目标溶解值，1表示完全彩色")]
    public float targetDissolve = 1f;

    [Tooltip("边缘柔和度")]
    public float edgeSoft = 0.1f;

    [Tooltip("噪声纹理")]
    public Texture2D noiseTexture;

    public Image[] spriteRenderers;
    

    void Start()
    {
        // 获取所有子物体的 SpriteRenderer
        //spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        
        // 为每个 SpriteRenderer 分配新的材质实例，并使用自定义 Shader
        foreach (var sr in spriteRenderers)
        {
            Material newMat = new Material(Shader.Find("Custom/DissolveGreyscaleToColor"));
            // 复制原有纹理
            newMat.SetTexture("_MainTex", sr.material.GetTexture("_MainTex"));
            newMat.SetTexture("_NoiseTex", noiseTexture);
            newMat.SetFloat("_Dissolve", 1); // 初始全灰度
            newMat.SetFloat("_EdgeSoft", edgeSoft);
            sr.material = newMat;
        }
    }

    
    void Update()
    {
        // 按下空格键启动颜色过渡效果
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartDissolveEffect();
        }
    }
    // 调用此方法启动溶解过渡效果
    public void StartDissolveEffect()
    {
        foreach (var sr in spriteRenderers)
        {
            // 利用 DOTween 动画逐渐将 _Dissolve 参数从 0 增加到 targetDissolve
            sr.material.DOFloat(targetDissolve, "_Dissolve", duration);
        }
    }
}