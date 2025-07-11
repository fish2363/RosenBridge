using UnityEngine;
using System;

public class BlackHole : MonoBehaviour
{
    [SerializeField]
    private Camera currentCamera;
    [SerializeField]
    private InputReader InputReader;
    [SerializeField]
    private Rigidbody2D _rigid;
    [field: SerializeField] public float MoveSpeed { get; set; }

    [Header("레벨 별 설정값")]
    public Level[] levelSetting;

    public int Level { get; set; } = 1;

    private void Start()
    {
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
        MoveSpeed = speed;
        transform.localScale = new Vector3(size,size, transform.localScale.z);
        currentCamera.fieldOfView = levelSetting[Level - 1].cameraSize;
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