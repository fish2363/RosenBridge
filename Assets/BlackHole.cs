using UnityEngine;
using System;
using TMPro;
using Ami.BroAudio;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class BlackHole : MonoBehaviour
{
    public TetrisCompo tetrisCompo;

    [SerializeField] private Camera currentCamera;
    [SerializeField] private InputReader InputReader;
    [SerializeField] private Rigidbody2D _rigid;
    [field: SerializeField] public float MoveSpeed { get; set; }

    [SerializeField] private TextMeshProUGUI levelText;

    [Header("���� �� ������")]
    public Level[] levelSetting;
    public int Level { get; set; } = 1;

    [Header("���� ����")]
    public float diameter;
    private CircleCollider2D col;

    [Header("�����")]
    [SerializeField] private SoundID levelUp;

    [Header("���� �ð�")]
    public float stunDuration;
    private float duri;
    private bool isDamage;

    [Header("������ UI ����")]
    [SerializeField] private Image targetImage;
    [SerializeField] private float blinkDuration = 2f;
    [SerializeField] private float blinkInterval = 0.7f;

    private Tween blinkTween;

    public void StartBlink()
    {
        blinkTween?.Kill();

        // ������ ���� �� �����ϰ� ����
        Color c = targetImage.color;
        c.a = 0f;
        targetImage.color = c;

        // ������ Ʈ�� (0 �� 1 �ݺ�)
        blinkTween = targetImage.DOFade(1f, blinkInterval)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .SetTarget(targetImage);

        // 3�� �� ���� �� ���� 0 ����
        DOVirtual.DelayedCall(blinkDuration, () =>
        {
            blinkTween?.Kill();
            targetImage.DOFade(0f, 0.1f);
        });
    }

    private void Start()
    {
        col = GetComponent<CircleCollider2D>();
        tetrisCompo = FindAnyObjectByType<TetrisCompo>();
        SetPlayerSetting(levelSetting[0].levelSpeed, levelSetting[0].levelSize);
    }

    public void DamagePlayer()
    {
        if (isDamage) return;
        _rigid.linearVelocity = Vector2.zero;
        tetrisCompo.OnLineDestroyEvent?.Invoke();
        StartBlink();
        isDamage = true;
    }

    public void PlusLevel()
    {
        Level++;
        if (Level == 20) SceneManager.LoadScene("Clear");
        BroAudio.Play(levelUp);
        FindAnyObjectByType<MeteoSpawner>().SpawnWhiteTetBoomSpawn();
        tetrisCompo.DecreaseWallSpawnSpeed(tetrisCompo.levelDecreaseWallSpawnSpeed);

        if (Level > levelSetting.Length) return;

        Debug.Log($"{Level}����");
        SetPlayerSetting(levelSetting[Level - 1].levelSpeed, levelSetting[Level - 1].levelSize);
    }

    public void SetPlayerSetting(float speed, float size)
    {
        levelText.text = $"<b>{Level}</b>";
        MoveSpeed = speed;
        transform.localScale = new Vector3(size, size, transform.localScale.z);
        currentCamera.fieldOfView = levelSetting[Level - 1].cameraSize;

        float radius = col.radius;
        float scale = transform.lossyScale.x;
        diameter = radius * 2f * scale;

        Debug.Log($"�� ������ {diameter:F2}");
    }

    private void FixedUpdate()
    {
        if (isDamage)
        {
            duri += Time.deltaTime;
            if (duri >= stunDuration)
            {
                isDamage = false;
                duri = 0f;
            }
        }
        else
        {
            Vector2 moveDir = InputReader.HoleInputDirection;
            _rigid.linearVelocity = moveDir * MoveSpeed;
        }
    }
}

[Serializable]
public struct Level
{
    public float levelSize;
    public float levelSpeed;
    public float cameraSize;
}
