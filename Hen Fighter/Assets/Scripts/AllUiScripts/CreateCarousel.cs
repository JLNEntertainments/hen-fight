using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;


public class CreateCarousel : MonoBehaviour
{

    public GameObject[] carouselObjects;//the elements of the carousel
    
  
    public bool ResetCenterRotation = true;//do you want to reset the rotation of the carousel center (recommended to be true)
    public float DistanceFromCenter = 10.0f;//the distance from the center of the carousel
    public bool AssumeObject = true; // if true assume the object that is picked, otherwise (false) keep checking what the next item is through raycast.
    public int ChosenObject = 0; //index of the object that is centered in the carousel
    
    public float speedOfrotation = 0.1f; //the speed in which the carousel rotates: values should be between 0.01f -> 1.0f, zero will stop the rotation
    
    Quaternion newRotation;
    RaycastHit hit;

    private static float diameter = 360.0f; //the diameter is always 360 degrees
    private Transform theRayCaster = null; //create an empty transform
    private float Angle = 0.0f; //the angle for each object in the carousel
    private float newAngle = 0.0f; //the calculated angle
    private bool firstTime = true; //used to calculate the offset for the first time

    void Start()
    {
        Debug.Log(carouselObjects[0].name);//just display the name of the first chosen element in the console
       
        GameObject raycastHolder = new GameObject();//create an empty gameobject
        raycastHolder.name = "RaycastPicker"; //rename it to RaycastPicker
        theRayCaster = raycastHolder.transform; // assign the transform of the newlycreated gameobject to the raycast transform variable
        theRayCaster.position = transform.position; // place it at the positon of the carousel center
        if (ResetCenterRotation)
        {
            transform.rotation = Quaternion.identity;//reset the rotation of the carousel center
        }

        Angle = diameter / (float)carouselObjects.Length;//calculate the angle according to the number of elements
        float ObjectAngle = Angle;//create a temp value that keeps track of the angle of each element
        for (int i = 0; i < carouselObjects.Length; i++)
        { //loop through the objects

            carouselObjects[i].transform.position = this.transform.position;//Reset objects to the postion of the carousel center
            carouselObjects[i].transform.rotation = Quaternion.identity; //make sure their rotation is zero
            carouselObjects[i].transform.parent = this.transform; // make the element child to the carousel center
            carouselObjects[i].transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + DistanceFromCenter);//move each carousel item from the center an amount of "DistanceFromCenter"
            carouselObjects[i].transform.RotateAround(this.transform.position, new Vector3(0, 1, 0), ObjectAngle);//position the element in their respective locations accordind to the center throufh rotation
          
            ObjectAngle += Angle;//calculate the next angle value
        }

        //Make sure an element is perfectly centered.
        if (carouselObjects.Length % 2 != 0)
        {
            float rotateAngle = Angle + Angle / 2;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotateAngle, transform.eulerAngles.z);
            newAngle = rotateAngle;
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, Angle, transform.eulerAngles.z);
            newAngle = Angle;
        }

        theRayCaster.position = transform.position;
        string objectName = "";
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -theRayCaster.forward, out hit, DistanceFromCenter))
        {
            objectName = hit.collider.name;
        }

        if (objectName != carouselObjects[0].name) // only work if the first item presented isn't the first item in the array
        {
            for (int c = 0; c < carouselObjects.Length; c++) //loop through the array
            {
                if (carouselObjects[c].name == objectName)
                {
                    float angleMultiplier = c++; //the array starts with zero so adding 1 to c gives the correct value
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + Angle * angleMultiplier, transform.eulerAngles.z); //rotate the carousel to center the first object in the array
                    newAngle = transform.eulerAngles.y; //reset the angle to the newly calculated angle

                    break; //exit the loop so it won't do any unecessary calculations
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (AssumeObject == false)
        {
            CheckSelectedItem();
        }
        UpdateRotation();
    }

    void CheckSelectedItem()
    {
        //use raycast to dynamically check which item is selected not recommended unless necessary
        theRayCaster.position = transform.position;
        if (Physics.Raycast(transform.position, -theRayCaster.forward, out hit, DistanceFromCenter))
        {
            Debug.Log(hit.collider.name);//display in the console which element is detected
                                         //HenName.text = carouselObjects[ChosenObject].name.ToString();
        }
    }

    void UpdateRotation()
    {
        newRotation = Quaternion.AngleAxis(newAngle, Vector3.up); // pick the rotation axis and angle
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, speedOfrotation);  //animate the carousel
    }

    public void rotateTheCarouselLeft() // call this function to rotate the carousel towards the left
    {
        if (firstTime)// if run the first time calcule the offset
        {
           newAngle = transform.eulerAngles.y;
           newAngle += Angle;
           firstTime = false; // stop this piece of code from running in the future
        }
        else
        {
            newAngle += Angle; //calculate the new angle
        }
        if (AssumeObject == true)//here we check which element is selected and if we reached the start of the array we reset the index
        {
            if (ChosenObject <= 0)
            {
                ChosenObject = carouselObjects.Length - 1;
            }
            else
            {
                ChosenObject--;
            }
            Debug.Log(carouselObjects[ChosenObject].name); //show in the console the name of the selected element
            //HenName.text = carouselObjects[ChosenObject].name.ToString();
        }
    }

    public void rotateTheCarouselRight()// call this function to rotate the carousel towards the right
    {

        if (firstTime) // if run the first time calcule the offset
        {
            newAngle = transform.eulerAngles.y;
            newAngle -= Angle;
            firstTime = false; // stop this piece of code from running in the future
        }
        else
        {
            newAngle -= Angle; //calculate the new angle
        }
        if (AssumeObject == true) //here we check which element is selected and if we reached the end of the array we reset the index
        {
            if (ChosenObject >= carouselObjects.Length - 1)
            {
                ChosenObject = 0; 
            }
            else
            {
                ChosenObject++;
            }
            Debug.Log(carouselObjects[ChosenObject].name); //show in the console the name of the selected element
            //HenName.text = carouselObjects[ChosenObject].name.ToString();
        }
    }
}