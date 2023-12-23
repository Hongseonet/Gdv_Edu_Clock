using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    [SerializeField]
    GameObject objHour, objMin, objSec;

    [SerializeField]
    TextMeshProUGUI txtTime;

    [SerializeField]
    Transform btnCtrTime;

    [SerializeField]
    Button btnClose, btnTTS;

    AudioSource audioSource;

    Vector3 curTime; //hour min sec
    bool isQuit;
    
    /*
        The hour hand moves 360 degrees / 12 in an hour = 30 degrees.
        It follows that in a minute it moves 30 degrees / 60 = 1/2 degree.
        Therefore in a second it moves 1/2 degrees / 60 = 1/120 degree. 
    */

    // Start is called before the first frame update
    void Start()
    {
        curTime = new Vector3(); //init time

        if (this.GetComponent<AudioSource>() == null)
            this.gameObject.AddComponent<AudioSource>();

        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        //btn search 2 depth, add listener
        foreach (Transform depth1 in btnCtrTime)
        {
            if (depth1.GetComponent<Button>() != null)
            {
                Button btn = depth1.GetComponent<Button>();
                btn.onClick.AddListener(() => BtnEvent(btn));
            }

            foreach(Transform depth2 in depth1)
            {
                if(depth2.GetComponent<Button> () != null)
                {
                    //Debug.Log("btn regist : " + depth1.name + " / " + depth2.name);
                    Button btn2 = depth2.GetComponent<Button>();
                    btn2.onClick.AddListener(() => BtnEvent(btn2));
                }
            }
        }

        btnClose.onClick.AddListener(() => BtnEvent(btnClose));
        btnTTS.onClick.AddListener(() => BtnEvent(btnTTS));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(QuitApp());
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            SetTimer('s', true);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            SetTimer('m', true);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            SetTimer('h', true);
        }
    }

    IEnumerator QuitApp()
    {
        if (isQuit)
        {
            StopAllCoroutines();
            Application.Quit();
        }

        isQuit = !isQuit;
        yield return new WaitForSecondsRealtime(2f); //release after 2 sec
        isQuit = !isQuit;
    }

    void BtnEvent(Button btn){
        //Debug.Log("dd " + btn.name);

        if (btn.transform.parent.name.Split('_').Length > 1)
        {
            switch (btn.transform.parent.name.Split('_')[1])
            {
                case "Hour":
                    if (btn.name.Split('_')[1].Equals("Increase"))
                    {
                        SetTimer('h', true);
                    }
                    else
                    {
                        SetTimer('h', false);
                    }
                    break;
                case "Min":
                    if (btn.name.Split('_')[1].Equals("Increase"))
                    {
                        SetTimer('m', true);
                    }
                    else
                    {
                        SetTimer('m', false);
                    }
                    break;
                case "Sec":
                    if (btn.name.Split('_')[1].Equals("Increase"))
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
        
        switch (btn.name.Split('_')[1])
        {
            case "Random":
                System.Random rand = new System.Random();
                curTime = new Vector3(rand.Next(1, 12), rand.Next(0, 60), rand.Next(0, 60));
                txtTime.text = string.Format("{0:D2}", (int)curTime.x) + " : " + string.Format("{0:D2}", (int)curTime.y) + " : " + string.Format("{0:D2}", (int)curTime.z);
                
                InitNiddle('a'); //reset all

                objHour.transform.Rotate(Vector3.back, (30f * curTime.x) + (0.5f * curTime.y), Space.Self);
                objMin.transform.Rotate(Vector3.back, 6f * curTime.y, Space.Self);
                objSec.transform.Rotate(Vector3.back, 6f * curTime.z, Space.Self);
                break;
            case "Close":
                StartCoroutine(QuitApp());
                break;
            case "TTS":
                Debug.Log("cur time : " + curTime);
                string filePath = "";

                if (Application.platform.Equals(RuntimePlatform.Android))
                {
                    //filePath = "jar:file://" + Application.dataPath + "!/assets";
                    filePath = "file:///storage/emulated/0";
                }
                else if (Application.platform.Equals(RuntimePlatform.WindowsEditor))
                {
                    filePath = Application.streamingAssetsPath;
                }
            //else
                //filePath = "file://" + Application.streamingAssetsPath;

                StartCoroutine(AudioPlay(Path.Combine(filePath + "/TTS/Hour/ddes.wav")));
                //string filePath = Application.streamingAssetsPath + "/TTS/Hour/ddes";
                
                break;
        }
    }

    IEnumerator AudioPlay(string filePath)
    {
        Debug.Log("fi : " + filePath);

        if (!File.Exists(filePath)){
            Debug.LogWarning("no file");
            yield break;
        }

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.UNKNOWN))
        //using (UnityWebRequest www = UnityWebRequest.Get(filePath))
        {
            Debug.Log("psf : " + www.url);

            yield return www.SendWebRequest();

            while (!www.isDone) { }

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = myClip;
                audioSource.Play();
            }
        }
    }

    void SetTimer(char n, bool isForward)
    {
        Vector3 direct = isForward ? Vector3.back : Vector3.forward;
        int weight = 0;
        if (isForward)
        {
            weight++;
        }
        else
        {
            weight--;
        }
        switch (n)
        {
            case 'h': //hour
                InitNiddle('h'); //temporary reset
                if(isForward)
                    curTime.x += weight;
                objHour.transform.Rotate(direct, (30f * curTime.x) + (0.5f * curTime.y), Space.Self);
                break;
            case 'm': //minute
                curTime.y += weight;
                objHour.transform.Rotate(direct, 0.5f, Space.Self);
                objMin.transform.Rotate(direct, 6f, Space.Self);
                break;
            case 's': //second
                curTime.z += weight;
                objSec.transform.Rotate(direct, 6f, Space.Self);
                break;
        }

        //positive
        if(curTime.z > 59)
        {
            curTime.y++;
            curTime.z = 0;
        }
        else if(curTime.y > 59)
        {
            curTime.x++;
            curTime.y = 0;
        }
        else if(curTime.x > 12)
        {
            curTime.x = 1;
        }
        //negative
        else if(curTime.z < 0)
        {
            curTime.z = 59;
        }
        else if(curTime.y < 0)
        {
            curTime.y = 59;
        }
        else if(curTime.x < 0)
        {
            curTime.x = 11;
        }

        txtTime.text = string.Format("{0:D2}", (int)curTime.x) + " : " + string.Format("{0:D2}", (int)curTime.y) + " : " + string.Format("{0:D2}", (int)curTime.z);
    }

    IEnumerator WiseSmooth(GameObject target, float startDegree, float endDegree)
    {
        float abc = Mathf.Lerp(startDegree, endDegree, 1f);

        yield return new WaitForSecondsRealtime(2f);
    }

    void InitNiddle(char idx)
    {
        switch (idx)
        {
            case 'h':
                objHour.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                break;
            case 'm':
                objMin.transform.localEulerAngles = new Vector3(0f, 0f, 90f); 
                break;
            case 's':
                objSec.transform.localEulerAngles = new Vector3(0f, 0f, 90f); 
                break;
            default:
                objHour.transform.localEulerAngles = new Vector3(0f, 0f, 90f); 
                objMin.transform.localEulerAngles = new Vector3(0f, 0f, 90f); 
                objSec.transform.localEulerAngles = new Vector3(0f, 0f, 90f); 
                break;
        }
    }
}