using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dancebbyShin : MonoBehaviour
{
    private float lowerBoundAngle = -10;
    private float upperBoundAngle = 0;
    private float currentAngle = 0;
    private bool keepIncreasing = true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (keepIncreasing)
        {
            if(currentAngle <= upperBoundAngle)
            {
                transform.Rotate(1, 0, 0);
                currentAngle+=1;
                
            }
            else
            {
                keepIncreasing = false;
            }
           
        }
        else
        {
            if (currentAngle >= lowerBoundAngle)
            {
                transform.Rotate(-1, 0, 0);
                currentAngle-=1;
            }
            else
            {
                keepIncreasing = true;

            }

           
        }
        
        //transform.Rotate(0,30,0);
        
    }
}
