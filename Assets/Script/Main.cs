using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    GameObject hour, min, sec;

    int secIndex, minIndex;


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