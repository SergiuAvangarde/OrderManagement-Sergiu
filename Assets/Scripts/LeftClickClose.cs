using UnityEngine;
using UnityEngine.EventSystems;


public class LeftClickClose : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            gameObject.SetActive(false);
        }
    }
}
