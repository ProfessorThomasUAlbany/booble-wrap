using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BubbleWrap : ClickableObject
{
    public UnityEvent onBubblePopped = new UnityEvent();
    public UnityEvent onSheetFinished = new UnityEvent();

    [SerializeField]
    private AudioSource aud;

    [SerializeField]
    private List<AudioClip> pops;

    private static List<GameObject> popSources = new List<GameObject>();

    [SerializeField]
    private GameObject popParticles = null;


    private List<GameObject> m_bubbles = new List<GameObject>();
    private int m_numBubbles = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.CompareTag("Bubble"))
            {
                m_bubbles.Add(child.gameObject);
            }
        }

        m_numBubbles = m_bubbles.Count;
    }

    public override void OnClick(GameObject clickedObject)
    {
        base.OnClick(clickedObject);

        if (clickedObject != null)
        {
            if (clickedObject.CompareTag("Bubble"))
            {
                POP(clickedObject);
            }
            else if (clickedObject.CompareTag("Wrap"))
            {
                 
            }
        }
    }

    private void POP(GameObject bubblePopped)
    {
        if (pops.Count > 0)
        {
            AudioClip pop = pops[Random.Range(0, pops.Count)];
            GameObject source = getPopSource();
            AudioSource popSource = source.GetComponent<AudioSource>();
            popSource.clip = pop;
            popSource.pitch = (Random.Range(0.8f, 1.2f));
            popSource.Play();
            Debug.Log("POP");
        }
        Instantiate(popParticles, bubblePopped.transform.position, bubblePopped.transform.rotation, transform);

        onBubblePopped.Invoke();
        GameManager.Instance.OnPop();
        if (m_bubbles.Contains(bubblePopped))
        {
            m_bubbles.Remove(bubblePopped);
            if (m_bubbles.Count == 0)
            {
                onSheetFinished.Invoke();
                GameManager.Instance.OnFinishSheet();
            }
        }

        bubblePopped.SetActive(false);
    }

    private static GameObject getPopSource()
    {
        GameObject popSource = null;
        foreach (GameObject popSourceObject in popSources)
        {
            AudioSource popAud = popSourceObject.GetComponent<AudioSource>();
            if (!popAud.isPlaying)
            {
                popSource = popSourceObject;
            }
        }

        if (popSource == null)
        {
            popSource = new GameObject();
            popSource.name = "popSource";
            popSource.AddComponent<AudioSource>();
            popSources.Add(popSource);
        }

        return popSource;
    }
}
