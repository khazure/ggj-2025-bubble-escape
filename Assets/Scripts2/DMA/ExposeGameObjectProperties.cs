using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExposeGameObjectProperties : MonoBehaviour
{

    public void SetGameObjectPosition(Vector3 newPosition)
    {
        this.gameObject.transform.position = newPosition;
    }
    
    public void SetGameObjectRotation(Vector3 newRotation)
    {
        this.gameObject.transform.eulerAngles = newRotation;   
    }
    
    public void SetGameObjectRotation(Quaternion newRotation)
    {
        this.gameObject.transform.rotation = newRotation;   
    }

    public void SetGameObjectScale(Vector3 newScale)
    {
        this.transform.localScale = newScale;
    }
}
