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
}