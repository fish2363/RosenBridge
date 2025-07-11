using UnityEngine;
using System.Collections;

public class PlanetTetrisBlock : MonoBehaviour
{
    public Rigidbody2D RbCompo { get; set; }
    public bool isPlace;
    public bool isBoom;
    public bool isBlinking = false;

    private SpriteRenderer sr;

    private void Awake()
    {
        RbCompo = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogWarning("SpriteRenderer가 PlanetTetrisBlock에 없습니다.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Place") || collision.gameObject.CompareTag("Tetris"))
        {
            isPlace = true;
        }
    }

    public void StartBlinking()
    {
        if (isBlinking || isBoom) return;
        isBlinking = true;
        StartCoroutine(BlinkRoutine());
    }

    public void StopBlinking()
    {
        isBlinking = false;
        if (sr != null)
        {
            sr.color = Color.white;
        }
    }

    private IEnumerator BlinkRoutine()
    {
        while (isBlinking)
        {
            sr.color = Color.gray;
            yield return new WaitForSeconds(0.2f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }
    }
}