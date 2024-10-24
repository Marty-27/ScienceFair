using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCellInfo : MonoBehaviour
{
    private int size;
    private Quaternion wallRotation;
    private string wallName;

    public void Initialize(int size, Vector3 rotation, string name)
    {
        this.size = size;
        this.wallRotation = Quaternion.Euler(rotation);
        this.wallName = name;
    }

    public bool GetCellAtPoint(Vector3 localPoint, out int x, out int y)
    {
        // Rotate the point to align with the grid
        Vector3 point = Quaternion.Inverse(wallRotation) * localPoint;

        switch (wallName)
        {
            case "Front wall":
            case "Back wall":
                x = Mathf.FloorToInt(point.x + size / 2f);
                y = Mathf.FloorToInt(point.y + size / 2f);
                break;
            case "Left wall":
            case "Right wall":
                x = Mathf.FloorToInt(point.z + size / 2f);
                y = Mathf.FloorToInt(point.y + size / 2f);
                break;
            case "Top wall":
            case "Floor wall":
                x = Mathf.FloorToInt(point.x + size / 2f);
                y = Mathf.FloorToInt(-point.z + size / 2f);  // **Negate Z to correct mirroring**
                break;
            default:
                x = y = -1;
                return false;
        }

        if (x >= 0 && x < size && y >= 0 && y < size)
        {
            return true;
        }
        else
        {
            x = y = -1;
            return false;
        }
    }
}
