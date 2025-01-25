using UnityEngine;
using UnityEngine.Events;

namespace UnityTemplateProjects.DMA
{
    public class EventOnTriggerEnter : MonoBehaviour
    {
        
        public UnityEvent @event;

        private void OnTriggerEnter(Collider other)
        {
            @event.Invoke(); 
        }
    }
}