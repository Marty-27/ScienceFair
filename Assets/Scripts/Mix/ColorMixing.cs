using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMixing : MonoBehaviour
{
    private Color MixedColor;
    private Dictionary<Color, float> colorAmounts = new Dictionary<Color, float>();
    private float totalAmount = 0f;
    private Renderer renderer;

    private void Start()
    {
        this.renderer = this.GetComponent<Renderer>();
        this.renderer.material.color = Color.white;
    }

    public void AddColor(Color color, float amount)
    {
        if (colorAmounts.ContainsKey(color))
        {
            colorAmounts[color] += amount;
        }
        else
        {
            colorAmounts[color] = amount;
        }
        totalAmount += amount;

        UpdateMixedColor();
    }

    private void UpdateMixedColor()
    {
        if (totalAmount == 0f)
        {
            renderer.material.color = Color.white;
            return;
        }

        Color mixedColor = Color.black;

        foreach (KeyValuePair<Color, float> entry in colorAmounts)
        {
            mixedColor += entry.Key * (entry.Value / totalAmount);
        }

        renderer.material.color = mixedColor;
    }

    public void Reset()
    {
        colorAmounts = new Dictionary<Color, float>();
        totalAmount = 0f;
        this.renderer.material.color = Color.white;
    }
}
