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
        //�@�e�R���|�[�l���g���擾
        virtualCamera = obj.GetComponent<CinemachineVirtualCamera>();
        orbitalTransposer = virtualCamera.GetComponentInChildren<CinemachineOrbitalTransposer>();
        text = textObj.GetComponent<Text>();

        //�@�eForkObject��Animator���擾
        foreach (var (v, i) in fork.Select((v, i) => (v, i)))
            animator[i] = v.GetComponent<Animator>();

        flyAway = kamatoo.GetComponent<Animator>();

        //�@�e�t�H�[�N�̐F�}�e���A���𕶎��F�Ɋ��蓖��
        for (int i = 0; i < FORK_QUANTITY; ++i)
            col[i] = fork[i].GetComponent<MeshRenderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        /* �X�V���� */
        axis = orbitalTransposer.m_XAxis.Value; //�@��]�p���X�V
        forkNumber = (int)(axis / QUARTER_CIRCLE); //�@�t�H�[�N�ԍ��X�V
        text.color = col[forkNumber]; //�@�e�L�X�g�F�X�V

        ResetAnimation(); //�@���Z�b�g����

        if (flyFlag) return;
        InputKeys(); //�@���̑��̃L�[���͏���
    }

    private void InputKeys()
    {
        if (Input.GetKey(KeyCode.LeftArrow)) orbitalTransposer.m_XAxis.Value++; //�@�����
        if (Input.GetKey(KeyCode.RightArrow)) orbitalTransposer.m_XAxis.Value--; //�@�E���
        if (Input.GetKey(KeyCode.Space))
        {
            animator[forkNumber].SetTrigger(eventName[forkNumber]); //�t�H�[�N���h��

            if (forkNumber == 1)
            {
                flyAway.SetTrigger("FlyAwayFlag"); //���܃g�D���
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
