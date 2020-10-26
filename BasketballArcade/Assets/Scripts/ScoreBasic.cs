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

    public void Update(Vector3 ball_location)
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            m_score = m_score + 1;
        }

        if ((ball_location - hoop_location).magnitude < 0.3f)
        {
            if (new_shot)
            {
                m_score += 1;
                new_shot = false;
            }
        }
        else
        {
            new_shot = true;
        }
    }
}
