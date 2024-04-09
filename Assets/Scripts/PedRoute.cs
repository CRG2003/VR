using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedRoute : MonoBehaviour
{
    public List<Transform> wps, route;
    public int rn = 0, targetWP = 0;
    public float initDelay;
    public bool go = false;

    GameObject ps;
    Rigidbody rb;


    void Start()
    {
        wps = new List<Transform>();
        ps = GameObject.Find("Waypoints");
        foreach (Transform child in ps.transform){
            wps.Add(child);
        }

        rb = GetComponent<Rigidbody>();

        initDelay = UnityEngine.Random.Range(2f, 12f);
        transform.position = new Vector3(0f, -5f, 0f);
    }


    void FixedUpdate()
    {
        if (!go){
            initDelay -= Time.deltaTime;
            if (initDelay <= 0f){
                go = true;
                setRoute();
            }
            else return;
        }

        Vector3 displacement = route[targetWP].position - transform.position;
        displacement.y = 0;
        float dist = displacement.magnitude;

        if (dist < 0.1f){
            targetWP++;
            if (targetWP >= route.Count){
                setRoute();
                return;
            }
        }

        Vector3 velocity = displacement;
        velocity.Normalize();
        velocity *= 1.4f;
        Vector3 newPos = transform.position;
        newPos += velocity * Time.deltaTime;
        rb.MovePosition(newPos);

        Vector3 dForward = Vector3.RotateTowards(transform.forward, velocity, 10.0f * Time.deltaTime, 0f);
        Quaternion rotation = Quaternion.LookRotation(dForward);
        rb.MoveRotation(rotation);
    }


    void setRoute(){
        rn = UnityEngine.Random.Range(0, 12);

        if (rn == 0) route = new List<Transform> { wps[0], wps[4],  wps[5], wps[6] };
        else if (rn == 1) route = new List<Transform> { wps[0], wps[4], wps[5], wps[7] };
        else if (rn == 2) route = new List<Transform> { wps[2], wps[1], wps[4], wps[5], wps[6] };
        else if (rn == 3) route = new List<Transform> { wps[2], wps[1], wps[4], wps[5], wps[7] };
        else if (rn == 4) route = new List<Transform> { wps[3], wps[4], wps[5], wps[6] };
        else if (rn == 5) route = new List<Transform> { wps[3], wps[4], wps[5], wps[7] };
        else if (rn == 6) route = new List<Transform> { wps[6], wps[5], wps[4], wps[0] };
        else if (rn == 7) route = new List<Transform> { wps[6], wps[5], wps[4], wps[3] };
        else if (rn == 8) route = new List<Transform> { wps[6], wps[5], wps[4], wps[1], wps[2] };
        else if (rn == 9) route = new List<Transform> { wps[7], wps[5], wps[4], wps[0] };
        else if (rn == 10) route = new List<Transform> { wps[7], wps[5], wps[4], wps[3] };
        else if (rn == 11) route = new List<Transform> { wps[7], wps[5], wps[4], wps[1], wps[2] };

        transform.position = new Vector3(route[0].position.x, 0.0f, route[0].position.z);
        targetWP = 1;
    }
}
