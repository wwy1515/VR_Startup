using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ShotClock : MonoBehaviour
{
    int clockTime = 24;
    float timeLeft = 1.0f;
    public Text clockText;
    int current_score;
    int compare_score;

    public TMPro.TextMeshProUGUI m_ScoreText;

    void setClockTime()
    {
        if (current_score > compare_score)//if basket made
        {
            compare_score = current_score;
            clockTime = 24;
            timeLeft = 1.0f;
        }
        else
        {
            if (clockTime > 0)
            {
                clockTime -= 1;
                timeLeft = 1.0f;
            }
            else if (clockTime == 0)
            {
                clockTime = 24;
                timeLeft = 1.0f;
            }
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        clockText.text = clockTime.ToString();//set shot clock to 24 sec
        current_score = int.Parse(m_ScoreText.text);
        compare_score = int.Parse(m_ScoreText.text);
        
    }


    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        current_score = int.Parse(m_ScoreText.text);
        if (timeLeft <= 0.0)
        {
            setClockTime();
            //Console.WriteLine(clockTime);
        }
        clockText.text = clockTime.ToString();

    }
}
