using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    private Collider2D myCollider;
    private int tetrisLayer;

    private float detectTimer = 0f;
    private const float requiredDuration = 3f;
    private const float requiredCoverageRatio = 0.8f; // ▶ 80% 기준

    private List<PlanetTetrisBlock> currentDetectedBlocks = new();
    private List<PlanetTetrisBlock> blinkingBlocks = new();

    private TetrisCompo tetrisCompo;

    private float currentCoverage = 0f; // 👉 기즈모용 필드 추가

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
        Bounds lineBounds = myCollider.bounds;
        Collider2D[] hits = Physics2D.OverlapBoxAll(lineBounds.center, lineBounds.size, 0f);

        currentDetectedBlocks.Clear();
        float totalWidthCovered = 0f;

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

                    Bounds blockBounds = hit.bounds;
                    float overlapMinX = Mathf.Max(lineBounds.min.x, blockBounds.min.x);
                    float overlapMaxX = Mathf.Min(lineBounds.max.x, blockBounds.max.x);
                    float overlapWidth = Mathf.Max(0f, overlapMaxX - overlapMinX);

                    totalWidthCovered += overlapWidth;
                }
            }
        }

        currentCoverage = totalWidthCovered / lineBounds.size.x;

        if (currentCoverage >= requiredCoverageRatio)
        {
            detectTimer += Time.fixedDeltaTime;

            foreach (var block in currentDetectedBlocks)
            {
                if (!blinkingBlocks.Contains(block))
                {
                    block.StartBlinking();
                    blinkingBlocks.Add(block);
                }
            }

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
            Gizmos.color = currentCoverage >= requiredCoverageRatio ? Color.green : Color.red;
            Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
        }
    }
}