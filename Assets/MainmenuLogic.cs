using DG.Tweening;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class MainmenuLogic : MonoBehaviour
{
    private LensDistortion lensDistortion;
    public PlayableDirector timeline;
    public Volume volume;
    public SineMover[] SineMover;
    public Camera Camera;
    private bool isRotate;
    public Image white;

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
        
    }

    public void GameStart()
    {
        AnimateDistortion(-0.49f, 1f);
        for (int i =0;i<SineMover.Length;i++)
        {
            SineMover[i].enabled = false;
        }
        GetComponentInChildren<CanvasGroup>().DOFade(0f,0.2f);
        timeline.Play();
    }

    public void InToVoid()
    {
        DOTween.To(() => Camera.orthographicSize, x => {
            Camera.orthographicSize = x;
        }, 0.56f, 0.5f).OnComplete(()=> { 
        white.DOFade(1f, 0.3f);
        });
        AnimateDistortion(-1f, 1f);
        isRotate = true;
    }

    private void Update()
    {
        if(isRotate)
        Camera.transform.parent.Rotate(0f,0f, 270f*Time.deltaTime);
    }

    public void StartTutorial()
    {

    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        // 에디터 모드에서 실행 중이면 Play 모드를 끔
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 빌드된 게임에서는 애플리케이션 종료
        Application.Quit();
#endif
    }
}
