using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    GameObject hour, min, sec;

    int idxHour, idxMin, idxSec;
    
    /*
        The hour hand moves 360 degrees / 12 in an hour = 30 degrees.
        It follows that in a minute it moves 30 degrees / 60 = 1/2 degree.
        Therefore in a second it moves 1/2 degrees / 60 = 1/120 degree. 
    */

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveClock());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
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
        yield return new WaitForSecondsRealtime(1f);

        secIndex++;

        if(secIndex == 60)
        {
            minIndex++;
            secIndex = 0;
        }


    }
}