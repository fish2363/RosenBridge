using Ami.BroAudio;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private BlackHole player;

    [Header("스코어 텍스트")]
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [field:SerializeField]
    public int CurrentScore { get; set; }

    [Header("레벨업하는 스코어 단위")]
    public float LevelUpScore =1000;

    [SerializeField]
    private CanvasGroup gameOverUI;

    private bool isInputGameOver;
    private bool isShaking;

    [Header("오디오")]
    [SerializeField]
    private SoundID bgm;
    [SerializeField]
    private SoundID gameOver;

    private void Awake()
    {
        Instance = this;
        player = FindAnyObjectByType<BlackHole>();
    }
    private void Start()
    {
        scoreText.text = $"{CurrentScore}";
        BroAudio.Play(bgm);
    }
    public void Score(int score)
    {
        CurrentScore += score;
        SetText();

        int expectedLevel = Mathf.FloorToInt(CurrentScore / LevelUpScore);
        if (player.Level <= expectedLevel)
        {
            player.PlusLevel();
        }
    }
    private void Update()
    {
        if(isInputGameOver)
        {
            isInputGameOver = false;
            if (Input.GetKeyDown(KeyCode.DownArrow))
                SceneManager.LoadScene("MainMenu");
            if (Input.GetKeyDown(KeyCode.S))
                SceneManager.LoadScene("GameScene");
        }
    }
    public void GameOver()
    {
        gameOverUI.DOFade(1f, 0.2f).OnComplete(() => {
            BroAudio.Play(gameOver);
            gameOverUI.blocksRaycasts = true;
            gameOverUI.interactable = true;
            isInputGameOver = true;
        }
        );
    }

    void SetText()
    {
        scoreText.text = $"{CurrentScore}";
        if(!isShaking)
        {
            isShaking = true;
            scoreText.GetComponent<RectTransform>().DOShakeRotation(0.2f, 20f, 10, 10f).OnComplete(() => isShaking = false);
        }
    }
}
