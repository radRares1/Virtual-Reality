using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    
    [SerializeField] public Rigidbody projectile;

    [SerializeField] [Range(10f, 80f)] public float angle = 15f;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                FireAtPoint(hit.point);
            }
        }
    }

    private void FireAtPoint(Vector3 point)
    {
        var velocity = ProjectileVelocity(point,angle);
        projectile.transform.position = transform.position;
        projectile.velocity = velocity;
    }

    private Vector3 ProjectileVelocity(Vector3 destination, float inputAngle)
    {
        Vector3 direction = destination - transform.position;
        float height = direction.y;
        direction.y = 0;
        float distance = direction.magnitude;
        float angleToRadians = inputAngle * Mathf.Deg2Rad;
        direction.y = distance * Mathf.Tan(angleToRadians);
        distance += height / Mathf.Tan(angleToRadians);

        float velocity = (float)Math.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * angleToRadians));
        return velocity * direction.normalized;
    }


    void Start()
    {  
        //projectile = gameObject.GetComponent<Rigidbody>(); // Add the rigidbody.
        //print(projectile.mass);
    }
}