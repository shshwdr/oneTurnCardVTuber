using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SpriteAnimator : MonoBehaviour
{
    public Image image;  // UI Image
    public Sprite[] sprites;  // 存储多个 Sprite（从 Sprite Mode = Multiple 中提取）

    public float frameRate = 0.1f;  // 每帧的显示时间
    public bool loop = true;  // 是否循环播放

    private void Start()
    {
        // 通过 DOTween 实现顺序播放
        PlayAnimation();
    }

    public void PlayAnimation()
    {
        // 创建一个序列，用于顺序播放所有 Sprite
        Sequence spriteSequence = DOTween.Sequence();

        foreach (Sprite sprite in sprites)
        {
            // 将每个 Sprite 动画添加到 Sequence 中
           // spriteSequence.Append(image.DOColor(new Color(1, 1, 1, 0), 0));  // 这里先设置透明，然后切换 Sprite
            spriteSequence.AppendCallback(() => image.sprite = sprite);  // 设置当前的 Sprite
            spriteSequence.Append(image.DOColor(new Color(1, 1, 1, 1), frameRate));  // 逐渐恢复不透明，模拟帧动画
        }

        // 如果需要循环播放
        if (loop)
        {
            spriteSequence.SetLoops(-1, LoopType.Restart);  // 无限循环
        }
        else
        {
            spriteSequence.SetLoops(1);  // 播放一次
        }

        // 启动动画
        spriteSequence.Play();
    }
}
