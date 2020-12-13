using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bellRinger : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource meow;
    void Start()
    {
        meow = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        meow.Play();
    }
}
