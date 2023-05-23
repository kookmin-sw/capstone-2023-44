using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] public int from;
    [SerializeField] public int to;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 org_pos;

    public void OnBeginDrag(PointerEventData ped)
    {
        //Debug.Log("OnBeginDrag");
        //GetComponent<CanvasGroup>().blocksRaycasts = false;
        //canvasGroup.alpha = 0.7f;
    }

    public void OnDrag(PointerEventData ped)
    {
        rectTransform.anchoredPosition += ped.delta;
        //Debug.Log("OnDrag");
    }

    public void OnEndDrag(PointerEventData ped)
    {
        //Debug.Log("OnEndDrag");
        rectTransform.anchoredPosition = org_pos;
        //GetComponent<CanvasGroup>().blocksRaycasts = true;
        //canvasGroup.alpha = 1.0f;
    }

    public void SetOrgPos(Vector3 new_pos)
    {
        org_pos = new_pos;
    }
    public void OnDrop(PointerEventData ped)
    {
        DragDrop dragged = ped.pointerDrag.GetComponent<DragDrop>();

        switch (dragged.from)
        {
            case 1:
                switch (to)
                {
                    case 1:
                        Destroy(dragged);
                        break;
                }
                break;
        }

        Debug.Log(string.Format("dragged {0} to {1}", dragged.from, to));
        dragged.SetOrgPos(GetComponent<RectTransform>().anchoredPosition3D);
    }
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        //canvasGroup = GetComponent<CanvasGroup>();
        org_pos = rectTransform.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {

    }
}