using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    private static GameMode s_instance;

    public TMPro.TextMeshProUGUI m_ScoreText;

    public PlayerController playerController;
    public Transform LeftHand;
    public Transform RightHand;

    public Transform hoop;
    public GameObject ballPrefab;
    public GameObject handledBall;

    ScoreInterface scoreLogic;
    PickInterface pickLogic;

    public enum ENVIRONMENT_TYPE
    {
        PC,
        VR
    }

    public ENVIRONMENT_TYPE enviromentType = ENVIRONMENT_TYPE.VR;

    public static GameMode GetInstance()
    {
        return s_instance;
    }

    void Awake()
    {
        s_instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreLogic = new ScoreBasic();
        scoreLogic.Init(hoop.position);

        pickLogic = new PickBasic();
        pickLogic.Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (handledBall)
        {
            scoreLogic.Update(handledBall);
        }
        m_ScoreText.text = scoreLogic.GetScore().ToString();

        pickLogic.Update();
    }
}
