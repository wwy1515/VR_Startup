using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ScoreInterface
{
    void Init(Vector3 hoop_location);
    void Update(Vector3 ball_location);
    int GetScore();
}
