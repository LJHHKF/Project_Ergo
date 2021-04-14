using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRepulsive : MonoBehaviour
{
    [SerializeField] private float x_power;
    [SerializeField] private float z_power;

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody t_rb = collision.gameObject.GetComponent<Rigidbody>();

        t_rb.AddForce(new Vector3(x_power, 0, z_power));
    }
}
