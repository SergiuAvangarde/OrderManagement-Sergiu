using UnityEngine;
using UnityEngine.EventSystems;


public class LeftClickClose : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// close the options pannel when the user click's outside of the panel
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            gameObject.SetActive(false);
        }
    }
}
