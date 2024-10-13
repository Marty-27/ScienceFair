using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instanciate : MonoBehaviour
{
    public GameObject instanciation_base_model;
    public Transform apearance_transform;
    public GameObject hierarchical_parent;
    public string instance_name;
    public bool name_num_add;
    public Vector3 dropping_force;
    public ForceMode force_mode;
    private int id;

    void Start()
    { id = 0; }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) Instanciate_prefab();
    }
    public void Instanciate_prefab()
    {
        if (instanciation_base_model)
        {
            //Id increment
            id++;
            GameObject created;
            string name = instance_name;
            if (name_num_add) name += id;
            Debug.Log("droping : " + name);
            //Create new instance from prefab model
            Vector3 instance_position = new Vector3();
            instance_position = apearance_transform.position;
            GameObject parent = hierarchical_parent;
            if (hierarchical_parent != null)
                created = Instantiate(instanciation_base_model, instance_position,
               apearance_transform.rotation, hierarchical_parent.transform);
            else
                created = Instantiate(instanciation_base_model, instance_position,
               apearance_transform.rotation);
            //Name it
            created.name = name;
            Debug.Log("-- " + name + "Dropped");
            //Droping Impulse Force
            created.GetComponent<Rigidbody>().AddForce(dropping_force, force_mode);
        }
    }
}
