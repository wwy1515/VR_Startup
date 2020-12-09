using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeLoop : MonoBehaviour
{
    // Start is called before the first frame update
    private CanvasGroup canvasGroup;

    public static Transform MoveTips;
    public static Transform PickTips;
    public static Transform ShootTips;
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        MoveTips= transform.Find("MoveTips");
        PickTips = transform.Find("PickTips");
        ShootTips = transform.Find("ShootTips");
        ReInit();
    }

    public void ReInit()
    {
        timer = 0.0f;
        overAllTimer = 0.0f;
    }

    public static void StartPickTips()
    {
        PickTips.gameObject.SetActive(true);
    }

    public static void StartMoveTips()
    {
        MoveTips.gameObject.SetActive(true);
    }

    public static void StartShootTips()
    {
        ShootTips.gameObject.SetActive(true);
    }

    public static void CloseAll()
    {
        MoveTips.gameObject.SetActive(false);
        PickTips.gameObject.SetActive(false);
        ShootTips.gameObject.SetActive(false);
    }

    static float timer = 0.0f;
    static float overAllTimer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        overAllTimer += Time.deltaTime;
        timer += Time.deltaTime;
        if(timer >= 2.0)
        {
            timer = 0.0f;
        }

        if(timer <= 1.0f)
        {
            canvasGroup.alpha = Mathf.Max(timer, 0.1f);
        }
        else
        {
            canvasGroup.alpha = Mathf.Max(2.0f - timer, 0.1f); 
        }
    }
}
