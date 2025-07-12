using DG.Tweening;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MainmenuLogic : MonoBehaviour
{
    private LensDistortion lensDistortion;
    public PlayableDirector timeline;
    public Volume volume;
    public SineMover[] SineMover;
    public Camera Camera;

    void Start()
    {
        if (!volume.profile.TryGet(out lensDistortion))
        {
            Debug.LogError("Lens Distortion not found in Volume Profile!");
        }
    }

    public void AnimateDistortion(float toValue, float duration)
    {
        if (lensDistortion == null) return;

        DOTween.To(
            () => lensDistortion.intensity.value,
            x => lensDistortion.intensity.value = x,
            toValue,
            duration
        ).SetEase(Ease.InOutSine);
        DOTween.To(() => Camera.orthographicSize, x => Camera.orthographicSize = x, 0.56f, 1f);
    }

    public void GameStart()
    {
        for(int i =0;i<SineMover.Length;i++)
        {
            SineMover[i].enabled = false;
        }
        GetComponent<CanvasGroup>().DOFade(0f,0.2f);
        timeline.Play();
    }

    public void InToVoid()
    {
        AnimateDistortion(-1f, 1f);
    }

    public void StartTutorial()
    {

    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        // ������ ��忡�� ���� ���̸� Play ��带 ��
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // ����� ���ӿ����� ���ø����̼� ����
        Application.Quit();
#endif
    }
}
