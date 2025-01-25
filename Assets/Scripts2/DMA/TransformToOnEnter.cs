using UnityEngine;

public class TransformToOnEnter : MonoBehaviour
{
    [SerializeField] private GameObject targetGameObject;

    [SerializeField] private Transform targetTransform;
    
    [SerializeField] private bool useRotation;
    [SerializeField] private bool useLocalRotation;

    [SerializeField] private bool useScale;
    [SerializeField] private bool useLocalScale;
    
    [SerializeField] private bool usePosition;
    [SerializeField] private bool useLocalPosition;

    [SerializeField] private bool taggedObjectsOnly = false;
    [SerializeField] private new string tag = "";
    
    void OnTriggerEnter(Collider objectEntered)
    {
        var character = objectEntered.gameObject.GetComponent<CharacterController>();
        if (character != null)
        {
            character.enabled = false;
        }
        if (!taggedObjectsOnly || objectEntered.gameObject.CompareTag(tag))
        {
            if (usePosition && useLocalPosition)
            {
                targetGameObject.transform.position = targetTransform.localPosition;
            }
            else if (usePosition)
            {
                Debug.Log(targetGameObject.transform.position +" to: " +targetTransform.position);
                targetGameObject.transform.position = targetTransform.position;
            }

            if (useRotation && useLocalRotation)
            {
                targetGameObject.transform.rotation = targetTransform.localRotation;
            } 
            else if (useRotation)
            {
                targetGameObject.transform.rotation = targetTransform.rotation;
            }
            
            if (useScale && useLocalScale)
            {
                targetGameObject.transform.localScale = targetTransform.localScale;
            } 
            else if (useRotation)
            {
                targetGameObject.transform.localScale = targetTransform.lossyScale;
            }
        }
        
        if (character != null)
        {
            character.enabled = true;
        }

    }
}
