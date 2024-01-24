using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    [SerializeField]
    bool isDev;

    [SerializeField]
    GameObject objHour, objMin, objSec;

    [SerializeField]
    TextMeshProUGUI txtTime;

    [SerializeField]
    Transform btnCtrTime;

    bool isThemeDark, isTTSPlay;

    AudioSource audioSource;

    Vector3 curTime; //hour min sec
    Queue<AudioClip> queueAudio; //hour min min sec sec

    /*
        The hour hand moves 360 degrees / 12 in an hour = 30 degrees.
        It follows that in a minute it moves 30 degrees / 60 = 1/2 degree.
        Therefore in a second it moves 1/2 degrees / 60 = 1/120 degree. 
    */

    // Start is called before the first frame update
    void Start()
    {
        isThemeDark = true; //default dark

        curTime = new Vector3(); //init time
        queueAudio = new Queue<AudioClip>();

        if (this.GetComponent<AudioSource>() == null)
            this.gameObject.AddComponent<AudioSource>();

        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        InitNiddle('a');

        //btn search 2 depth, add listener
        foreach (Transform depth1 in btnCtrTime)
        {
            if (depth1.GetComponent<Button>() != null)
            {
                Button btn = depth1.GetComponent<Button>();
                btn.onClick.AddListener(() => BtnEvent(btn));
            }

            foreach (Transform depth2 in depth1)
            {
                if (depth2.GetComponent<Button>() != null)
                {
                    //Debug.Log("btn regist : " + depth1.name + " / " + depth2.name);
                    Button btn2 = depth2.GetComponent<Button>();
                    btn2.onClick.AddListener(() => BtnEvent(btn2));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(Common.Instance.QuitApp());
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

    void BtnEvent(Button btn) {
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
            case "CurTime":
                InitNiddle('a'); //reset all

                //string strTime = DateTime.Now.ToString("HH:mm:ss"); //24
                string[] strTime = DateTime.Now.ToString("hh:mm:ss").Split(':'); //12
                curTime = new Vector3(float.Parse(strTime[0]), float.Parse(strTime[1]), float.Parse(strTime[2]));
                txtTime.text = string.Format("{0:D2}", (int)curTime.x) + ":" + string.Format("{0:D2}", (int)curTime.y) + ":" + string.Format("{0:D2}", (int)curTime.z);

                objHour.transform.Rotate(Vector3.back, (30f * curTime.x) + (0.5f * curTime.y), Space.Self);
                objMin.transform.Rotate(Vector3.back, 6f * curTime.y, Space.Self);
                objSec.transform.Rotate(Vector3.back, 6f * curTime.z, Space.Self);

                break;
            case "Random":
                InitNiddle('a'); //reset all

                System.Random rand = new System.Random();
                curTime = new Vector3(rand.Next(1, 12), rand.Next(0, 60), rand.Next(0, 60));
                txtTime.text = string.Format("{0:D2}", (int)curTime.x) + ":" + string.Format("{0:D2}", (int)curTime.y) + ":" + string.Format("{0:D2}", (int)curTime.z);

                objHour.transform.Rotate(Vector3.back, (30f * curTime.x) + (0.5f * curTime.y), Space.Self);
                objMin.transform.Rotate(Vector3.back, 6f * curTime.y, Space.Self);
                objSec.transform.Rotate(Vector3.back, 6f * curTime.z, Space.Self);
                break;
            case "Close":
                StartCoroutine(Common.Instance.QuitApp());
                break;
            case "TTS":
                //Debug.Log("cur time : " + curTime);
                string filePath = "";
                string ttsTime = "", ttsType = ""; //num 1 2 3 / type min sec

                if (Application.platform.Equals(RuntimePlatform.Android))
                {
                    //filePath = "jar:file://" + Application.dataPath + "!/assets/";
                    filePath = "jar:file://" + Application.streamingAssetsPath;
                }
                else if (Application.platform.Equals(RuntimePlatform.WindowsEditor))
                {
                    filePath = Application.streamingAssetsPath;
                }

                for (int i = 0; i < 3; i++) //tts speak hour min sec
                {
                    switch (i)
                    {
                        case 0: //hour
                            ttsTime = "narr_hour_" + curTime.x.ToString();
                            ttsType = "";
                            if (curTime.x == 0 || curTime.x == 12)
                                ttsTime = "narr_hour_12";

                            //StartCoroutine(GetTTS(Path.Combine(filePath + "/TTS/Hour/" + ttsTime + ".mp3")));
                            GetTTS(Path.Combine(filePath + "/TTS/Hour/" + ttsTime + ".wav"));
                            break;
                        default: //min sec
                            string typeIdx = i == 1 ? "min" : "sec";
                            ttsTime = i == 1 ? "narr_" + curTime.y.ToString() : "narr_" + curTime.z.ToString();
                            ttsType = "narr_" + typeIdx;

                            //exception 0 min 0 sec
                            //StartCoroutine(GetTTS(Path.Combine(filePath + "/TTS/Min_Sec/" + ttsTime + ".mp3")));
                            GetTTS(Path.Combine(filePath + "/TTS/Min_Sec/" + ttsTime + ".wav"));
                            if (curTime.y != 0 || curTime.z != 0)
                                //StartCoroutine(GetTTS(Path.Combine(filePath + "/TTS/Min_Sec/" + ttsType + ".mp3")));
                                GetTTS(Path.Combine(filePath + "/TTS/Min_Sec/" + ttsType + ".wav"));
                            break;
                    }
                }
                StartCoroutine(PlayTTS());
                break;
            case "Theme":
                SetTheme();

                break;
        }
    }

    void SetTheme()
    {
        isThemeDark = !isThemeDark; //theme toggle
        string imgTheme;

        imgTheme = isThemeDark ? "theme_black" : "theme_white";
        //btnTheme.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/" + imgTheme);

        objHour.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/" + imgTheme);
        objHour.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/" + imgTheme);
        objHour.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/" + imgTheme);
    }

    //IEnumerator GetTTS(string filePath)
    void GetTTS(string filePath)
    {
        //Debug.Log("filePath : " + filePath);

        if (Application.platform.Equals(RuntimePlatform.WindowsEditor))
        {
            if (!File.Exists(filePath))
            {
                Debug.LogWarning("no file exist");
                //yield break;
                //return;
            }
        }

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.UNKNOWN))
        //using (UnityWebRequest www = UnityWebRequest.Get(filePath))
        {
            //Debug.Log("psf : " + www.url);
            //yield return www.SendWebRequest();
            www.SendWebRequest();

            while (!www.isDone) { }

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                queueAudio.Enqueue(DownloadHandlerAudioClip.GetContent(www));
            }
        }
    }

    IEnumerator PlayTTS()
    {
        if (!isTTSPlay)
        {
            isTTSPlay = true; //toggle

            int loopCnt = queueAudio.Count; //for fix total count
            //Debug.Log("cnt : " + queueAudio.Count);

            for (int i = 0; i < loopCnt; i++)
            {
                audioSource.clip = queueAudio.Dequeue();
                //Debug.Log("length " + i + " / " + audioSource.clip.length);
                audioSource.Play();
                yield return new WaitForSecondsRealtime(audioSource.clip.length);
                audioSource.Stop();
            }
            queueAudio.Clear();

            isTTSPlay = false;
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

        txtTime.text = string.Format("{0:D2}", (int)curTime.x) + ":" + string.Format("{0:D2}", (int)curTime.y) + ":" + string.Format("{0:D2}", (int)curTime.z);
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

        curTime = Vector3.zero;

        txtTime.text = string.Format("{0:D2}", (int)curTime.x) + ":" + string.Format("{0:D2}", (int)curTime.y) + ":" + string.Format("{0:D2}", (int)curTime.z);
    }
}