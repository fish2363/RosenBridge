using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private BlackHole player;

    [field:SerializeField]
    public int CurrentScore { get; set; }

    [Header("레벨업하는 스코어 단위")]
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
    private void Update()
    {
    }
    public void Score(int score)
    {
        CurrentScore += score;

        int expectedLevel = Mathf.FloorToInt(CurrentScore / LevelUpScore);
        if (player.Level <= expectedLevel)
        {
            player.PlusLevel();
        }
    }
}
