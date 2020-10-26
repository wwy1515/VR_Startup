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

    public Vector3 hoopLocation;
    public GameObject ballPrefab;
    public GameObject handledBall;

    ScoreInterface scoreLogic;
    PickInterface pickLogic;

    public enum ENVIRONMENT_TYPE
    {
        PC,
        VR
    }

    public ENVIRONMENT_TYPE enviromentType = ENVIRONMENT_TYPE.PC;

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
        scoreLogic.Init(hoopLocation);

        pickLogic = new PickBasic();
        pickLogic.Init();
    }

    // Update is called once per frame
    void Update()
    {
        scoreLogic.Update(GameObject.Find(ballPrefab.name).transform.position);
        m_ScoreText.text = scoreLogic.GetScore().ToString();

        pickLogic.Update();
    }
}
