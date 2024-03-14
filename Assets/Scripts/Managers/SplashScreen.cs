using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    public GameObject permanentObject;
    public string nextScene;
    public SplashLogo[] logos;
    public Image background;
    public Color defaultColor;
    int logo;
    float time;

    void Awake()
    {
        background.color = defaultColor;
        foreach (var l in logos)
            l.logoImage.color = new Color(1, 1, 1, 0);
    }
    void Start()
    {
        DontDestroyOnLoad(Instantiate(permanentObject));
    }
    void Update()
    {
        if (logo == logos.Length)
            return;
        time += Time.deltaTime;
        while (time >= logos[logo].transitionTime * 2 + logos[logo].waitTime) {
            time -= logos[logo].transitionTime * 2 + logos[logo].waitTime;
            logos[logo].logoImage.color = new Color(1, 1, 1, 0);
            logo++;
            if (logo == logos.Length)
            {
                SceneManager.LoadSceneAsync(nextScene);
                return;
            }
        }
        var enter = time < logos[logo].transitionTime;
        var exit = time > logos[logo].transitionTime + logos[logo].waitTime;
        bool end = (enter && logo == 0) || (exit && logo == logos.Length - 1);
        var fade = enter || exit ? (enter
                    ? time
                    : (logos[logo].transitionTime - time + logos[logo].transitionTime + logos[logo].waitTime))
                / logos[logo].transitionTime
                : 1;
        background.color = enter || exit
            ? Color.Lerp(
                end ? defaultColor : logos[logo - 1].backColor,
                logos[logo].backColor,
                fade * (end ? 1 : 0.5f))
            : logos[logo].backColor;
        logos[logo].logoImage.color = new Color(1, 1, 1, fade);


    }

    [Serializable]
    public class SplashLogo
    {
        public Color backColor;
        public Image logoImage;
        public float transitionTime;
        public float waitTime;
    }
}
