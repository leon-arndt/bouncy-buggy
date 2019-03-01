using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 0f;
    float direction = 0f;
    bool doubleTurn = false;
    private int playerID;

    float maxSpeed = 32f; // was 25 before
    float acceleration = 0.25f; //was 0.15 before
    float turnSpeed = 150f;
    public float jumpForce = 500f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();    
    }

    // Update is called once per frame
    void Update()
    {
        //rotation decay (prevents the robot from tipping over)
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, 0)), 0.001f * Time.time);

        //slowly reset the camera to normal
        transform.GetChild(0).GetComponent<Camera>().transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z));
        this.gameObject.transform.GetChild(0).GetComponent<Camera>().transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, 0)), 0.001f * Time.time);

        //speed decay
        speed *= 0.97f;

        //movement
        if (Input.GetKey(KeyCode.W))
        {
            if (speed < maxSpeed)
            {
                speed += acceleration;
            }
        }
        //else
        //flying
        if (!Physics.Raycast(transform.position, Vector3.down, 1f))
        {
            Debug.Log("Can fly");
            if (Input.GetKey(KeyCode.S) && speed > 15f)
            {
                transform.Rotate(turnSpeed * Vector3.left * Time.deltaTime);

                Vector3 lookDir = transform.position - 5f * Vector3.up - this.gameObject.transform.GetChild(0).GetComponent<Camera>().transform.position;
                lookDir.y = -5f; // keep only the horizontal direction
                                    //transform.rotation = Quaternion.LookRotation(lookDir);
                this.gameObject.transform.GetChild(0).GetComponent<Camera>().transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z));
                this.gameObject.transform.GetChild(0).GetComponent<Camera>().transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDir), 0.0005f * Time.time);
                //this.gameObject.transform.GetChild(0).GetComponent<Camera>().transform.LookAt(transform);

                if (speed < maxSpeed)
                {
                    speed += 1.05f * acceleration;
                }
            }
            doubleTurn = false;
        }
        else //must be grounded
        {
            doubleTurn = (Input.GetKey(KeyCode.S));

            if (Input.GetKey(KeyCode.Space))
            {
                rb.AddForce(jumpForce * Vector3.up);
            }

        }

        float factor = doubleTurn ? 2f : 1f;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(turnSpeed * Vector3.down * factor * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(turnSpeed * Vector3.up * factor * Time.deltaTime);
        }

        //update fov according to speed
        Camera cam = this.gameObject.transform.GetChild(0).GetComponent<Camera>();
        float desiredFov = Mathf.Lerp(cam.fieldOfView, Mathf.Min(110, 70f + speed + rb.velocity.magnitude), 4f * Time.deltaTime);
        cam.fieldOfView = desiredFov;
    }

    private void FixedUpdate()
    {
        //move the player forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
