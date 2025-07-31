using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using System;

public class EmojiFlyIn : Singleton<EmojiFlyIn>
{
    public RectTransform targetArea;
    public RectTransform emojiPrefab;

    private List<Action<RectTransform, CanvasGroup>> animationVariants;

    void Start()
    {
        animationVariants = new List<Action<RectTransform, CanvasGroup>>
        {
            FlyInPopShake,
            FloatUpFadeSpin,
            PopJumpFade,
            ShakeFlash,
            ArcFlyIn
        };
    }

    public void SpawnEmoji( Sprite sprite)
    {
        RectTransform emoji = Instantiate(emojiPrefab, transform);
        emoji.GetComponent<Image>().sprite = sprite;
        emoji.localScale = Vector3.zero;
        emoji.anchoredPosition = new Vector2(UnityEngine.Random.Range(-500, 500), -300);

        var cg = emoji.GetComponent<CanvasGroup>();
        if (cg == null) cg = emoji.gameObject.AddComponent<CanvasGroup>();
        cg.alpha = 0;

        // Pick a random animation
        var anim = animationVariants[UnityEngine.Random.Range(0, animationVariants.Count)];
        anim.Invoke(emoji, cg);
    }

    private Vector2 GetRandomPositionInRect(RectTransform rect)
    {
        Vector2 size = rect.rect.size;
        return (Vector2)rect.localPosition + new Vector2(
            UnityEngine.Random.Range(-size.x / 2, size.x / 2),
            UnityEngine.Random.Range(-size.y / 2, size.y / 2)
        );
    }

    // 🎯 Fly in → pop → shake → fade
    void FlyInPopShake(RectTransform emoji, CanvasGroup cg)
    {
        Vector2 target = GetRandomPositionInRect(targetArea);

        Sequence s = DOTween.Sequence();
        s.Append(cg.DOFade(1, 0.2f));
        s.Join(emoji.DOScale(1.2f, 0.3f).SetEase(Ease.OutBack));
        s.Join(emoji.DOAnchorPos(target, 0.6f).SetEase(Ease.OutCubic));
        s.Append(emoji.DOScale(1f, 0.2f));
        s.Append(emoji.DOShakeScale(0.5f, 0.2f, 10, 90));
        s.Append(cg.DOFade(0f, 0.5f));
        s.Join(emoji.DOScale(0.2f, 0.5f));
        s.AppendCallback(() => Destroy(emoji.gameObject));
    }

    // 💫 Float up slowly with spin and fade
    void FloatUpFadeSpin(RectTransform emoji, CanvasGroup cg)
    {
        Vector2 start = GetRandomPositionInRect(targetArea);
        emoji.anchoredPosition = start;

        cg.alpha = 0;
        emoji.localScale = Vector3.one;

        Sequence s = DOTween.Sequence();
        s.Append(cg.DOFade(1, 0.2f));
        s.Join(emoji.DOLocalRotate(new Vector3(0, 0, 360f), 1.5f, RotateMode.FastBeyond360));
        s.Join(emoji.DOAnchorPos(start + new Vector2(0, 100), 1.5f).SetEase(Ease.OutSine));
        s.Append(cg.DOFade(0, 0.5f));
        s.AppendCallback(() => Destroy(emoji.gameObject));
    }

    // 💥 Pop in + jump + fade
    void PopJumpFade(RectTransform emoji, CanvasGroup cg)
    {
        Vector2 target = GetRandomPositionInRect(targetArea);

        Sequence s = DOTween.Sequence();
        s.Append(cg.DOFade(1, 0.1f));
        s.Join(emoji.DOScale(1.5f, 0.2f).SetEase(Ease.OutBack));
        s.Join(emoji.DOAnchorPos(target, 0.4f).SetEase(Ease.OutQuart));
        s.Append(emoji.DOAnchorPosY(target.y + 30, 0.2f).SetLoops(2, LoopType.Yoyo));
        s.Append(cg.DOFade(0, 0.4f));
        s.Join(emoji.DOScale(0f, 0.4f));
        s.AppendCallback(() => Destroy(emoji.gameObject));
    }

    // 🌈 Flash + shake
    void ShakeFlash(RectTransform emoji, CanvasGroup cg)
    {
        Vector2 target = GetRandomPositionInRect(targetArea);
        emoji.anchoredPosition = target;
        emoji.localScale = Vector3.one;

        Sequence s = DOTween.Sequence();
        s.Append(cg.DOFade(1, 0.1f));
        s.Append(emoji.DOShakePosition(0.4f, 10f, 20, 90));
        s.Append(emoji.DOShakeRotation(0.4f, 15f));
        s.Append(cg.DOFade(0, 0.5f));
        s.AppendCallback(() => Destroy(emoji.gameObject));
    }

    // 🚀 Arc/Bezier-style fly-in
    void ArcFlyIn(RectTransform emoji, CanvasGroup cg)
    {
        Vector2 start = emoji.anchoredPosition;
        Vector2 end = GetRandomPositionInRect(targetArea);
        Vector2 control = (start + end) / 2 + new Vector2(0, 150); // 控制点，决定弧度

        float t = 0f;
        cg.alpha = 1f;
        emoji.localScale = Vector3.one;

        DOTween.To(() => t, x => {
            t = x;
            emoji.anchoredPosition = Bezier(start, control, end, t);
        }, 1f, 1f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            emoji.GetComponent<Image>().DOFade(0f, 0.5f).OnComplete(() => Destroy(emoji.gameObject));
        });
    }

    // 简单二阶贝塞尔插值
    Vector2 Bezier(Vector2 a, Vector2 b, Vector2 c, float t)
    {
        Vector2 ab = Vector2.Lerp(a, b, t);
        Vector2 bc = Vector2.Lerp(b, c, t);
        return Vector2.Lerp(ab, bc, t);
    }
}
