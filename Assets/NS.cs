using UnityEngine;
using System.Collections;

public class NS : MonoBehaviour {

    float timeToNext;
    float startTime;
    float interval = 1f;
    float bpm = 60.0f;
    public int numBeatsPerSegment = 1;

    public AudioClip[] clips = new AudioClip[2];
    public AudioClip x_clip = new AudioClip();
    private double nextEventTime;
    private int flip = 0;
    private AudioSource[] audioSources = new AudioSource[2];
    private AudioSource x_audioSource = new AudioSource();

    private bool running = false;

    float[] z = new float[16];
    float[] x = new float[16];

    //public AudioClip z_audio;
    AudioSource z_audio;
    //AudioSource x_audio;

	// Use this for initialization
	void Start () {
        z[0] = 1;
        z[8] = 1;

        x[4]  = 1;
        x[12] = 1;

        startTime = Time.time + interval;
        z_audio = GetComponent<AudioSource>();

        int i = 0;
        while (i < 2 ) {
            GameObject child = new GameObject("Player");
            child.transform.parent = gameObject.transform;

            GameObject child2 = new GameObject("Player2");
            child2.transform.parent = gameObject.transform;
            audioSources[i] = child.AddComponent<AudioSource>();
            x_audioSource = child2.AddComponent<AudioSource>();
            i++;
        }
        nextEventTime = AudioSettings.dspTime + 1.0F;
        running = true;
	}
	
	// Update is called once per frame
	void Update () {
        /*
         * timeToNext = startTime - Time.time;
        if (timeToNext< 0) {
            audio.PlayOneShot(z_audio);
            startTime = Time.time + interval;
        }
        */
            
        if (!running)
            return;

        double time = AudioSettings.dspTime;
        if (time + 1.0f > nextEventTime) {
            audioSources[flip].clip = clips[flip];
            
            audioSources[flip].PlayScheduled(nextEventTime);
            x_audioSource.clip = x_clip;
            x_audioSource.PlayScheduled(nextEventTime);
            Debug.Log("Scheduled source " + flip + " to start at time " +
                    nextEventTime);
            nextEventTime += 60.0f / (bpm * (float)numBeatsPerSegment);
            flip = 1 - flip;
        }
    }
}
