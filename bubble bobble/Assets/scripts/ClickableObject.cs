using UnityEngine;
using UnityEngine.Events;

public class ClickableObject : MonoBehaviour
{
    public UnityEvent onClicked;

    public virtual void OnClick(GameObject clickedObject)
    {
        onClicked.Invoke();
    }
}
