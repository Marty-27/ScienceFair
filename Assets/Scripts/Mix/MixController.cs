using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MixController : MonoBehaviour
{
    public ColorMixing colorMixing;

    public void Reset()
    {
        colorMixing.Reset();

        GameObject[] buckets = GameObject.FindGameObjectsWithTag("Bucket");
        foreach(GameObject bucket in buckets) {
            PaintBucket paintBucket = bucket.GetComponent<PaintBucket>();
            paintBucket.Reset();
        }
    }
    public void Quit()
    {
        SceneManager.LoadScene("ScienceFair");
    }
}
