using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravitational : MonoBehaviour
{
    public float mass;
    public float G_constant;
    public float minimal_distance;
    void Start()
    {
        if (minimal_distance == 0) minimal_distance = 0.0001f; //divide safety
    }
    void Update()
    { }
    private void OnTriggerStay(Collider col)
    {
        Rigidbody other = col.gameObject.GetComponent<Rigidbody>();
        if (other != null)
        {
            Vector3 f = new Vector3();
            f = this.transform.position -
           col.gameObject.GetComponent<Transform>().position;
            float dsqr = f.magnitude;
            if (dsqr < minimal_distance) dsqr = minimal_distance;
            float gmm = col.gameObject.GetComponent<Rigidbody>().mass * mass;
            f.Normalize();
            f *= (gmm / (2 * dsqr));
            other.AddForce(f);
        }
    }
}
