using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BeatManager : MonoBehaviour {

    public static BeatManager instance;
    public static float beatLength;

    private Rigidbody2D Body;
	private Vector2 Direction;
	public SoundManager theSoundManager;
	public Spawner theSpawner;
	private bool audioStarted = false;
	public int bar, beat = 1;
	private int i; // To count through the whenToSpawn array

	public float bpm;
	public bool spawnSoundOn;


	void Awake()
	{
		beatLength = (60/bpm);

		Body = GetComponent<Rigidbody2D> ();
		theSoundManager = FindObjectOfType<SoundManager> ();
		theSpawner = FindObjectOfType<Spawner> ();

        if (instance == null) {
            instance = this;
        }
        else {
            Debug.LogError("Too many beat managers in the scene!");
        }
    }

	void Start () 
	{
		InvokeRepeating ("BeatEvent", beatLength, beatLength);
		
	}
	

	void BeatEvent()
	{
		if (!audioStarted)
		{
			theSoundManager.beatTest.Play ();
			audioStarted = true;
		}

		checkSpawners();
        beatCount();
    }

	// Keep track of current bar and beat (in 4/4 time)
	public void beatCount()
	{
		beat++;

		if (beat == 5)
		{
			bar++;
			beat = 1;
		}
	}

	public void checkSpawners()
	{
        if (beat == theSpawner.whenToSpawn[i].beat) {
            if (bar == theSpawner.whenToSpawn[i].bar) {
                if (spawnSoundOn) // Use test sound to ensure spawning matches music track
                {
                    theSoundManager.hitBall.Play();
                }

                theSpawner.Spawn(theSpawner.whenToSpawn[i]);

                i++;

                if (i == theSpawner.whenToSpawn.Length) {
                    //print("end of array");

                    i = 0;
                    bar = 1;
                    beat = 1;
                }
            }
        }
    }
}
