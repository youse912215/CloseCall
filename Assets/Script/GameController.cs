using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Cinemachine;

public class GameController : MonoBehaviour
{
    private const int FORK_QUANTITY = 4;
    private const int QUARTER_CIRCLE = 90;
    private Animator[] animator = new Animator[FORK_QUANTITY];
    [SerializeField] private GameObject kamatoo;
    private Animator flyAway;
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineOrbitalTransposer orbitalTransposer;
    private Text text;

    [SerializeField] private GameObject[] fork = new GameObject[FORK_QUANTITY];
    [SerializeField] private GameObject obj;
    [SerializeField] GameObject textObj;

    private readonly List<string> eventName = new List<string>()
    {
        "EventBlueFork",
        "EventYellowFork",
        "EventGreenFork",
        "EventRedFork"
    };

    private Color[] col = new Color[FORK_QUANTITY];

    private float axis = 0.0f;
    private int forkNumber = 0;
    private bool flyFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        //　各コンポーネントを取得
        virtualCamera = obj.GetComponent<CinemachineVirtualCamera>();
        orbitalTransposer = virtualCamera.GetComponentInChildren<CinemachineOrbitalTransposer>();
        text = textObj.GetComponent<Text>();

        //　各ForkObjectのAnimatorを取得
        foreach (var (v, i) in fork.Select((v, i) => (v, i)))
            animator[i] = v.GetComponent<Animator>();

        flyAway = kamatoo.GetComponent<Animator>();

        //　各フォークの色マテリアルを文字色に割り当て
        for (int i = 0; i < FORK_QUANTITY; ++i)
            col[i] = fork[i].GetComponent<MeshRenderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        /* 更新処理 */
        axis = orbitalTransposer.m_XAxis.Value; //　回転角を更新
        forkNumber = (int)(axis / QUARTER_CIRCLE); //　フォーク番号更新
        text.color = col[forkNumber]; //　テキスト色更新

        ResetAnimation(); //　リセット処理

        if (flyFlag) return;
        InputKeys(); //　その他のキー入力処理
    }

    private void InputKeys()
    {
        if (Input.GetKey(KeyCode.LeftArrow)) orbitalTransposer.m_XAxis.Value++; //　左回り
        if (Input.GetKey(KeyCode.RightArrow)) orbitalTransposer.m_XAxis.Value--; //　右回り
        if (Input.GetKey(KeyCode.Space))
        {
            animator[forkNumber].SetTrigger(eventName[forkNumber]); //フォークを刺す

            if (forkNumber == 1)
            {
                flyAway.SetTrigger("FlyAwayFlag"); //かまトゥ飛ぶ
                obj.SetActive(false);
                flyFlag = true;
            }
        }
    }

    private void ResetAnimation()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            obj.SetActive(true);
            animator[0].Play("Idle");
            animator[1].Play("Idle");
            animator[2].Play("Idle");
            animator[3].Play("Idle");
            flyAway.Play("Idle");
            orbitalTransposer.m_XAxis.Value = 0.0f;
            flyFlag = false;
        }
    }
}
