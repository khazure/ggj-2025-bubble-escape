using UnityEngine;

public class TransformToOnEvent : MonoBehaviour
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

    public void Transform(GameObject target)
    {
        targetGameObject = target;
        
        var character = target.gameObject.GetComponent<CharacterController>();
        if (character != null)
        {
            character.enabled = false;
        }
        
        if (!taggedObjectsOnly || targetGameObject.gameObject.CompareTag(tag))
        {
            if (usePosition && useLocalPosition)
            {
                targetGameObject.transform.position = targetTransform.localPosition;
            }
            else if (usePosition)
            {
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
