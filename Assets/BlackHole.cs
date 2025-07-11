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
    

    [Header("레벨 별 설정값")]
    public Level[] levelSetting;

    public int Level { get; set; } = 1;

    [Header("현재 지름")]
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
        Debug.Log($"{Level}레벨");
        SetPlayerSetting(levelSetting[Level-1].levelSpeed, levelSetting[Level-1].levelSize);
    }

    public void SetPlayerSetting(float speed,float size)
    {
        levelText.text = $"<b>{Level}</b>";
        MoveSpeed = speed;
        transform.localScale = new Vector3(size,size, transform.localScale.z);
        currentCamera.fieldOfView = levelSetting[Level - 1].cameraSize;
        float radius = col.radius;
        float scale = transform.lossyScale.x; // Circle이면 x와 y가 같다고 가정
        diameter = radius * 2f * scale;

        Debug.Log($"내 지름은 {diameter:F2}");
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