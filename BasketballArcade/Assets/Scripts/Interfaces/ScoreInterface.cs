using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ScoreInterface
{
    void Init(Vector3 hoop_location);
    void Update(GameObject ball);
    void AddScore(int val);
    int GetScore();
}
