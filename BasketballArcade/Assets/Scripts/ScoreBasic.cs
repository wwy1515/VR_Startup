using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBasic : ScoreInterface
{
    int m_score = 0;
    public void Init()
    {
        m_score = 0;
    }

    public int GetScore()
    {
        return m_score;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            m_score = m_score + 1;
        }
    }
}
