using UnityEngine;
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
    float bpm = 60f;

    //float et;

    long[] noteTicksS = new long[3];
    long targetS;
    int noteindexS;
    bool noteindexaddedS;
    bool completedS;

    long[] noteTicksD = new long[2];
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
    public Transform sheet;


    // audio stuff...

    // need to streamline
    AudioSource[] audioS = new AudioSource[3];
    public AudioClip[] audioclipS = new AudioClip[3];
    AudioSource[] audioD = new AudioSource[2];
    public AudioClip[] audioclipD = new AudioClip[2];



	// Use this for initialization
	void Start () {

        sw.Start();

        org = 5f;
        dest = 0f;
        spd = (dest-org) / 10000000;
        //transform.position = (new Vector3(0, 5,0));
	
        //audioSclips = new AudioClip[3];
        //audioDclips = new AudioClip[2];
        Populate();
        targetS = noteTicksS[noteindexS];
        targetD = noteTicksD[noteindexD];
        tresh = 1000000;

        //Renderer rend = GetComponent<Renderer>();

        //audio stuff...

        for (int i=0; i<audioS.Length; i++) {
        audioS[i] = this.gameObject.AddComponent<AudioSource>();
        audioS[i].clip = audioclipS[i];
        }

        for (int i=0; i<audioD.Length; i++) {
        audioD[i] = this.gameObject.AddComponent<AudioSource>();
        audioD[i].clip = audioclipD[i];
        }

	}
	
	// Update is called once per frame
	void Update () {
        RespondToPressS();
        RespondToPressD();
        ts = sw.Elapsed;
        Count();
        if (!completedS) {
            CheckS();
        }
        if (!completedD) {
            CheckD();
        }

        //Check();
	
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
                    noteindexS --;
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
                    noteindexS --;
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
                    noteindexD --;
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
                    noteindexD --;
                }
                noteindexaddedD = true; // only adds one time!
            }
        }
    }

    void Populate() {
        //                    8 beats per bar
        //
        //                   first bar is 16
        //                   for a delay of
        //                   one second
        noteTicksS[0]  = ConvertLocToTick(16 + 1  - 1);
        noteTicksS[1]  = ConvertLocToTick(16 + 9  - 1);
        noteTicksS[2]  = ConvertLocToTick(16 + 11 - 1);

        noteTicksD[0]  = ConvertLocToTick(16 + 5  - 1);
        noteTicksD[1]  = ConvertLocToTick(16 + 13 - 1);
        /*
        noteTicksS[0]  = ConvertLocToTick(16 + 1 - 1);
        noteTicksS[1]  = ConvertLocToTick(16 + 4 - 1);
        noteTicksS[2]  = ConvertLocToTick(16 + 1 - 1);
        noteTicksS[3]  = ConvertLocToTick(16 + 3 - 1);

        noteTicksS[4]  = ConvertLocToTick(32 + 0 - 1);
        noteTicksS[5]  = ConvertLocToTick(32 + 4 - 1);
        noteTicksS[6]  = ConvertLocToTick(32 + 1 - 1);
        noteTicksS[7]  = ConvertLocToTick(32 + 3 - 1);



        noteTicksD[0]  = ConvertLocToTick(16 + 3 - 1);
        noteTicksD[1]  = ConvertLocToTick(32 + 3 - 1);
        */


        for (int i=0; i<noteTicksS.Length; i++) {
            GameObject clone;
            clone = Instantiate(marker, new Vector3(transform.position.x, 
                        noteTicksS[i]/10000000f * 5f,
                        transform.position.z), 
                        Quaternion.identity) as GameObject;
            clone.transform.SetParent(sheet);
            
        }

        for (int i=0; i<noteTicksD.Length; i++) {
            GameObject clone;
            clone = Instantiate(marker, new Vector3(transform.position.x+1f, 
                        noteTicksD[i]/10000000f * 5f,
                        transform.position.z), 
                        Quaternion.identity) as GameObject;
            clone.transform.SetParent(sheet);
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
            audioS[noteindexS].Play();
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
            audioD[noteindexD].Play();
            pointD = sw.ElapsedTicks;
            rendD.material.SetColor("_Color", Color.blue);
        } else {
            if (downTicksD > 500000) {
                rendD.material.SetColor("_Color", Color.white);
                downTicksD = 0;
            }
        }
    }

    long ConvertLocToTick(int loc) {
        return ((long)((float)loc*0.0625f*10000000f*120f/bpm));
    }
}
