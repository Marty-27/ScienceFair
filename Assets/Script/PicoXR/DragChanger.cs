using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragChanger : MonoBehaviour
{
    public float inside_drag;
    public float leaving_drag;
    public float inside_r_drag;
    public float leaving_r_drag;
    void Start()
    { }
    void Update()
    { }
    private void OnTriggerStay(Collider col)
    {
        Rigidbody other = col.gameObject.GetComponent<Rigidbody>();
        if (other != null)
        {
            other.drag = inside_drag;
            other.angularDrag = inside_r_drag;
        }
    }
    private void OnTriggerExit(Collider col)
    {
        Rigidbody other = col.gameObject.GetComponent<Rigidbody>();
        if (other != null)
        {
            other.drag = leaving_drag;
            other.angularDrag = leaving_r_drag;
        }
    }
}
