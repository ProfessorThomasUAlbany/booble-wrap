using System.Collections.Generic;
using UnityEngine;

public class BubbleWrap : ClickableObject
{
    [SerializeField]
    private AudioSource aud;

    [SerializeField]
    private List<AudioClip> pops;

    private List<GameObject> popSources = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        bubblePopped.SetActive(false);
    }

    private GameObject getPopSource()
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
