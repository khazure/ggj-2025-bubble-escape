using UnityEngine;

public class StopParticleOnExit : MonoBehaviour 
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

	private void OnTriggerExit(Collider objectExited)
	{
		if (TaggedObjectsOnly)
		{
			if (objectExited.gameObject.tag == Tag)
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
