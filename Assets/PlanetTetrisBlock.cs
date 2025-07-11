using UnityEngine;

public class PlanetTetrisBlock : MonoBehaviour
{
    public Rigidbody2D RbCompo { get; set; }
    public bool isPlace;

    private void Awake()
    {
        RbCompo = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Place"))
        {
            isPlace = true;
        }
    }
}
