using System.Collections.Generic;
using UnityEngine;

public class MeteoSpawner : MonoBehaviour
{
    [Header("�����ɸ��½ð�")]
    [SerializeField] private float spawnDuration;

    private float duration;
    [SerializeField] private GameObject meteoPrefab;
    [SerializeField] private GameObject whiteTetBoomPrefab;
    [SerializeField] private int spawnCount = 2;

    private BoxCollider2D col;

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        duration += Time.deltaTime;
        
        if(spawnDuration <= duration)
        {
            SpawnMeteors();
            duration = 0f;
        }
    }

    public void SpawnWhiteTetBoomSpawn()
    {
        Vector2 center = col.bounds.center;
        Vector2 size = col.bounds.size;

        for (int i = 0; i < 1; i++)
        {
            // X�� ����~������ ���̿��� ����, Y�� �߽� (�Ǵ� ���� ����)
            float randomX = Random.Range(center.x - size.x / 2f, center.x + size.x / 2f);
            float y = center.y;

            Vector3 spawnPos = new Vector3(randomX, y, 0f);
            GameObject gameOb = Instantiate(whiteTetBoomPrefab, spawnPos, Quaternion.identity);
        }
    }
    private void SpawnMeteors()
    {
        Vector2 center = col.bounds.center;
        Vector2 size = col.bounds.size;

        for (int i = 0; i < spawnCount; i++)
        {
            // X�� ����~������ ���̿��� ����, Y�� �߽� (�Ǵ� ���� ����)
            float randomX = Random.Range(center.x - size.x / 2f, center.x + size.x / 2f);
            float y = center.y;

            Vector3 spawnPos = new Vector3(randomX, y, 0f);
            GameObject gameOb = Instantiate(meteoPrefab, spawnPos, Quaternion.identity);
        }
    }
}
