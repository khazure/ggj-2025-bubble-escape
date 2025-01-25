using UnityEngine;

public class PlayParticleOnEnter : MonoBehaviour
{
	[SerializeField] private new ParticleSystem particleSystem = null;

	[SerializeField] private bool TaggedObjectsOnly = false;
	[SerializeField] private string Tag ="";
	
	// Use this for initialization
	void Start () 
	{
		if (particleSystem == null)
		{
			Debug.LogError("Missing Particle System! Click Here!", gameObject);
		}
	}

	private void OnTriggerEnter(Collider objectEntered)
	{
		if (TaggedObjectsOnly)
		{
			if (objectEntered.gameObject.tag == Tag)
			{
				particleSystem.Play();
			}
		}
		else
		{		
			particleSystem.Play();
		}
	}
}
