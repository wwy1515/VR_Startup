using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float value = 0.0f;
    public Transform body;
    public Transform head;

    public Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(value <= 0.3f)
        {
            body.gameObject.SetActive(false);
            head.gameObject.SetActive(false);
        }
        else
        {
            body.gameObject.SetActive(true);
            head.gameObject.SetActive(true);
        }

        body.localPosition = new Vector3(0.0f, 0.0f,value * 0.1f);
        body.localScale = new Vector3(0.1f, value * 0.1f, 0.1f);

        head.localPosition = new Vector3(0.0f, 0.0f, value * 0.2f);  

        this.transform.LookAt(this.transform.position + direction);
    }
}
