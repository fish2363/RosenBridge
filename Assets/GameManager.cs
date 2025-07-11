using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private BlackHole player;

    [Header("���ھ� �ؽ�Ʈ")]
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [field:SerializeField]
    public int CurrentScore { get; set; }

    [Header("�������ϴ� ���ھ� ����")]
    public float LevelUpScore =1000;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        player = FindAnyObjectByType<BlackHole>();
    }
    private void Start()
    {
        scoreText.text = $"{CurrentScore}";
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

    void SetText()
    {
        scoreText.text = $"{CurrentScore}";
    }
}
