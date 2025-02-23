﻿using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class KillBox : MonoBehaviour
{
	AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    // Array of strings for what Tag to care about.
    // 'string' is programmer for text.
    public string AffectedTag;

	// Function called by Unity's physics. 
	// Gives us the collider that entered this trigger.
	void OnTriggerEnter( Collider objectEntered )
	{

		string objectsTag = objectEntered.gameObject.tag;

		// 'if' objectsTag any of our 'AffectedTag' 
		if ( AffectedTag.Contains( objectsTag ) )
		{
			//THEN DO THIS

			string activeSceneName = SceneManager.GetActiveScene().name;

			//Load the scene file with the same name as activeSceneName
			SceneManager.LoadScene( activeSceneName );

		}
		else
		{

			// Debug.Log will print your message to Unity's console!
			// You can add two or more 'strings' together with the '+' operator.
			Debug.Log( "Collider not in Affected Tag! " + objectEntered.gameObject.tag );

		}

	}

}