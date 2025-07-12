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

    [Header("행성프리펩(순서:수,금,지,화,목,토,천,해)")]
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

    [Header("테트리스 블럭 설정값")]
    [field: SerializeField] public float MoveSpeed { get; set; }
    [field: SerializeField] public float RotateSpeed { get; set; }

    [Header("벽 생성 속도")]
    [SerializeField] private float wallSpawnSpeed;
    private float wallSpawnIdx;

    [Header("한 줄당 점수")]
    public int boomScore =500;
    [Header("폭발 이펙트")]
    public GameObject EffectPrefabs;
    [Header("레벨당 생성 속도 감소")]
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

        // 벽과의 거리 계산
        float left = leftWall.transform.position.x;
        float right = rightWall.transform.position.x;
        float minGap = 0.6f; // 벽과 최소 간격 (Collider 크기 기준)

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
