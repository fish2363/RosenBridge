using UnityEngine;
using System;
using TMPro;

public class BlackHole : MonoBehaviour
{
    [SerializeField]
    private Camera currentCamera;
    [SerializeField]
    private InputReader InputReader;
    [SerializeField]
    private Rigidbody2D _rigid;
    [field: SerializeField] public float MoveSpeed { get; set; }

    [SerializeField]
    private TextMeshProUGUI levelText;
    

    [Header("���� �� ������")]
    public Level[] levelSetting;

    public int Level { get; set; } = 1;

    [Header("���� ����")]
    public float diameter;
    CircleCollider2D col;

    private void Start()
    {
        col = GetComponent<CircleCollider2D>();
        SetPlayerSetting(levelSetting[0].levelSpeed,levelSetting[0].levelSize);
    }

    public void PlusLevel()
    {
        Level++;
        if (Level > levelSetting.Length) return;
        Debug.Log($"{Level}����");
        SetPlayerSetting(levelSetting[Level-1].levelSpeed, levelSetting[Level-1].levelSize);
    }

    public void SetPlayerSetting(float speed,float size)
    {
        levelText.text = $"<b>{Level}</b>";
        MoveSpeed = speed;
        transform.localScale = new Vector3(size,size, transform.localScale.z);
        currentCamera.fieldOfView = levelSetting[Level - 1].cameraSize;
        float radius = col.radius;
        float scale = transform.lossyScale.x; // Circle�̸� x�� y�� ���ٰ� ����
        diameter = radius * 2f * scale;

        Debug.Log($"�� ������ {diameter:F2}");
    }

    private void FixedUpdate()
    {
        Vector2 moveDir = InputReader.HoleInputDirection;

        _rigid.linearVelocity = moveDir * MoveSpeed;
    }
}

[Serializable]
public struct Level
{
    public float levelSize;
    public float levelSpeed;
    public float cameraSize;
}