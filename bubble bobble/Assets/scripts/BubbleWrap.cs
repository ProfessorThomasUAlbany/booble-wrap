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
    [SerializeField]
    private GameObject poppedReplacement = null;

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

    private void Update()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * 5f);
        Vector3 pushedVector = transform.localPosition;
        pushedVector.z = Mathf.Lerp(pushedVector.z, 0, Time.deltaTime * 5f);
        transform.localPosition = pushedVector;
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
        if (poppedReplacement  != null)
        {
            Instantiate(poppedReplacement, bubblePopped.transform.position, bubblePopped.transform.rotation, transform);
        }
        SheetShake(bubblePopped.transform.position);

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

    private void SheetShake(Vector3 bubblePoppedPosition)
    {
        // The math here is bad and doesn't account for how the sheet's parent is rotated.
        // There's probably a way to do this with local transform stuff but oh well
        Vector3 lineToBubble = (bubblePoppedPosition - transform.position).normalized;
        Vector3 perpendicularLine = new Vector3(lineToBubble.z, lineToBubble.x, 0);
        Debug.Log(lineToBubble + ", " + perpendicularLine);

        transform.Rotate(perpendicularLine, -1f * lineToBubble.magnitude);

        Vector3 pushedVector = transform.localPosition;
        pushedVector.z = lineToBubble.magnitude * 0.15f;
        transform.localPosition = pushedVector;
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
