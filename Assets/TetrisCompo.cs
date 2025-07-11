using System;
using System.Collections.Generic;
using UnityEngine;

public class TetrisCompo : MonoBehaviour
{
    [field: SerializeField]
    private InputReader InputReader { get; set; }

    private Dictionary<PlanetType, GameObject> getPlanetsDictionary = new Dictionary<PlanetType, GameObject>();
    private int idx;

    [Header("행성프리펩(순서:수,금,지,화,목,토,천,해)")]
    [SerializeField]
    private GameObject[] planetPrefabs;

    [SerializeField]
    private Transform[] spawnPoints;

    private List<PlanetTetrisBlock> fieldTetris=new();

    [field: SerializeField] public float MoveSpeed { get; set; }

    private void Awake()
    {
        foreach (PlanetType type in Enum.GetValues(typeof(PlanetType)))
        {
            getPlanetsDictionary.Add(type, planetPrefabs[idx]);
            Debug.Log($"{planetPrefabs[idx].name}");
            idx++;
        }
    }

    public void SpawnTetris(PlanetType type)
    {
        int rand = UnityEngine.Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[rand];
        PlanetTetrisBlock tetris = Instantiate(getPlanetsDictionary[type],spawnPoint.position,Quaternion.identity).GetComponent<PlanetTetrisBlock>();
        fieldTetris.Add(tetris);
    }

    public void RemoveTetris()
    {
        fieldTetris.RemoveAt(0);
    }

    void FixedUpdate()
    {
        Vector2 moveDir = InputReader.TetrisInputDirection;
        if (moveDir.y > 0f) moveDir.y = 0f;

        if (fieldTetris[0].isPlace) RemoveTetris();
        else
            fieldTetris[0].RbCompo.linearVelocity = moveDir * MoveSpeed;
    }
}
