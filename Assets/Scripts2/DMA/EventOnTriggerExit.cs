using UnityEngine;
using UnityEngine.Events;

namespace UnityTemplateProjects.DMA
{
    public class EventOnTriggerExit : MonoBehaviour
    {

        public UnityEvent @event;

        private void OnTriggerExit(Collider other)
        {
            @event.Invoke();
        }
    }
}
