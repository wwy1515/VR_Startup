using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    private static GameMode s_instance;

    public TMPro.TextMeshProUGUI m_ScoreText;

    ScoreInterface scoreLogic;

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
        scoreLogic.Init();
    }

    // Update is called once per frame
    void Update()
    {
        scoreLogic.Update();
        m_ScoreText.text = scoreLogic.GetScore().ToString();
    }
}
