using UnityEngine;

[CreateAssetMenu(fileName = "EatSO", menuName = "SO/EatSO")]
public class EatSO : ScriptableObject
{
    public PlanetType planetType;
    public GameObject planet;
    [Header("������ ������")]
    public float[] sizes;
}

public enum PlanetType
{
    Mercury,
    Venus,
    Earth,
    Mars,
    Jupiter,
    Saturn,
    Uranus,
    Neptune
}

