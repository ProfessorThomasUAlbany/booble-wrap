using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField]
    private TMP_Text scoreCount;

    [SerializeField]
    private Transform sheetParent;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject bubbleWrapSheet = null;

    [Header("Anim stuff")]
    [SerializeField]
    float swapTime = 0.7f;
    [SerializeField]
    float swapDistance = 20f;
    [SerializeField]
    private AnimationCurve sheetEnterCurve;
    [SerializeField]
    private AnimationCurve sheetExitCurve;
    [SerializeField]
    private GameObject combo_assets;


    private int m_popCount = 0;
    private GameObject m_currentBubbleSheet = null;
    private GameObject m_currentBacking = null;
    private Renderer matRenderer = null;
    private bool rainbow = false;
    private float rainbow_timer = 0f;
    private int combo_count = 0;
    private AudioSource rainbow_music;


    private static GameManager s_instance;
    public static GameManager Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = FindAnyObjectByType<GameManager>();
                if (s_instance == null)
                {
                    GameObject gameManager = new GameObject();
                    gameManager.name = "Game Manager";
                    s_instance = gameManager.AddComponent<GameManager>();
                }
            }
            return s_instance;
        }
    }

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else if (s_instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreCount.text = "0";
        rainbow_music = GetComponent<AudioSource>();
    }

    void Update() 
    {
        if (rainbow) 
        {
            UpdateRainbow();
        }
    }

    private GameObject CreateBubbleWrapSheet()
    {
        GameObject newSheet = Instantiate(bubbleWrapSheet, sheetParent);
        newSheet.transform.localPosition = Vector3.zero;
        BubbleWrap bubbleWrapComponent = bubbleWrapSheet.GetComponent<BubbleWrap>();
        bubbleWrapComponent.onBubblePopped.AddListener(OnPop);
        bubbleWrapComponent.onSheetFinished.AddListener(OnFinishSheet);
        m_currentBacking = bubbleWrapComponent.getMaterial();
        matRenderer = m_currentBacking.GetComponent<Renderer>();
        

        return newSheet;
    }

    public void OnPop()
    {
        m_popCount++;
        scoreCount.text = m_popCount.ToString();
        combo_count++;
        rainbow_timer = 0;
        if (combo_count > 50 && !rainbow) 
        {
            StartRainbow();
        }
    }

    public void OnFinishSheet()
    {
        StartCoroutine(SwapSheets());
    }

    private IEnumerator SwapSheets()
    {
        // Little pause before we swap sheets
        yield return new WaitForSeconds(0.3f);

        GameObject newSheet = CreateBubbleWrapSheet();

        float currentSwapTime = 0f;

        while (currentSwapTime < swapTime)
        {
            float progress = Mathf.Clamp01(currentSwapTime / swapTime);

            Vector3 newSheetPos = new Vector3(0, sheetEnterCurve.Evaluate(progress) * swapDistance, 0);
            newSheet.transform.localPosition = newSheetPos;

            if (m_currentBubbleSheet != null)
            {
                Vector3 oldSheetPos = new Vector3(0, sheetExitCurve.Evaluate(progress) * swapDistance * -1, 0);
                m_currentBubbleSheet.transform.localPosition = oldSheetPos;
            }

            yield return null;
            currentSwapTime += Time.deltaTime;
        }

        newSheet.transform.localPosition = Vector3.zero;

        if (m_currentBubbleSheet != null)
        {
            Destroy(m_currentBubbleSheet);
        }
        m_currentBubbleSheet = newSheet;
    }

    private void StartRainbow () 
    {
        //print(m_currentBacking);
        matRenderer.sharedMaterial.color = Color.red;
        rainbow = true;
        rainbow_music.Play();
        combo_assets.SetActive(true);
    }

    private void UpdateRainbow() 
    {
        float H, S, V;

        Color.RGBToHSV(matRenderer.sharedMaterial.color, out H, out S, out V);
        if (H < 1) 
        {
            H += 0.5f * Time.deltaTime;
        }
        else 
        {
            H = 0;
        }
        matRenderer.sharedMaterial.color = Color.HSVToRGB(H,S,V);

        rainbow_timer += Time.deltaTime;
        if (rainbow_timer > 3) 
        {
            EndRainbow();
        }
    }

    private void EndRainbow() 
    {
        matRenderer.sharedMaterial.color = Color.white;
        rainbow = false;
        combo_count = 0;
        rainbow_music.Stop();
        combo_assets.SetActive(false);
    }

    public void EndGame() 
    {
        Application.Quit();
    }
}
