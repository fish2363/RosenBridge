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
            Debug.LogError("Collider2D�� �����ϴ�!");
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

        // 3�� �̻� ������ ���� ���� ��
        if (count >= 10)
        {
            detectTimer += Time.fixedDeltaTime;

            // ������ ���� (1����)
            foreach (var block in currentDetectedBlocks)
            {
                if (!blinkingBlocks.Contains(block))
                {
                    block.StartBlinking();
                    blinkingBlocks.Add(block);
                }
            }

            // 3�� �̻� ���� �������� �� �ı� �غ�
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
            // ���� ���� �� �ʱ�ȭ
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

        // �ڿ������� �������� ����
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