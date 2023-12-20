using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Main : MonoBehaviour
{
    [SerializeField]
    GameObject objHour, objMin, objSec;

    [SerializeField]
    TextMeshPro txtTime;

    [SerializeField]
    Transform btnRoot;

    bool isStart;
    int idxHour, idxMin, idxSec;
    int jumpGap, secJump;
    
    /*
        The hour hand moves 360 degrees / 12 in an hour = 30 degrees.
        It follows that in a minute it moves 30 degrees / 60 = 1/2 degree.
        Therefore in a second it moves 1/2 degrees / 60 = 1/120 degree. 
    */

    // Start is called before the first frame update
    void Start()
    {
        isStart = true;
        secJump = 90; //1 or Dev
        jumpGap = 15; //1 or Dev

        //btn search 2 depth, add listener
        foreach(Transform depth1 in btnRoot){
            foreach(Transform depth2 in depth1)
            {
                if(depth2.GetComponent<Button> () != null)
                {
                    //Debug.Log("dd : " + depth1.name + " / " + depth2.name);
                    Button btn = depth2.GetComponent<Button>();
                    btn.onClick.AddListener(() => BtnEvent(btn));
                }
            }
        }

        //StartCoroutine(MoveClock());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //isStart = false;
            //Application.Quit();
        }
    }

    void BtnEvent(Button btn){

        switch (btn.transform.parent.name.Split('_')[1].ToLower())
        {
            case "hour":
                if (btn.name.Split('_')[1].ToLower().Equals("inscrse"))
                {
                    SetTimer('h', true);
                }
                else
                {
                    SetTimer('h', false);
                }
                break;
            case "min":
                if (btn.name.Split('_')[1].ToLower().Equals("inscrse"))
                {
                    SetTimer('m', true);
                }
                else
                {
                    SetTimer('m', false);
                }
                break;
            case "sec":
                if (btn.name.Split('_')[1].ToLower().Equals("inscrse"))
                {
                    SetTimer('s', true);
                }
                else
                {
                    SetTimer('s', false);
                }
                break;
        }
    }

    void SetTimer(char n, bool isForward)
    {
        Vector3 direct = isForward ? Vector3.back : Vector3.forward;
            
        switch (n)
        {
            case 'h': //hour
                objHour.transform.Rotate(direct, 30f, Space.Self);
                break;
            case 'm': //minute
                objMin.transform.Rotate(direct, 6f, Space.Self);
                break;
            case 's': //second
                objSec.transform.Rotate(direct, 6f, Space.Self);
                break;
        }
    }

    IEnumerator MoveClock()
    {
        while (isStart)
        {
            yield return new WaitForSecondsRealtime(1);
            objSec.transform.Rotate(Vector3.back, secJump, Space.Self);
            idxSec += jumpGap;

            if (idxSec > 59)
            {
                idxMin++;
                idxSec = 0;
                SetTimer('m', true);
            }
            else if (idxMin >= 60)
            {
                idxHour++;
                idxSec = 0;
                idxMin = 0;
                SetTimer('h', true);
            }
            else if (idxHour == 13)
            {
                idxHour = 0;
                idxMin = 0;
                idxSec = 0;
            }
        }
    }

    IEnumerator MovingSmooth(GameObject target, float startDegree, float endDegree)
    {
        float abc = Mathf.Lerp(startDegree, endDegree, 1f);

        yield return new WaitForSecondsRealtime(2f);
    }

    void BtnEvnet(Button btn)
    {
        switch (btn.name.Split('_'))
        {

        }
    }
}