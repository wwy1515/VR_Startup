using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    enum State
    {
        Inside,
        Outside,
    }

    State state = State.Outside;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponents<ScoreObserver>() != null)
        {
            if(transform.position.y > GameMode.GetInstance().targetPos.position.y)
            {
                state = State.Inside;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponents<ScoreObserver>() != null)
        {
            if(state == State.Inside)
            {
                state = State.Outside;
                if(transform.position.y < GameMode.GetInstance().targetPos.position.y)
                {
                    GameMode.GetInstance().scoreLogic.AddScore(1);
                }
            }
        }
    }

}
