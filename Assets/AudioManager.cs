using UnityEngine;



public class AudioManager : MonoBehaviour

{

    [Header("---------- Audio Source ----------")]

    [SerializeField] AudioSource musicSource;

    [SerializeField] AudioSource SFXSource;



    [Header("---------- Audio Clip ----------")]

    public AudioClip background;

    public AudioClip death;

    public AudioClip obstacleTouch;

    public AudioClip bubblePad;



    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()

    {

        musicSource.clip = background;

        musicSource.Play();

    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}