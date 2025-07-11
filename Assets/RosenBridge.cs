using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // ��Ʈ�� ���ӽ����̽�
using UnityEngine.UI;
using TMPro;

public class RosenBridge : MonoBehaviour
{
    public static RosenBridge Instance;

    [SerializeField] private TetrisCompo _tetrisCompo;

    [SerializeField] private Transform left;
    [SerializeField] private Transform right;
    [SerializeField] private GameObject planetPrefabs;

    [Header("���ʿ��� �����ʱ��� �����̴� �ð�")]
    [SerializeField] private float moveDuration;

    private Dictionary<PlanetType, Sprite> getPlanetsDictionary = new Dictionary<PlanetType, Sprite>();
    private int idx;

    [Header("�༺�̹���(����:��,��,��,ȭ,��,��,õ,��)")]
    [SerializeField]
    private Sprite[] planetSprites;

    [Header("�ִϸ��̼�")]
    public Ease inAnimation;
    public Ease outAnimation;
    public Ease moveAnimation;

    private void Awake()
    {
        Instance = this;

        foreach (PlanetType type in Enum.GetValues(typeof(PlanetType)))
        {
            getPlanetsDictionary.Add(type, planetSprites[idx]);
            idx++;
        }
    }

    public void EatPlanet(PlanetType planet)
    {
        GameObject p = Instantiate(planetPrefabs, gameObject.transform);
        p.GetComponent<RectTransform>().position = left.position;
        if (planet == PlanetType.Venus)
            p.GetComponent<BugUIFix>().FixLong();
        if (planet == PlanetType.Earth)
            p.GetComponent<BugUIFix>().FixEarth();
        var sr = p.GetComponent<Image>();
        sr.sprite = getPlanetsDictionary[planet];

        Transform tf = p.transform;
        tf.localScale = Vector3.zero; // ���� ũ�� = 0

        // Ŀ���� ����
        tf.DOScale(Vector3.one, 0.5f).SetEase(inAnimation); // 0.5�� ���� õõ�� Ŀ��

        // �̵� + ���� �� �۾����� ����
        tf.DOMove(right.position, moveDuration).SetEase(moveAnimation).OnComplete(() =>
        {
            tf.DOScale(Vector3.zero, 0.5f).SetEase(outAnimation).OnComplete(() =>
            {
                _tetrisCompo.SpawnTetris(planet);
                Destroy(p);
            });
        });
    }
}