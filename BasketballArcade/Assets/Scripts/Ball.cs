using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public enum State
    {
        Inside,
        Outside,
        Scored,
        TheForce,
    }

    public State state = State.Outside;

    public AudioSource scoreSound;

    // Start is called before the first frame update
    void Start()
    {
        GameMode.GetInstance().balls.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -10.0f)
        {  
            GameMode.GetInstance().balls.Remove(this);
            Destroy(this.gameObject);
        }

        if(transform.position.y > GameMode.GetInstance().targetPos.transform.position.y && state == State.Outside)
        {
            Vector3 toHoop = (GameMode.GetInstance().targetPos.transform.position - this.transform.position).normalized;
            GetComponent<Rigidbody>().AddForce(toHoop * 1.2f, ForceMode.Force);
        }

        if(state == State.TheForce)
        {
            this.GetComponent<Rigidbody>().useGravity = false;
            Vector3 toHand = (GameMode.GetInstance().rightPick.transform.position - this.transform.position).normalized;
            GetComponent<Rigidbody>().AddForce(toHand * 4.0f, ForceMode.Force);

            if((GameMode.GetInstance().rightPick.transform.position - this.transform.position).sqrMagnitude < 0.01f)
            {
                state = State.Outside;
                this.GetComponent<Rigidbody>().useGravity = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponents<ScoreObserver>() != null)
        {
            if(state != State.Scored && transform.position.y > GameMode.GetInstance().targetPos.position.y)
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
                if(transform.position.y < GameMode.GetInstance().targetPos.position.y)
                {
                    state = State.Outside;
                    GameMode.GetInstance().scoreLogic.AddScore(1);
                    scoreSound.Play();
                }
                else
                {
                    state = State.Outside;
                }
            }
        }
    }

}
