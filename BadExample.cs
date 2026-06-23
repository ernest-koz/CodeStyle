using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoPlaces : MonoBehaviour
{
    public float _float;



    public Transform AllPlacespoint;
    Transform[] arrayPlaces;
    private int NumberOfPlaceInArrayPlaces;
    void Start() {
        arrayPlaces = new Transform[AllPlacespoint.childCount];

        for (int abcd = 0; abcd < AllPlacespoint.childCount; abcd++)
            arrayPlaces[abcd] = AllPlacespoint.GetChild(abcd).GetComponent<Transform>();
        }
    // Update is called once per frame
    public void Update()
    {
        var _pointByNumberInArray= arrayPlaces[NumberOfPlaceInArrayPlaces];
        transform.position   =  Vector3.MoveTowards(transform.position , _pointByNumberInArray.position, _float * Time.deltaTime);


          if (transform.position == _pointByNumberInArray.position)  NextPlaceTakerLogic();
    }
    public Vector3 NextPlaceTakerLogic(){
        NumberOfPlaceInArrayPlaces++;

            if (NumberOfPlaceInArrayPlaces == arrayPlaces.Length)
                NumberOfPlaceInArrayPlaces  = 0;

        var thisPointVector = arrayPlaces[NumberOfPlaceInArrayPlaces].transform.position;
        transform.forward = thisPointVector - transform.position;
        return thisPointVector;

        
    }


}