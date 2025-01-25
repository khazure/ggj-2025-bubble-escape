using UnityEngine;

public class TriggerAnimatorOnExit : MonoBehaviour 
{
	[SerializeField] private Animator animator = null;

	[SerializeField] private string animatorTriggerName = null;
	
	[SerializeField] private bool TaggedObjectsOnly = false;
	[SerializeField] private string Tag ="";
	
	// Use this for initialization
	void Start () 
	{
		if (animator == null)
		{
			Debug.LogError("Missing Animator! Click Here!", gameObject);
		}
	}

	private void OnTriggerExit(Collider objectExited)
	{
		if (TaggedObjectsOnly)
		{
			if (objectExited.gameObject.tag == Tag)
			{
				animator.SetTrigger(animatorTriggerName);
			}
		}
		else
		{		
			animator.SetTrigger(animatorTriggerName);
		}
	}
}
