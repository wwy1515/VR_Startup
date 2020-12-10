using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    public float targetTime;
    public float remainTime;
    public int targetScore;
    public int currentScore;

    public void GenTask()
    {
        targetScore = (int)(Mathf.Max(1.0f, 8.0f * Random.value));
        targetTime = (8.0f + Random.value * 4.0f) * targetScore;

        currentScore = 0;
        remainTime = targetTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
