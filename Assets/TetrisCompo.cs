using System;
using System.Collections;
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
    private List<PlanetTetrisBlock> destoryBlock=new();

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

    public void DestroyTetrisBlock()
    {
        List<PlanetTetrisBlock> toDestroy = new();

        foreach (PlanetTetrisBlock block in fieldTetris)
        {
            if (block.isBoom)
            {
                toDestroy.Add(block);
            }
        }

        foreach (var block in toDestroy)
        {
            fieldTetris.Remove(block);
            destoryBlock.Add(block);
        }

        StartCoroutine(DestroyRoutine());
    }


    private IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(1f);
        foreach (PlanetTetrisBlock block in destoryBlock)
        {
            Destroy(block.gameObject);
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


        if (fieldTetris.Count <= 0) return;
        if (fieldTetris[0].isPlace) RemoveTetris();
        else
            fieldTetris[0].RbCompo.linearVelocity = new Vector3(moveDir.x * MoveSpeed, fieldTetris[0].RbCompo.linearVelocityY);
    }
}
