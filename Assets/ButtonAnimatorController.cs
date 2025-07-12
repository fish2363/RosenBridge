using UnityEngine;
using DG.Tweening;

public class ButtonAnimatorController : MonoBehaviour
{
    public RectTransform[] buttons; // 0: ��, 1: ���, 2: �Ʒ�

    private Vector2[] originalSizes;
    private Vector2[] originalPositions;

    [Header("ũ�� ����")]
    public float expandedHeight = 150f;    // Ŀ�� ��ư ����
    public float normalHeight = 120f;      // ���� ����
    public float squishedHeight = 100f;    // ���� ��ư ����

    [Header("�ִϸ��̼� ����")]
    public float moveRatio = 0.25f;        // ��ġ �̵� ���� (0.25�̸� 25%)
    public float duration = 0.2f;

    private void Start()
    {
        int count = buttons.Length;
        originalSizes = new Vector2[count];
        originalPositions = new Vector2[count];

        for (int i = 0; i < count; i++)
        {
            originalSizes[i] = buttons[i].sizeDelta;
            originalPositions[i] = buttons[i].anchoredPosition;
        }
    }

    public void OnButtonHover(int hoveredIndex)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            RectTransform rect = buttons[i];

            float newHeight;
            Vector2 newPos = originalPositions[i];

            if (i == hoveredIndex)
            {
                newHeight = expandedHeight;
            }
            else
            {
                newHeight = squishedHeight;

                float offset = (expandedHeight - normalHeight) * moveRatio;
                if (i < hoveredIndex)
                    newPos.y += offset;
                else if (i > hoveredIndex)
                    newPos.y -= offset;
            }

            Vector2 newSize = new(rect.sizeDelta.x, newHeight);

            rect.DOSizeDelta(newSize, duration).SetEase(Ease.OutCubic);
            rect.DOAnchorPos(newPos, duration).SetEase(Ease.OutCubic);
        }
    }

    public void OnButtonExit()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].DOSizeDelta(originalSizes[i], duration).SetEase(Ease.OutCubic);
            buttons[i].DOAnchorPos(originalPositions[i], duration).SetEase(Ease.OutCubic);
        }
    }
}