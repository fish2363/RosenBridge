using UnityEngine;

public class Planet : MonoBehaviour
{
    [HideInInspector]
    public EatSO currentSO;
    [HideInInspector]
    public float currentScale;
    [HideInInspector]
    public float currentLevel;
    [HideInInspector]
    public int score;

    [Header("��Ȧ ����")]
    public Transform blackholeCenter;     // �߽��� (��Ȧ)
    public float rotateSpeed = 180f;      // ��/��
    public float shrinkSpeed = 0.5f;      // ��� �ִϸ��̼� �ӵ�
    public float magneticForceSizeSpeed = 0.1f;      //�ڷ� ��� �ӵ�
    public float magnetisRotateSpeed = 180f;      // ��/��
    public float magnetismSpeed = 0.3f;      // �ڷ��� ������� �ӵ�
    public float pullRate = 0.97f;        // ���� �ӵ� (0.95~0.99)

    [Header("�ı� ����")]
    public float minScaleThreshold = 0.1f;   // ��� ������ ����
    public float minRadiusThreshold = 0.05f; // ������ ����

    private bool isAbsorbing = false;

    private float currentRadius;
    private float currentAngle;

    [Header("���� ����")]
    [SerializeField]
    private float diameter;

    CircleCollider2D col;
    bool isTry;
    bool isAte;

    void Start()
    {
        col = GetComponent<CircleCollider2D>();
        blackholeCenter = FindAnyObjectByType<BlackHole>().gameObject.transform;
    }

    public void StartBlackhole()
    {
        if (blackholeCenter == null) return;

        isAbsorbing = true;

        // ���� ��ġ���� ��Ȧ �߽ɱ����� �Ÿ� �� ���� ���
        Vector3 offset = transform.position - blackholeCenter.position;
        currentRadius = offset.magnitude;
        currentAngle = Mathf.Atan2(offset.y, offset.x);
    }

    void Update()
    {
        float radius = col.radius;
        float scale = transform.lossyScale.x; // Circle�̸� x�� y�� ���ٰ� ����
        diameter = radius * 2f * scale;

        if(isTry)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, shrinkSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, blackholeCenter.position, magnetismSpeed * Time.deltaTime);
        }

        if (isAbsorbing && blackholeCenter != null)
        {
            // 1. ȸ�� (���� ������ ����)
            float angleDelta = rotateSpeed * Mathf.Deg2Rad * Time.deltaTime;
            currentAngle += angleDelta;
            transform.Rotate(0, 0, magnetisRotateSpeed * Time.deltaTime);

            // 2. �߽����� ���� (���� ������ ���)
            currentRadius *= Mathf.Pow(pullRate, Time.deltaTime * 60f); // ������ ����

            // 3. ���ο� ��ġ ���
            Vector3 offset = new Vector3(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle), 0f) * currentRadius;
            transform.position = blackholeCenter.position + offset;

            // 4. ���� �۾�����
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, shrinkSpeed * Time.deltaTime);

            // 5. ũ�� ���� or �Ÿ� �������� ����
            float avgScale = (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3f;
            if (avgScale < minScaleThreshold || currentRadius < minRadiusThreshold)
            {
                RosenBridge.Instance.EatPlanet(currentSO.planetType);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<BlackHole>().diameter >= diameter && !isAte)
            {
                GameManager.Instance.Score(score);
                StartBlackhole();
                isAte = true;
            }
            else
            {
                isTry = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTry = false;
            transform.localScale = new Vector2(currentScale,currentScale);
        }
    }
}
