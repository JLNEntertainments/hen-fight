using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    [SerializeField] private GameObject _mint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void RotationClickNext()
    {
        _mint.transform.Rotate(0f, 60f, 0f);
        
    }

    public void RotationClickPre()
    {
        _mint.transform.Rotate(0f, -60f, 0f);
    }
}
