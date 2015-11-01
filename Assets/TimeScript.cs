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

    //float et;

    long[] noteTicks = new long[12];
    long target;
    int noteindex;
    bool noteindexadded;
    bool completed;
    int hit = 0 , missed = 0;
    float tresh;
    public Renderer rend;

    long point;
    long downTicks;

    public GameObject marker;
	// Use this for initialization
	void Start () {
        sw.Start();

        org = 5f;
        dest = 0f;
        spd = (dest-org) / 10000000;
        //transform.position = (new Vector3(0, 5,0));
	
        Populate();
        target = noteTicks[noteindex];
        tresh = 1000000;

        //Renderer rend = GetComponent<Renderer>();
        


	}
	
	// Update is called once per frame
	void Update () {
        ts = sw.Elapsed;
        Count();
        if (!completed) {
            NewCheck();
        }

        RespondToPress();
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

    void NewCheck() {
        if (Timing(target) == 0) {
            // if before treshold
        } else if (Timing(target) == 1) {
            // if within treshold
            //rend.material.SetColor("_Color", Color.green);
            noteindexadded = false;
            if (Input.GetKeyDown(KeyCode.S)) {
                hit ++;
                print("NOICE! total hit: " + hit);

                noteindex ++;
                if (noteindex != noteTicks.Length) {
                    target = noteTicks[noteindex];
                } else {
                    print("completed");
                    completed = true;
                }
            }
        } else {
            // if passed treshold
            //rend.material.SetColor("_Color", Color.red);

            
            if (!noteindexadded) {
                missed ++;
                print("too late! total missed: " + missed);

                noteindex ++;
                if (noteindex != noteTicks.Length) {
                    target = noteTicks[noteindex];
                } else {
                    print("completed");
                    completed = true;
                }
                noteindexadded = true; // only adds one time!
            }
        }

            

        /*
        if (WithinTreshold(target)) {
            rend.material.SetColor("_Color", Color.green);
            if (Input.GetKeyDown(KeyCode.S)) {
                print("NOICE!");

                noteindex ++;
                target = noteTicks[noteindex];
            }
        } else {
            rend.material.SetColor("_Color", Color.red);
        }
        */
    }

    void Populate() {
        noteTicks[0]  = 10000000;
        noteTicks[1]  = 15000000;
        noteTicks[2]  = 20000000;
        noteTicks[3]  = 25000000;
        noteTicks[4]  = 30000000;
        noteTicks[5]  = 35000000;
        noteTicks[6]  = 40000000;
        noteTicks[7]  = 45000000;
        noteTicks[8]  = 50000000;
        noteTicks[9]  = 55000000;
        noteTicks[10] = 60000000;
        noteTicks[11] = 65000000;


        for (int i=0; i<noteTicks.Length; i++) {
            Instantiate(marker, new Vector3(transform.position.x, 
                        noteTicks[i]/10000000f * 5f,
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
            if (noteindexadded == false) {
                noteindex ++;
                target = noteTicks[noteindex];
                noteindexadded = true;
            }
            return false;
        } else {
            return false;
        }
    

    }

    void RespondToPress() {
        downTicks = sw.ElapsedTicks - point;
        if (Input.GetKeyDown(KeyCode.S)) {
            point = sw.ElapsedTicks;
            rend.material.SetColor("_Color", Color.blue);
        } else {
            if (downTicks > 500000) {
                rend.material.SetColor("_Color", Color.white);
                downTicks = 0;
            }
        }

            
    }
}
