using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // 두트윈 네임스페이스
using UnityEngine.UI;
using TMPro;

public class RosenBridge : MonoBehaviour
{
    public static RosenBridge Instance;

    [SerializeField] private TetrisCompo _tetrisCompo;

    [SerializeField] private Transform left;
    [SerializeField] private Transform right;
    [SerializeField] private GameObject planetPrefabs;

    [Header("왼쪽에서 오른쪽까지 움직이는 시간")]
    [SerializeField] private float moveDuration;

    private Dictionary<PlanetType, Sprite> getPlanetsDictionary = new Dictionary<PlanetType, Sprite>();
    private int idx;

    [Header("행성이미지(순서:수,금,지,화,목,토,천,해)")]
    [SerializeField]
    private Sprite[] planetSprites;

    [Header("애니메이션")]
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
        tf.localScale = Vector3.zero; // 시작 크기 = 0

        // 커지며 등장
        tf.DOScale(Vector3.one, 0.5f).SetEase(inAnimation); // 0.5초 동안 천천히 커짐

        // 이동 + 도착 후 작아지며 제거
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