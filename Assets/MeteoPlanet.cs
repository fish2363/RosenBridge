using DG.Tweening;
using UnityEngine;

public class MeteoPlanet : MonoBehaviour
{
    private Rigidbody2D _rigid;
    [SerializeField]
    private Transform meteoSprite;
    public float meteoSpinSpeed=90f;
    [SerializeField]
    private float moveSpeed=10f;
    [Header("디스폰시간")]
    [SerializeField] private float deSpawnDuration;
    private float deSpawnDu;

    public bool isEnemy;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        deSpawnDu += Time.deltaTime;
        if (deSpawnDuration <= deSpawnDu)
            Destroy(gameObject);
        _rigid.linearVelocity = new Vector2(-1, -1).normalized * moveSpeed;
        meteoSprite.Rotate(0f,0f,meteoSpinSpeed*Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            transform.DOMove(collision.gameObject.transform.position,1f);
            transform.DOScale(0f,1f).OnComplete(()=>
            {
                if (isEnemy)
                {
                    collision.GetComponent<BlackHole>().DamagePlayer();
                    DestroyObj();
                }
                else
                {
                    
                    DestroyObj();
                }
            });
            

        }
    }
    private void DestroyObj()
    {
        Destroy(gameObject);
    }
}
