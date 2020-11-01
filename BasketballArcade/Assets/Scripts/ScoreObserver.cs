using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreObserver : MonoBehaviour
{
    enum State
    {
        Inside,
        Outside,
        
    }

    class ScoreContext
    {
        State state = State.Outside;
        GameObject ball = null;
    }

    List<ScoreContext> scoreContexts = new List<ScoreContext>();




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
