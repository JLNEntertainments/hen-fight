using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class HoverObject : MonoBehaviour
{
    public GameObject inspection;
    public inspectionObjects inspectionobj;
    public int index;

    // Update is called once per frame
    void Update()
    {
        /*if (inspection.activeInHierarchy && SceneManager.GetActiveScene().buildIndex == 0) 
            return;
        Ray ray = Camera.main.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Color color = GetComponent<MeshRenderer>().material.color;
        if (GetComponent<Collider>().Raycast(ray, out hit, 100f))
        {
            color.a = 0.6f;
            if (Input.GetMouseButtonDown(0))
            {
                inspection.SetActive(true);
                inspectionobj.TurnonInspection(index);
            } 
        }
        else
        {
            color.a = 1.0f;
        }
        GetComponent<MeshRenderer>().material.color = color;
       

*/
    }
}
