using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inspectionObjects : MonoBehaviour
{
    public GameObject[] inspectionObj;
    private int currentINdex;

    public void TurnonInspection(int index)
    {
        currentINdex = index;
        inspectionObj[index].SetActive(true);
    }
    public void turnoffInspection()
    {
        inspectionObj[currentINdex].SetActive(false);
    }
}
