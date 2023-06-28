using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiSafeArea : MonoBehaviour
{
    public static UiSafeArea singletonInstance = null;
    RectTransform parentRtf;
    [SerializeField] TMPro.TextMeshProUGUI levelText;
    [SerializeField] Image goldImage;
    [SerializeField] TMPro.TextMeshProUGUI goldText;
    [SerializeField] GameObject endGameWin;
    [SerializeField] GameObject endGameLose;
    [SerializeField] Button endGameWinButton;
    [SerializeField] Button endGameLoseButton;
    [SerializeField] Image bannerAd;
    [SerializeField] Image fullscreenAd;
    [SerializeField] float buttonDelay = 2f;
    const int bannerAdW = 900;
    const int bannerAdH = 140;
    bool successOrFailShown;
    Global global;
    public float scaling = 1f;
    float animTime;
    bool animatingSuccess, animatingFail;

    void Awake()
    {
        singletonInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        /*** this script must under root Canvas ***/
        Debug.Assert(GetComponentInParent<Canvas>(), "parent is not Canvas");
        parentRtf = transform.parent?.GetComponent<RectTransform>();
        Debug.Assert(parentRtf != null, "parentRtf is null");
        
        Debug.Assert(bannerAd, "bannerAd is null");
        ApplySafeArea();

#if UNITY_EDITOR
        Application.targetFrameRate = 80;
#else
        Application.targetFrameRate = 60;
#endif

        global = FindObjectOfType<Global>();
        Debug.Assert(global, "global is null");
        Debug.Assert(levelText, "levelText not assigned");
        Debug.Assert(endGameWin, "endGameWin not assigned");
        Debug.Assert(endGameLose, "endGameLose not assigned");
        Debug.Assert(endGameWinButton, "endGameWinButton not assigned");
        Debug.Assert(endGameLoseButton, "endGameLoseButton not assigned");
        endGameWinButton.onClick.AddListener(EndGameWinButtonClicked);
        endGameLoseButton.onClick.AddListener(EndGameLoseButtonClicked);
        Debug.Assert(goldImage, "goldImage is null");
        Debug.Assert(goldText, "goldText is null");
        Debug.Assert(fullscreenAd, "fullscreenAd is null");
        
        goldText.text = PlayerPrefs.GetInt("gold", 0).ToString("N0");

        StartCoroutine(AfterGlobal());
        IEnumerator AfterGlobal()
        {
            yield return new WaitUntil(() => global.inited);
            levelText.text = global.SceneName;
        }
    }

    void ApplySafeArea()
    {
        print("screen w " + Screen.width + " h " + Screen.height);
        Rect safeAreaRect = Screen.safeArea;
        print("safeAreaRect " + safeAreaRect);
        // NOTE UI canvas (0,0) is at top-left, but phone safe area (0,0) is at bottom-left

        scaling = Screen.width / parentRtf.rect.width;
        print("canvas scaling ratio " + scaling);
        safeAreaRect.yMin += bannerAdH * scaling;
        print("safeAreaRect with banner ad " + safeAreaRect);

        var rtf = GetComponent<RectTransform>();
        //print("  -- rtf " + rtf.rect + "   omin " + rtf.offsetMin + " omax " + rtf.offsetMax);
        var left = safeAreaRect.xMin / scaling;
        var right = ( Screen.width - safeAreaRect.xMax ) / scaling;
        var top = ( Screen.height - safeAreaRect.yMax ) / scaling;
        var bottom = safeAreaRect.yMin / scaling;
        //print("  top " + top + " left " + left + " right " + right + " bottom " + bottom);
        rtf.offsetMin = new Vector2( left, bottom );
        rtf.offsetMax = new Vector2( -right, -top );
        //print("  ++ rtf " + rtf.rect + "   omin " + rtf.offsetMin + " omax " + rtf.offsetMax);

        bannerAd.rectTransform.anchoredPosition = new Vector2(0f, bannerAdH / 2);
        bannerAd.rectTransform.sizeDelta = new Vector2(bannerAdW, bannerAdH);
        bannerAd.rectTransform.anchoredPosition = new Vector2(0f, rtf.offsetMin.y - bannerAd.rectTransform.anchoredPosition.y);
        print("bannerAd pos " + bannerAd.rectTransform.anchoredPosition + " size " + bannerAd.rectTransform.sizeDelta);
    }

    /*
    https://gist.github.com/SeanMcTex/c28f6e56b803cdda8ed7acb1b0db6f82

    private void Update() {
        if ( lastSafeArea != Screen.safeArea ) {
            ApplySafeArea();
        }
    }

    private void ApplySafeArea() {
        Rect safeAreaRect = Screen.safeArea;

        float scaleRatio = parentRectTransform.rect.width / Screen.width;

        var left = safeAreaRect.xMin * scaleRatio;
        var right = -( Screen.width - safeAreaRect.xMax ) * scaleRatio;
        var top = -safeAreaRect.yMin * scaleRatio;
        var bottom = ( Screen.height - safeAreaRect.yMax ) * scaleRatio;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.offsetMin = new Vector2( left, bottom );
        rectTransform.offsetMax = new Vector2( right, top );

        lastSafeArea = Screen.safeArea;
    }
    */

    public void Win()
    {
        endGameWin.SetActive(true);
        StartCoroutine(EnableButton());
        IEnumerator EnableButton()
        {
            yield return new WaitForSeconds(buttonDelay);
            endGameWinButton.gameObject.SetActive(true);
        }
    }

    public void Lose()
    {
        endGameLose.SetActive(true);
        StartCoroutine(EnableButton());
        IEnumerator EnableButton()
        {
            yield return new WaitForSeconds(buttonDelay);
            endGameLoseButton.gameObject.SetActive(true);
        }
    }

    void EndGameWinButtonClicked()
    {
        global.LoadScene(true);
    }

    void EndGameLoseButtonClicked()
    {
        global.LoadScene(false);
    }
}
