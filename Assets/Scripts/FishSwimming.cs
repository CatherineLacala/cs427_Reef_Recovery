using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
//using System.Threading.Tasks.Dataflow;
using UnityEngine;

public class FishSwimming : MonoBehaviour
{
    public float swimmingSpeed = 1.5f;
    //public float turningSpeed = 2.0f;
    public float dirChangeTime = 4.0f;
    //public float objObstacleDist = 2.0f;
    public float wigglingAmount = 8f;
    public float wiggleSpeed = 4f;
    public float circleTurningSpeed = 20f; //degrees per second
    private float timer;
    //private float rotationBase;
    private float currTurnSpeed;
    // Start is called before the first frame update
    void Start()
    {
        // rotationBase = transform.eulerAngles.y;
        chooseNewMovement();
    }

    // Update is called once per frame
    void Update()
    {
        //moving forward
        //transform.Translate(Vector3.forward * swimmingSpeed * Time.deltaTime);
        transform.position += transform.right * swimmingSpeed * Time.deltaTime;

        //gradually turning in a circle swimming motion
        transform.Rotate(0, currTurnSpeed * Time.deltaTime, 0);

        //fish wiggling swimming motion
        float wiggle = Mathf.Sin(Time.time * wiggleSpeed) * wigglingAmount;
        transform.Rotate(0, wiggle * Time.deltaTime, 0);

        // Vector3 rotate = transform.eulerAngles;
        // rotate.y = rotationBase + wiggle;
        // transform.eulerAngles = rotate;

        //randomly changing swimming direction
        timer += Time.deltaTime;
        if (timer > dirChangeTime)
        {
            chooseNewMovement();
            //rotationBase += Random.Range(-45f, 45f);
            // rotationBase += 90f; //rotate 180 degrees
            timer = 0;
        }
    }

    void chooseNewMovement()
    {
        //sometimes swimming straight, sometimes circling
        int moveType = Random.Range(0,2);

        if (moveType == 0)
        {
            //straight swimming
            currTurnSpeed = 0f; 
        }
        else
        {
            currTurnSpeed = Random.Range(-circleTurningSpeed, circleTurningSpeed);
        }
    }
}
