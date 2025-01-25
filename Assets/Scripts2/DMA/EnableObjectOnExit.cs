using UnityEngine;

public class EnableObjectOnExit : MonoBehaviour 
{
	[SerializeField] private GameObject target = null;

	[SerializeField] private bool TaggedObjectsOnly = false;
	[SerializeField] private string Tag ="";
	
	// Use this for initialization
	void Start () 
	{
		if (target == null)
		{
			Debug.LogError("Missing target object! Click Here!", gameObject);
		}
	}

	private void OnTriggerExit(Collider objectExited)
	{
		if (TaggedObjectsOnly)
		{
			if (objectExited.gameObject.tag == Tag)
			{
				target.SetActive(true);
			}
		}
		else
		{		
			target.SetActive(true);
		}
	}
}
