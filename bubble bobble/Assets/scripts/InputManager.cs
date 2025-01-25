using TMPro;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private TMP_Dropdown controlsDropdown;

    public enum PopControls
    {
        Click = 0,
        Drag = 1,
        Hover = 2
    }

    public PopControls controls = PopControls.Click;

    private void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }

        controlsDropdown.onValueChanged.AddListener(SetControls);
        controlsDropdown.SetValueWithoutNotify((int)controls);
    }

    // Update is called once per frame
    void Update()
    {
        switch(controls)
        {
            case PopControls.Click:
                if (Input.GetMouseButtonDown(0))
                {
                    OnClick();
                }
                break;

            case PopControls.Drag:
                if (Input.GetMouseButton(0))
                {
                    OnClick();
                }
                break; 

            case PopControls.Hover:
                OnClick();
                break;
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

    public void SetControls(int whatIsThisIntFor)
    {
        controls = (PopControls)controlsDropdown.value;
    }
}
