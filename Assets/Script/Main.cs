using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    [SerializeField]
    GameObject objHour, objMin, objSec;

    [SerializeField]
    TextMeshPro txtTime;

    [SerializeField]
    Button btnA;

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
        StartCoroutine(MoveClock());
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

    void MoveNIddle(bool isAuto, GameObject target)
    {
        
        if (isAuto)
        {

        }
        else
        {

        }
    }

    void RotateNiddle(char n)
    {
        switch (n)
        {
            case 'h': //hour

                break;
            case 'm': //minute
                objMin.transform.Rotate(Vector3.back, 6f, Space.Self);
                break;
            case 's': //second
                //not stop ever
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
                RotateNiddle('m');
            }
            else if (idxMin >= 60)
            {
                idxHour++;
                idxSec = 0;
                idxMin = 0;
                RotateNiddle('h');
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