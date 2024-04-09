using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carRoute : MonoBehaviour
{
    public List<Transform> wps, route, peds, close;
    public List<float> dists;
    public int rnum = 0, targetWP = 0;
    public float initDelay, stopTimer;
    public bool go, stop;
    public Transform user;

    GameObject wp, tl;
    Rigidbody rb;

    void Start()
    {
        wps = new List<Transform>();
        wp = GameObject.Find("cWaypoints");
        foreach (Transform child in wp.transform){
            wps.Add(child);
        }

        peds = new List<Transform>();
        wp = GameObject.Find("Pedestrians");
        foreach (Transform child in wp.transform){
            peds.Add(child);
        }

        wp = GameObject.Find("Cars");
        foreach (Transform child in wp.transform){
            peds.Add(child);
        }
        peds.Add(user);
        peds.Remove(this.transform);

        rb = GetComponent<Rigidbody>();
        tl = GameObject.Find("Traffic Lights");

        initDelay = UnityEngine.Random.Range(3f, 12f);
        transform.position = new Vector3(10f, -5f, 10f);

        dists.Add(0f);
        dists.Add(0f);
        dists.Add(0f);
    }

    void FixedUpdate()
    {
        if (!go){
            initDelay -= Time.deltaTime;
            if (initDelay <= 0){
                go = true;
                setRoute();
            }
            else return;
        }

        close = new List<Transform>();
        foreach (Transform t in peds){
            Vector3 gap = transform.position - t.transform.position;
            gap.y = 0;
            float gapm = gap.magnitude;

            if (gapm <= 4.9f && stopTimer < -0.5f){
                close.Add(t);
            }
        }
        if (close.Count > 1){
            return;
        }
        else if (close.Count == 1){
            stopTimer = 3f;
        }

        stopTimer -= Time.deltaTime;
        if (stopTimer > 0){
            return;
        }

        int i = 0;
        foreach (Transform t in tl.transform){
            Vector3 d = transform.position - t.transform.position;
            d.y = 0;
            float dd = d.magnitude;
            dists[i] = dd;
            i++;
        }

        if (tl.GetComponent<TrafficLights>().state){

            if (rnum == 0 && dists[2] <= 6f){
                return;
            }
            else if ((rnum == 1 || rnum == 3) && dists[1] <= 6f){
                return;
            }
        }
        else if (rnum == 2 && dists[0] <= 6f){
            return;
        }

        Vector3 displ = route[targetWP].position - transform.position;
        displ.y = 0;
        float dist = displ.magnitude;

        if (dist< 0.1f){
            targetWP++;
            if (targetWP >= route.Count){
                setRoute();
                return;
            }
        }

        Vector3 vel = displ;
        vel.Normalize();
        vel *= 6f;
        Vector3 newPos = transform.position;
        newPos += vel * Time.deltaTime;
        rb.MovePosition(newPos);

        Vector3 dfor = Vector3.RotateTowards(transform.forward, vel, 10f * Time.deltaTime, 0f);
        Quaternion rot = Quaternion.LookRotation(dfor);
        rb.MoveRotation(rot);
    }

    void setRoute(){
        rnum = UnityEngine.Random.Range(0, 3);
        targetWP = 1;

        if (rnum == 0) route = new List<Transform> {wps[3], wps[2]};
        else if (rnum == 1) route = new List<Transform> {wps[1], wps[0]};
        else if (rnum == 2) route = new List<Transform> {wps[4], wps[5], wps[6], wps[7], wps[8], wps[2]};
        else if (rnum == 3) route = new List<Transform> {wps[1], wps[9], wps[10], wps[11], wps[12], wps[13]};

        transform.position = new Vector3(route[0].position.x, 0.0f, route[0].position.z);
    }
}
