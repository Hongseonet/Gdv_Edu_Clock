using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Common : MonoSingleton<Common>
{
    [HideInInspector]
    public bool isQuit;

    public override void Init() { }

    public void SetImage(Image target, bool isResource, string resourcePath)
    {
        if (isResource)
        {
            target.sprite = Resources.Load<Sprite>(resourcePath);
        }
        else
        {

        }
    }

    public void SetImage(RawImage target, bool isResource, string resourcePath) 
    {

    }

    public IEnumerator QuitApp()
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

    public void SetLog(char idx, string header, string msg)
    {
        switch (idx)
        {
            case 'w':
                Debug.LogWarning(header + " / " + msg);
                break;
            case 'e':
                Debug.LogError(header + " / " + msg);
                break;
            default:
                Debug.Log(header + " / " + msg);
                break;
        }
    }
}