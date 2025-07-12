using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TetrisCompo : MonoBehaviour
{
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;

    [field: SerializeField]
    private InputReader InputReader { get; set; }

    private Dictionary<PlanetType, GameObject> getPlanetsDictionary = new Dictionary<PlanetType, GameObject>();
    private int idx;

    [Header("�༺������(����:��,��,��,ȭ,��,��,õ,��)")]
    [SerializeField]
    private GameObject[] planetPrefabs;

    [SerializeField]
    private GameObject[] walletPrefabs;
    private int wallIdx;
    private float firstScore;
    private float currentScore;
    private bool isOneTime = true;

    [SerializeField]
    private Transform[] spawnPoints;

    private List<PlanetTetrisBlock> fieldTetris=new();

    [Header("��Ʈ���� �� ������")]
    [field: SerializeField] public float MoveSpeed { get; set; }
    [field: SerializeField] public float RotateSpeed { get; set; }

    [Header("�� ���� �ӵ�")]
    [SerializeField] private float wallSpawnSpeed;
    private float wallSpawnIdx;

    [Header("�� �ٴ� ����")]
    public int boomScore =500;
    [Header("���� ����Ʈ")]
    public GameObject EffectPrefabs;
    [Header("������ ���� �ӵ� ����")]
    public float levelDecreaseWallSpawnSpeed =0.2f;

    public UnityEvent OnLockEvent;
    public UnityEvent OnUNLockEvent;
    public UnityEvent OnLineDestroyEvent;

    private void Awake()
    {
        foreach (PlanetType type in Enum.GetValues(typeof(PlanetType)))
        {
            getPlanetsDictionary.Add(type, planetPrefabs[idx]);
            Debug.Log($"{planetPrefabs[idx].name}");
            idx++;
        }
    }

    public void DecreaseWallSpawnSpeed(float value)
    {
        if(wallSpawnSpeed > 1)
        wallSpawnSpeed -= value;
    }

        private void Update()
    {
        wallSpawnIdx += Time.deltaTime;
        currentScore = GameManager.Instance.CurrentScore;

        if(wallSpawnIdx > wallSpawnSpeed)
        {
            LockLastLine();
            wallSpawnIdx = 0f;
            if (isOneTime)
            {
                firstScore = GameManager.Instance.CurrentScore;
                isOneTime = false;
            }
        }
        if(firstScore + 500f <= currentScore)
        {
            firstScore = currentScore;
            UnLockFirstLine();
        }
    }

    public void UnLockFirstLine()
    {
        if (wallIdx <= 0) return;
        wallIdx--;
        walletPrefabs[wallIdx].GetComponent<Line>().LockLine();
        OnUNLockEvent?.Invoke();
    }

    [ContextMenu("TestLockEvent")]
    public void TestLockEvent() => OnLockEvent?.Invoke();

    [ContextMenu("TestUNLockEvent")]
    public void TestUNLockEvent() => OnUNLockEvent?.Invoke();


    [ContextMenu("LineDestroyEffect")]
    public void LineDestroyEffect() => OnLineDestroyEvent?.Invoke();

    public void LockLastLine()
    {
        if (walletPrefabs.Length -1 < wallIdx) return;
        walletPrefabs[wallIdx].GetComponent<Line>().UnLockLine();
        wallIdx++;
        OnLockEvent?.Invoke();
    }

    public void SpawnTetris(PlanetType type)
    {
        int rand = UnityEngine.Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[rand];

        Vector3 pos = spawnPoint.position;

        // ������ �Ÿ� ���
        float left = leftWall.transform.position.x;
        float right = rightWall.transform.position.x;
        float minGap = 0.6f; // ���� �ּ� ���� (Collider ũ�� ����)

        if (Mathf.Abs(pos.x - left) < minGap)
            pos.x = left + minGap;

        if (Mathf.Abs(pos.x - right) < minGap)
            pos.x = right - minGap;

        PlanetTetrisBlock tetris = Instantiate(getPlanetsDictionary[type], pos, Quaternion.identity).GetComponent<PlanetTetrisBlock>();
        fieldTetris.Add(tetris);
    }

    public void DestroyTetris()
    {
        foreach(PlanetTetrisBlock planet in fieldTetris)
        {
            if(planet.isBoom)
            {
                fieldTetris.Remove(planet);
            }
        }
    }

    public void RemoveTetris()
    {
        fieldTetris.RemoveAt(0);
    }

    void FixedUpdate()
    {
        Vector2 moveDir = InputReader.TetrisInputDirection;
        if (fieldTetris.Count <= 0) return;
        if (moveDir.y > 0f)
        {
            moveDir.y = 0f;
            fieldTetris[0].transform.Rotate(0,0,RotateSpeed*Time.fixedDeltaTime);
        }

        fieldTetris[0].RbCompo.gravityScale = moveDir.y < 0f ? 1f : 0.1f;

        if (fieldTetris[0].isPlace) RemoveTetris();
        else
            fieldTetris[0].RbCompo.linearVelocity = new Vector3(moveDir.x * MoveSpeed, fieldTetris[0].RbCompo.linearVelocityY);
    }
}
