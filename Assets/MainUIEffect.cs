using DG.Tweening;
using UnityEngine;

public class MainUIEffect : MonoBehaviour
{
    private bool isShaking;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void ShakeLeftRightUI()
    {
        rectTransform.DOShakeRotation(0.3f, 1f, 10, 90f).OnComplete(() => isShaking = false);
    }
    public void ShakeUpDownUI()
    {
        rectTransform.DOShakeAnchorPos(
            0.3f,
            new Vector2(0, 10f), // Y축으로만 흔들림
            10,
            90f,
            false,
            true
        ).OnComplete(() => isShaking = false);
    }
}
