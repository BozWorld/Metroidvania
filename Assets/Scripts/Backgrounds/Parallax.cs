using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour { 

    private float length, startx;
    private float height, starty;
    public GameObject cam;
    public float parallaxEffectLength;
    public float parallaxEffectHeight;

    void Start()
    {
        startx = transform.position.x;
        starty = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void FixedUpdate()
    {
    float dist = (cam.transform.position.x * parallaxEffectLength);
    transform.position = new Vector3(startx + dist, transform.position.y, transform.position.z);
    float diste = (cam.transform.position.y * parallaxEffectHeight);
    transform.position = new Vector3(transform.position.x, starty + diste, transform.position.z);
    }
}
