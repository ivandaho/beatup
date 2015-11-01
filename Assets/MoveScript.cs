using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class MoveScript : MonoBehaviour {
    public Stopwatch sw = new Stopwatch();
    float initpos;

	// Use this for initialization
	void Start () {
        sw.Start();
        initpos = transform.position.y;
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = (new Vector3(0,
                    sw.ElapsedTicks/10000000f*-5f + initpos, 0));
	
	}
}
