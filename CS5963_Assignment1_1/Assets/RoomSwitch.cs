using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSwitch : MonoBehaviour
{
    public Transform TransportAnchor;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        // Transport Position
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.gameObject.SetActive(false);
            transform.position = TransportAnchor.position;
            this.gameObject.SetActive(true);
        }
    }
}
