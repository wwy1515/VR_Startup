using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBasic : ScoreInterface
{
    int m_score = 0;
    public Vector3 hoop_location;
    bool new_shot;

    public void Init(Vector3 hoop_location)
    {
        this.hoop_location = hoop_location;
        m_score = 0;
        new_shot = true;
    }

    public int GetScore()
    {
        return m_score;
    }

    public void Update(GameObject ball)
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            m_score = m_score + 1;
        }
    }

    public void AddScore(int val)
    {
        m_score += val;
    }
}
