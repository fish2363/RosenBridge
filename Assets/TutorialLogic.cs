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
    [Header("��� �Է��ϱ� \\n�� �Է��ϸ� ����")]
    public Dialogue[] tutorialDialogue;
    int idx;
    private bool isMouse;
    private bool isOn;

    [SerializeField]
    private Image[] tutorialImage;

    [SerializeField]
    private TextMeshProUGUI _text;

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

    private void Update()
    {
        if (isOn&&Input.GetMouseButtonDown(0)) isMouse = true;
    }

    private IEnumerator DialogueRoutine()
    {
        _text.text = tutorialDialogue[idx].tutorialText;
        _text.transform.position = tutorialDialogue[idx].TextPoint.position;
        if (tutorialImage.Length > idx)
            tutorialImage[idx].gameObject.SetActive(true);
        yield return new WaitUntil(()=>isMouse);
        isMouse = false;
        if (tutorialImage.Length > idx)
            tutorialImage[idx].gameObject.SetActive(false);
        idx++;

        if (!(tutorialDialogue.Length <= idx))
            StartCoroutine(DialogueRoutine());
        else
        {
            TutorialIN(-10f);
            _text.text = "";
            _text.gameObject.SetActive(false);
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
