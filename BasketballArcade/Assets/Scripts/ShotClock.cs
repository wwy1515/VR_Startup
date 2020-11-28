using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotClock : MonoBehaviour
{
    int clockTime = 24;
    float timeLeft = 1.0f;
    public Text clockText;

    void setClockTime()
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

    // Start is called before the first frame update
    void Start()
    {
        clockText.text = clockTime.ToString();//set shot clock to 24 sec
    }


    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0.0)
        {
            setClockTime();
            //Console.WriteLine(clockTime);
        }
        clockText.text = clockTime.ToString();
    }
}
