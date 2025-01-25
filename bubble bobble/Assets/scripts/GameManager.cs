using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField]
    private TMP_Text scoreCount;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject bubbleWrapSheet = null;


    private int popCount = 0;


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
        CreateBubbleWrapSheet();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateBubbleWrapSheet()
    {
        Instantiate(bubbleWrapSheet);
        BubbleWrap bubbleWrapComponent = bubbleWrapSheet.GetComponent<BubbleWrap>();
        bubbleWrapComponent.onBubblePopped.AddListener(OnPop);
        bubbleWrapComponent.onSheetFinished.AddListener(OnFinishSheet);
    }

    public void OnPop()
    {
        popCount++;
        scoreCount.text = popCount.ToString();
    }

    public void OnFinishSheet()
    {
        //TODO
    }
}
