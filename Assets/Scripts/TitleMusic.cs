using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMusic : MonoBehaviour
{
    public AudioSource source;

    float duration;
    float reset;
    // Start is called before the first frame update
    void Start()
    {
        duration = 82;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - reset >= duration)
        {
            reset = Time.time;
            source.time = 6;
            source.Play();
            duration = 76;
        }
    }
}
