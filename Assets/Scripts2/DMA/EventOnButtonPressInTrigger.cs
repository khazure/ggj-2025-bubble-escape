using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace UnityTemplateProjects.DMA
{
    public class EventOnButtonPressInTrigger : MonoBehaviour
    {
        
        public UnityEvent @event;

        public Key keyToPress;

        public float pressDelay = 0f;

        private float pressStart = 0f;
        private float pressTime = 0f;
        private bool inZone = false;
        
        private void OnTriggerEnter(Collider other)
        {
            inZone = true;
        }

        private void OnTriggerExit(Collider other)
        {
            inZone = false;
        }

        private void Update()
        {
            if(inZone && Keyboard.current.allKeys.Any(it => it.keyCode == keyToPress) )
            {
                if (pressStart == 0f)
                {
                    pressStart = Time.time;
                }
                else
                {
                    pressTime += Time.deltaTime;
                }

                if (pressTime >= pressStart + pressDelay)
                {
                    pressStart = 0f;
                    pressTime = 0f;
                    @event.Invoke(); 
                }
            } 
        }
    }
}