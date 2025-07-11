using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    private Collider2D myCollider;
    private int tetrisLayer;

    private float detectTimer = 0f;
    private const float requiredDuration = 3f;

    private List<PlanetTetrisBlock> currentDetectedBlocks = new();
    private List<PlanetTetrisBlock> blinkingBlocks = new();

    private TetrisCompo tetrisCompo;

    private void Start()
    {
        tetrisCompo = FindObjectOfType<TetrisCompo>();
        myCollider = GetComponent<Collider2D>();
        if (myCollider == null)
        {
            Debug.LogError("Collider2D가 없습니다!");
            enabled = false;
            return;
        }

        tetrisLayer = LayerMask.NameToLayer("Tetris");
    }

    private void FixedUpdate()
    {
        Bounds bounds = myCollider.bounds;
        Collider2D[] hits = Physics2D.OverlapBoxAll(bounds.center, bounds.size, 0f);

        currentDetectedBlocks.Clear();
        int count = 0;

        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject) continue;

            GameObject obj = hit.gameObject;
            if (obj.CompareTag("Tetris") || obj.layer == tetrisLayer)
            {
                var block = hit.GetComponent<PlanetTetrisBlock>();
                if (block != null)
                {
                    currentDetectedBlocks.Add(block);
                    count++;
                }
            }
        }

        // 3개 이상 감지된 상태 유지 중
        if (count >= 10)
        {
            detectTimer += Time.fixedDeltaTime;

            // 깜빡임 시작 (1번만)
            foreach (var block in currentDetectedBlocks)
            {
                if (!blinkingBlocks.Contains(block))
                {
                    block.StartBlinking();
                    blinkingBlocks.Add(block);
                }
            }

            // 3초 이상 감지 유지됐을 때 파괴 준비
            if (detectTimer >= requiredDuration)
            {
                foreach (PlanetTetrisBlock block in currentDetectedBlocks)
                {
                    block.StopBlinking();
                    block.isBoom = true;
                }

                blinkingBlocks.Clear();
                detectTimer = 0f;

                StartCoroutine(DestroyRoutine());
            }
        }
        else
        {
            // 감지 해제 시 초기화
            detectTimer = 0f;
            foreach (var block in blinkingBlocks)
            {
                block?.StopBlinking();
            }
            blinkingBlocks.Clear();
        }
    }

    private IEnumerator DestroyRoutine()
    {
        tetrisCompo.DestroyTetris();
        yield return new WaitForSeconds(0.7f);

        // 뒤에서부터 역순으로 제거
        for (int i = currentDetectedBlocks.Count - 1; i >= 0; i--)
        {
            var block = currentDetectedBlocks[i];
            if (block.isBoom)
            {
                currentDetectedBlocks.RemoveAt(i);
                Destroy(block.gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        var col = GetComponent<Collider2D>();
        if (col != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
        }
    }
}