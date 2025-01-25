using System.Collections.Generic;
using UnityEngine;

public class BubbleWrap : ClickableObject
{
    [SerializeField]
    private AudioSource aud;

    [SerializeField]
    private List<AudioClip> pops;



    [SerializeField]
    private 


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

        Debug.Log("bubble time for " + clickedObject.tag);
        if (clickedObject != null)
        {
            if (clickedObject.CompareTag("Bubble"))
            {
                if (pops.Count > 0)
                {
                    AudioClip pop = pops[Random.Range(0, pops.Count)];
                    aud.PlayOneShot(pop, 1);
                    Debug.Log("POP");
                }
            }
            else if (clickedObject.CompareTag("Wrap"))
            {
                 
            }
        }
    }
}
