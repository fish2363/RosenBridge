using UnityEngine;
using System;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using DG.Tweening;

public class TutorialLogic : MonoBehaviour
{
    [Header("대사 입력하기 \\n을 입력하면 띄어쓰기")]
    public Dialogue[] tutorialDialogue;
    int idx;
    private bool isMouse;
    private bool isOn;

    [SerializeField]
    private Image[] tutorialImage;

    [SerializeField]
    private TextMeshProUGUI _text;
    [SerializeField]
    private Image textPanel;

    public Volume volume;
    private ColorAdjustments colorAdjustments;

    void Start()
    {
        if (!volume.profile.TryGet(out colorAdjustments))
        {
            Debug.LogError("Lens Distortion not found in Volume Profile!");
        }
    }

    public void TutorialStart()
    {
        idx = 0;
        TutorialIN(-100f);
        GetComponent<CanvasGroup>().alpha = 1f;
        isOn = true;
        StartCoroutine(DialogueRoutine());
    }

    void TutorialIN(float value)
    {
        DOTween.To(
            () => colorAdjustments.saturation.value,
            x => colorAdjustments.saturation.value = x,
            value,
            0.1f
        ).SetEase(Ease.InOutSine);
    }

    public void SetText(string text)
    {
        _text.SetText(text);               //텍스트를 입력
        _text.ForceMeshUpdate();

        textPanel.gameObject.SetActive(true);
        textPanel.GetComponent<RectTransform>().position = tutorialDialogue[idx].TextPoint.position;
        Vector2 textSize = _text.GetRenderedValues(false);   //띄어쓰기도 포함된 (렌더링 된) 텍스트의 너비
        Vector2 offset = new Vector2(8, 8); //여백의 크기
        textPanel.GetComponent<RectTransform>().sizeDelta = textSize + offset;
    }

    private void Update()
    {
        if (isOn&&Input.GetMouseButtonDown(0)) isMouse = true;
    }

    private IEnumerator DialogueRoutine()
    {
        SetText(tutorialDialogue[idx].tutorialText);
        if (tutorialImage.Length > idx)
            tutorialImage[idx].gameObject.SetActive(true);
        yield return new WaitUntil(()=>isMouse);
        isMouse = false;
        if (tutorialImage.Length > idx)
            tutorialImage[idx].gameObject.SetActive(false);
        textPanel.gameObject.SetActive(false);
        idx++;

        if (!(tutorialDialogue.Length <= idx))
            StartCoroutine(DialogueRoutine());
        else
        {
            TutorialIN(-10f);
            GetComponent<CanvasGroup>().alpha = 0f;
            isOn = false;
        }
    }
}

[Serializable]
public struct Dialogue
{
    public string tutorialText;
    public Transform TextPoint;
}
