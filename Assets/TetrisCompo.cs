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
    private GameObject[] walletPrefabs;

    [SerializeField]
    private Transform[] spawnPoints;

    private List<PlanetTetrisBlock> fieldTetris=new();

    [Header("테트리스 블럭 설정값")]
    [field: SerializeField] public float MoveSpeed { get; set; }
    [field: SerializeField] public float RotateSpeed { get; set; }

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


        if (fieldTetris[0].isPlace) RemoveTetris();
        else
            fieldTetris[0].RbCompo.linearVelocity = new Vector3(moveDir.x * MoveSpeed, fieldTetris[0].RbCompo.linearVelocityY);
    }
}
