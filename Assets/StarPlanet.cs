using Ami.BroAudio;
using UnityEngine;

public class StarPlanet : MonoBehaviour
{
    [HideInInspector]
    public EatSO currentSO;
    [HideInInspector]
    public float currentScale;
    [HideInInspector]
    public int score;

    [Header("��Ȧ ����")]
    public Transform blackholeCenter;     // �߽��� (��Ȧ)
    public float rotateSpeed = 180f;      // ��/��
    public float shrinkSpeed = 0.5f;      // ��� �ӵ�
    public float pullRate = 0.97f;        // ���� �ӵ� (0.95~0.99)
    public float pullDuration = 0.97f;        // ���� �ӵ� (0.95~0.99)
    public float magnetisRotateSpeed = 180f;      // ��/��

    [Header("�ı� ����")]
    public float minScaleThreshold = 0.1f;   // ��� ������ ����
    public float minRadiusThreshold = 0.05f; // ������ ����

    private bool isAbsorbing = false;

    private float currentRadius;
    private float currentAngle;
    [Header("�����")]
    [SerializeField]
    private SoundID eatSFX;
    private void Start()
    {
        pullDuration = Random.Range(70,200);
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
        if (!isAbsorbing && blackholeCenter == null) return;

        // 1. ȸ�� (���� ������ ����)
        float angleDelta = rotateSpeed * Mathf.Deg2Rad * Time.deltaTime;
        currentAngle += angleDelta;
        transform.Rotate(0, 0, magnetisRotateSpeed * Time.deltaTime);

        // 2. �߽����� ���� (���� ������ ���)
        currentRadius *= Mathf.Pow(pullRate, Time.deltaTime * pullDuration); // ������ ����

        // 3. ���ο� ��ġ ���
        Vector3 offset = new Vector3(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle), 0f) * currentRadius;
        transform.position = blackholeCenter.position + offset;

        // 4. ���� �۾�����
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, shrinkSpeed * Time.deltaTime);

        // 5. ũ�� ���� or �Ÿ� �������� ����
        float avgScale = (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3f;
        if (avgScale < minScaleThreshold || currentRadius < minRadiusThreshold)
        {
            BroAudio.Play(eatSFX);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isAbsorbing)
        {
            blackholeCenter = collision.gameObject.transform;
            GameManager.Instance.Score(score);
            StartBlackhole();
        }
    }

}
