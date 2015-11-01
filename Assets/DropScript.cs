using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System;

public class DropScript : MonoBehaviour {

    public GameObject obj;
    long tth = 2500000;
    

    public Stopwatch sw = new Stopwatch();
    public TimeSpan ts;

    long interval = Stopwatch.Frequency;
    long intervalStart = Stopwatch.Frequency;
    long timeToNext;
    long min, max;

	// Use this for initialization
	void Start () {
	}

	
	// Update is called once per frame
	void Update () {

        ts = sw.Elapsed;
        timeToNext = interval - sw.ElapsedTicks;

        //if (time > min && time < max) {
        //Instantiate(obj, transform.position, Quaternion.identity);
        //}
	}
}
