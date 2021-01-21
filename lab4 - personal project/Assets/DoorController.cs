using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    public GameObject instructions;
    public GameObject jumpInstr;
    public GameObject curtainInstr;
    public GameObject curtain;
    
    private void OnTriggerStay(Collider other){
        if (other.tag == "Door")
        {
            instructions.SetActive(true);
            Animator anim = other.GetComponentInChildren<Animator>();
            if (Input.GetKeyDown(KeyCode.E))
            {
                anim.SetTrigger("OpenClose");
            }
        }

        if (other.tag == "Jump")
        {
            jumpInstr.SetActive(true);
        }
        
        if (other.tag == "Curtain")
        {
            curtainInstr.SetActive(true);
            if (Input.GetKeyDown(KeyCode.V))
            {
                curtain.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Door")
        {
            instructions.SetActive(false);
        }
        
        if (other.tag == "Jump")
        {
            jumpInstr.SetActive(false);
        }

        if (other.tag == "Curtain")
        {
            curtainInstr.SetActive(false);
        }

    }
}
