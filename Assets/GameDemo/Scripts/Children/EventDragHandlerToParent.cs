using UnityEngine;
using UnityEngine.EventSystems;

namespace GameDemo.Scripts.Children
{
    public class EventDragHandlerToParent : MonoBehaviour
                                          , IBeginDragHandler
                                          , IDragHandler
                                          , IEndDragHandler
    {
        private IBeginDragHandler _parentBeginDragHandler;
        private IDragHandler      _parentDragHandler;
        private IEndDragHandler   _parentEndDragHandler;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _parentBeginDragHandler ??= this.transform.parent.GetComponent<IBeginDragHandler>();
            _parentBeginDragHandler?.OnBeginDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _parentDragHandler ??= this.transform.parent.GetComponent<IDragHandler>();
            _parentDragHandler?.OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _parentEndDragHandler ??= this.transform.parent.GetComponent<IEndDragHandler>();
            _parentEndDragHandler?.OnEndDrag(eventData);
        }
    }
}