using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // ��Ʈ�� ���ӽ����̽�
using UnityEngine.UI;
using TMPro;
using Ami.BroAudio;

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

    [Header("������ Ʀ ����")]
    [SerializeField] private float squishScale = 0.8f;
    [SerializeField] private float stretchScale = 1.2f;
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private int loopCount = -1; // ���� �ݺ�

    private RectTransform rectTransform;
    private Sequence bounceSequence;

    private Animator Animator;

    [Header("�����")]
    [SerializeField]
    private SoundID inSound;

    private void Awake()
    {
        Instance = this;
        rectTransform = GetComponent<RectTransform>();
        Animator = GetComponentInChildren<Animator>();
        foreach (PlanetType type in Enum.GetValues(typeof(PlanetType)))
        {
            getPlanetsDictionary.Add(type, planetSprites[idx]);
            idx++;
        }
    }

    private int _eatCount = 0;
    public void EatPlanet(PlanetType planet)
    {
        Animator.SetBool("Rosen", true);
        BroAudio.Play(inSound);
        _eatCount++;


        GameObject p = Instantiate(planetPrefabs, gameObject.transform);
        p.GetComponent<RectTransform>().position = left.position;

        if (planet == PlanetType.Venus)
            p.GetComponent<BugUIFix>().FixLong();
        if (planet == PlanetType.Earth)
            p.GetComponent<BugUIFix>().FixEarth();

        var sr = p.GetComponent<Image>();
        sr.sprite = getPlanetsDictionary[planet];

        Transform tf = p.transform;
        tf.localScale = Vector3.zero;

        tf.DOScale(Vector3.one, 0.5f).SetEase(inAnimation);

        tf.DOMove(right.position, moveDuration).SetEase(moveAnimation).OnComplete(() =>
        {
            tf.DOScale(Vector3.zero, 0.5f).SetEase(outAnimation).OnComplete(() =>
            {
                _tetrisCompo.SpawnTetris(planet);
                Destroy(p);

                _eatCount--;
                TryFinishAll();
            });
        });
    }


    private void TryFinishAll()
    {
        if (_eatCount == 0)
        {
            Animator.SetBool("Rosen", false);
        }
    }
}