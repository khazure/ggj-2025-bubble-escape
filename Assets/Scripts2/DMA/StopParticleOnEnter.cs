using UnityEngine;

public class StopParticleOnEnter : MonoBehaviour 
{
	[SerializeField] private new ParticleSystem particleSystem = null;

	[SerializeField] private bool TaggedObjectsOnly = false;
	[SerializeField] private string Tag ="";
	
	// Use this for initialization
	void Start () 
	{
		if (particleSystem == null)
		{
			Debug.LogError("Missing pArticle System! Click Here!", gameObject);
		}
	}

	private void OnTriggerEnter(Collider objectEntered)
	{
		if (TaggedObjectsOnly)
		{
			if (objectEntered.gameObject.tag == Tag)
			{
				particleSystem.Stop();
			}
		}
		else
		{		
			particleSystem.Stop();
		}
	}
}
