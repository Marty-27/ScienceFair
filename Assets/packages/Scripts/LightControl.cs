using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour
{
    public GameObject btn_light;
    public GameObject btn_bulb;
    [Tooltip("button/bulb color")]
    public Color btn_color;
    [Tooltip("OFF state light intensity")]
    public float off_intensity;
    [Tooltip("ON state light intensity")]
    public float on_intensity;
    //Internals
    private Light l;
    private bool on_state; //Light state (true = on, false = off)
    
    // Start is called before the first frame update
    void Start()
    {
        on_state = false;
        l = btn_light.GetComponent<Light>();
        if (l == null)
        {
            Debug.Log("LightControl " + this.name + "-Error no local light defined");
        }
        else l.color = btn_color;
        Renderer rend = btn_bulb.GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.Log("LightControl " + this.name + "-btn_bulb do not have renderer");
        }
        else rend.material.color = btn_color;
    }

    public void Switch_On()
    {
        if (l != null) l.intensity = on_intensity;
        this.on_state = true;
    }

    public void Switch_Off()
    {
        if (l != null) l.intensity = off_intensity;
        this.on_state = false;
    }

    public void Switch_State()
    {
        Debug.Log("state : " + on_state);
        if (this.on_state) this.Switch_Off();
        else this.Switch_On();
        Debug.Log("state : " + on_state);
    }

    public void test()
    {
        Debug.Log("test");
    }

    void Update()
    {
        on_state = false;
        l = btn_light.GetComponent<Light>();
        if (l == null)
        {
            Debug.Log("LightControl " + this.name + "-Error no local light defined");
        }
        else l.color = btn_color;
        Renderer rend = btn_bulb.GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.Log("LightControl " + this.name + "-btn_bulb do not have renderer");
        }
        else rend.material.color = btn_color;
    }
}
