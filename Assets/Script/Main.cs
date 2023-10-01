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
    
    /*
        The hour hand moves 360 degrees / 12 in an hour = 30 degrees.
        It follows that in a minute it moves 30 degrees / 60 = 1/2 degree.
        Therefore in a second it moves 1/2 degrees / 60 = 1/120 degree. 
    */

    // Start is called before the first frame update
    void Start()
    {
        isStart = true;
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

    IEnumerator MoveClock()
    {
        while (isStart)
        {
            yield return new WaitForSecondsRealtime(1f);

            objSec.transform.Rotate(Vector3.back, 1f, Space.Self);

            idxSec++;

            if (idxSec == 60)
            {
                idxMin++;
                idxSec = 0;
            }
            else if (idxMin == 60)
            {
                idxHour++;
                idxSec = 0;
                idxMin = 0;
            }
            else if (idxHour == 13)
            {
                idxHour = 0;
                idxMin = 0;
                idxSec = 0;
            }
        }
    }
}