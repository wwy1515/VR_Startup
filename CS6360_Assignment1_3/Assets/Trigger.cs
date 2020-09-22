using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public List<Trigger> OtherLights;
    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 3.0f)
        {
            Terminate();
        }
    }

    void Terminate()
    {
        timer = 0.0f;

        gameObject.SetActive(false);
        if (Random.value > 0.5)
        {
            OtherLights[0].gameObject.SetActive(true);
        }
        else
        {
            OtherLights[1].gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(GameMode.playerController.playerControllerPlatform.HandleInteractiveButton())
        {
            GameMode.Score += 1;
            GameObject.Find("txtScore").GetComponent<UnityEngine.UI.Text>().text = GameMode.Score.ToString();
            Terminate();
        }
    }
}
