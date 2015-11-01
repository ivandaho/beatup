﻿using UnityEngine;
using System.Collections;
using System.Diagnostics; // req for stopwatch
using System.Threading;
using System;

public class TimeScript : MonoBehaviour {
    public Stopwatch sw = new Stopwatch();
    public TimeSpan ts;

    long interval = Stopwatch.Frequency;
    long intervalStart = Stopwatch.Frequency;
    long timeToNext;

    float org, dest;
    float spd;

    //float et;

    long[] noteTicksS = new long[4];
    long targetS;
    int noteindexS;
    bool noteindexaddedS;
    bool completedS;

    long[] noteTicksD = new long[4];
    long targetD;
    int noteindexD;
    bool noteindexaddedD;
    bool completedD;

    int hit = 0 , missed = 0;
    float tresh;
    public Renderer rendS;
    public Renderer rendD;

    long pointS;
    long pointD;
    long downTicksS;
    long downTicksD;

    public GameObject marker;
	// Use this for initialization
	void Start () {
        sw.Start();

        org = 5f;
        dest = 0f;
        spd = (dest-org) / 10000000;
        //transform.position = (new Vector3(0, 5,0));
	
        Populate();
        targetS = noteTicksS[noteindexS];
        targetD = noteTicksD[noteindexD];
        tresh = 1000000;

        //Renderer rend = GetComponent<Renderer>();
        


	}
	
	// Update is called once per frame
	void Update () {
        ts = sw.Elapsed;
        Count();
        if (!completedS) {
            CheckS();
        }
        if (!completedD) {
            CheckD();
        }

        RespondToPressS();
        RespondToPressD();
        //Check();
        //et = Convert.ToSingle(sw.ElapsedTicks);

        // dont really need to position...
        //transform.position = (new Vector3(0,
        //            sw.ElapsedTicks/10000000f*-5f + 5f, 0));



        //transform.position += (new Vector3(0, 
                    //(dest-org)/60, 0));


        //transform.position -= (new Vector3(0, 
        // spd*et, 0));

        // current =  (init + dest/spd)
	
	}
    
    void Count() {
        timeToNext = interval - sw.ElapsedTicks;
        if (timeToNext < 0) {
            interval = intervalStart + sw.ElapsedTicks;
            //print(sw.ElapsedTicks%intervalStart);
        }
    }

    void Check() {
        if (timeToNext < 500000) {
            if (Input.GetKeyDown(KeyCode.S)) {
                transform.position = (new Vector3 (0, org, 0));
                print("NOICE");
            }
        } else if (timeToNext < 1000000) {
            if (Input.GetKeyDown(KeyCode.S)) {
                transform.position = (new Vector3 (0, org, 0));
                print("EARLY");
            }
        } else if (timeToNext > 9000000 && timeToNext < 10000000) {
            if (Input.GetKeyDown(KeyCode.S)) {
                transform.position = (new Vector3 (0, org, 0));
                print("LATE");
            }
        }
    }

    void CheckS() {
        if (Timing(targetS) == 0) {
            // if before treshold
        } else if (Timing(targetS) == 1) {
            // if within treshold
            //rend.material.SetColor("_Color", Color.green);
            noteindexaddedS = false;
            if (Input.GetKeyDown(KeyCode.S)) {
                hit ++;
                print("NOICE! total hit: " + hit);

                noteindexS ++;
                if (noteindexS != noteTicksS.Length) {
                    targetS = noteTicksS[noteindexS];
                } else {
                    print("completedS");
                    completedS = true;
                }
            }
        } else {
            // if passed treshold
            //rend.material.SetColor("_Color", Color.red);
            
            if (!noteindexaddedS) {
                missed ++;
                print("too late! total missed: " + missed);

                noteindexS ++;
                if (noteindexS != noteTicksS.Length) {
                    targetS = noteTicksS[noteindexS];
                } else {
                    print("completedS");
                    completedS = true;
                }
                noteindexaddedS = true; // only adds one time!
            }
        }
    }

    void CheckD() {
        if (Timing(targetD) == 0) {
            // if before treshold
        } else if (Timing(targetD) == 1) {
            // if within treshold
            //rend.material.SetColor("_Color", Color.green);
            noteindexaddedD = false;
            if (Input.GetKeyDown(KeyCode.D)) {
                hit ++;
                print("NOICE! total hit: " + hit);

                noteindexD ++;
                if (noteindexD != noteTicksD.Length) {
                    targetD = noteTicksD[noteindexD];
                } else {
                    print("completedD");
                    completedD = true;
                }
            }
        } else {
            // if passed treshold
            //rend.material.DetColor("_Color", Color.red);
            
            if (!noteindexaddedD) {
                missed ++;
                print("too late! total missed: " + missed);

                noteindexD ++;
                if (noteindexD != noteTicksD.Length) {
                    targetD = noteTicksD[noteindexD];
                } else {
                    print("completedD");
                    completedD = true;
                }
                noteindexaddedD = true; // only adds one time!
            }
        }
    }

    void Populate() {
        noteTicksS[0]  = 10000000;
        noteTicksS[1]  = 20000000;
        noteTicksS[2]  = 30000000;
        noteTicksS[3]  = 40000000;

        noteTicksD[0] = 15000000;
        noteTicksD[1] = 25000000;
        noteTicksD[2] = 35000000;
        noteTicksD[3] = 45000000;


        for (int i=0; i<noteTicksS.Length; i++) {
            Instantiate(marker, new Vector3(transform.position.x, 
                        noteTicksS[i]/10000000f * 5f,
                        transform.position.z), 
                        Quaternion.identity); 
        }

        for (int i=0; i<noteTicksD.Length; i++) {
            Instantiate(marker, new Vector3(transform.position.x+1f, 
                        noteTicksD[i]/10000000f * 5f,
                        transform.position.z), 
                        Quaternion.identity); 
        }

        
    }

    // 0 = before, 1 = inside, 2 = outside
    int Timing(long t) {
        if (sw.ElapsedTicks < t - tresh) {
            //early
            return 0;
        } else if (sw.ElapsedTicks < t + tresh){
            // not yet reached end of treshold
            return 1;
        } else {
            return 2;
        }
    }

    
    bool WithinTreshold(long t) {
        if (sw.ElapsedTicks > t - tresh && sw.ElapsedTicks < t + tresh){
            return true;
        } else if (sw.ElapsedTicks > t + tresh){
            if (noteindexaddedS == false) {
                noteindexS ++;
                targetS = noteTicksS[noteindexS];
                noteindexaddedS = true;
            }
            return false;
        } else {
            return false;
        }
    

    }

    void RespondToPressS() {
        downTicksS = sw.ElapsedTicks - pointS;
        if (Input.GetKeyDown(KeyCode.S)) {
            pointS = sw.ElapsedTicks;
            rendS.material.SetColor("_Color", Color.blue);
        } else {
            if (downTicksS > 500000) {
                rendS.material.SetColor("_Color", Color.white);
                downTicksS = 0;
            }
        }
    }

    void RespondToPressD() {
        downTicksD = sw.ElapsedTicks - pointD;
        if (Input.GetKeyDown(KeyCode.D)) {
            pointD = sw.ElapsedTicks;
            rendD.material.SetColor("_Color", Color.blue);
        } else {
            if (downTicksD > 500000) {
                rendD.material.SetColor("_Color", Color.white);
                downTicksD = 0;
            }
        }
    }
}