using UnityEngine;

public class SinMover : MonoBehaviour
{
    private float amplitude = 0.1f;   // ����
    [SerializeField] private float frequency = 1f;     // ���� �ӵ�
    [SerializeField] private bool startOnAwake = true;

    private Vector3 initialLocalPosition;
    private float timeOffset;

    private bool isMoving = false;

    private void Awake()
    {
        initialLocalPosition = transform.localPosition;
        timeOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    private void OnEnable()
    {
        if (startOnAwake)
            StartMoving();
    }

    public void StartMoving()
    {
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
        transform.localPosition = initialLocalPosition;
    }

    private void LateUpdate()
    {
        if (!isMoving) return;

        float offset = Mathf.Sin(Time.time * frequency + timeOffset) * amplitude;

        // �θ� ���� ���� Y�� �������� ���� � ������ ����
        transform.localPosition = initialLocalPosition + Vector3.up * offset;
    }
}
