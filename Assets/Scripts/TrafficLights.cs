using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLights : MonoBehaviour
{
    public Transform t1, t2, t3;
    public GameObject t1g, t1r, t2g, t2r, t3g, t3r;
    public float timer;
    public bool state;

    void Start()
    {
        t1 = transform.Find("TL1");
        t2 = transform.Find("TL2");
        t3 = transform.Find("TL3");

        t1g = t1.Find("Green light").gameObject;
        t2g = t2.Find("Green light").gameObject;
        t3g = t3.Find("Green light").gameObject;
        t1r = t1.Find("Red light").gameObject;
        t2r = t2.Find("Red light").gameObject;
        t3r = t3.Find("Red light").gameObject;

        timer = 10f;
        setState(true);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0){
            if (state){
                setState(false);
            }
            else{
                setState(true);
            }
            timer = 10f;
        }
    }

    void setState(bool c){
        state = c;
        if (c){
            t1g.active = true;
            t2g.active = false;
            t3g.active = false;

            t1r.active = false;
            t2r.active = true;
            t3r.active = true;
        }
        else{
            t1g.active = false;
            t2g.active = true;
            t3g.active = true;

            t1r.active = true;
            t2r.active = false;
            t3r.active = false;    
        }
    }
}
