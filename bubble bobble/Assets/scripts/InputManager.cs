using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClick();
        }
    }

    void OnClick()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100))
        {
            //Debug.Log("hit " + hit.transform.name);

            ClickableObject clicked = hit.transform.GetComponent<ClickableObject>();
            if (clicked == null)
            {
                clicked = hit.transform.GetComponentInParent<ClickableObject>();
            }

            if (clicked != null)
            {
                clicked.OnClick(hit.transform.gameObject);
            }
        }
    }
}
