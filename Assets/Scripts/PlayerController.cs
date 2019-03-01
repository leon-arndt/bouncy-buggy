using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player script. Needs to stay under 150 lines.
/// </summary>
public class PlayerController : MonoBehaviour
{
    public float speed = 0f;

    float maxSpeed = 36f; // was 32 before
    float acceleration = 0.36f; //was 0.25 before
    float turnSpeed = 150f;
    private float jumpForce = 500f;
    public float jumpTorqueFactor = 2f;
    public bool tippedOver;
    public bool flipped;

    private Rigidbody rb;
    private Vector3 startPosition;

    [SerializeField]
    private Transform rotateAxel;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //rotation decay (prevents the robot from tipping over)
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, 0)), 0.001f * Time.time);

        //slowly reset the camera to normal
        transform.GetChild(0).GetComponent<Camera>().transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z));
        this.gameObject.transform.GetChild(0).GetComponent<Camera>().transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, 0)), 0.001f * Time.time);

        //accelerate
        if (Input.GetKey(KeyCode.W))
        {
            if (speed < maxSpeed)
            {
                speed = Mathf.Min(speed + acceleration, maxSpeed);
            }
            //speed decay
            speed *= 0.98f;
        }
        else
        {
            //speed decay
            speed *= 0.97f;
        }

        //flying through the skies
        if (!Physics.Raycast(transform.position, Vector3.down, 1f))
        {
        }
        else //must be grounded
        {
            //figure out if tippedOver
            if (Physics.Raycast(transform.position, transform.right, 1f) || Physics.Raycast(transform.position, -transform.right, 1f))
            {
                tippedOver = true;
            }
            else
            {
                tippedOver = false;
            }

            //flip
            if (Input.GetKey(KeyCode.S))
            {
                transform.RotateAround(rotateAxel.position, -10f * transform.right, 50 * Time.deltaTime);
            }

            //action
            if (Input.GetKey(KeyCode.Space))
            {
                if (!tippedOver)
                {
                    rb.AddForce(jumpForce * Vector3.up);
                    rb.AddRelativeTorque(jumpTorqueFactor * Random.insideUnitSphere);
                }
                else
                {
                    StartCoroutine(TurnUpright());
                }
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(turnSpeed * Vector3.down * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(turnSpeed * Vector3.up * Time.deltaTime);
        }

        //update camera fov according to speed
        Camera cam = this.gameObject.transform.GetChild(0).GetComponent<Camera>();
        float desiredFov = Mathf.Lerp(cam.fieldOfView, Mathf.Min(110, 70f + speed + rb.velocity.magnitude), 4f * Time.deltaTime);
        cam.fieldOfView = desiredFov;

        //respawn if dead
        if (transform.position.y < -4)
        {
            transform.position = startPosition;
            transform.rotation = Quaternion.Euler(Vector3.zero);
            rb.velocity = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        //move the player forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    IEnumerator TurnUpright()
    {
        float desiredZ = 0f;
        for (float f = 0f; f <= 1; f += 0.01f)
        {
            float newZ = Mathf.Lerp(transform.rotation.z, desiredZ, f);
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, newZ));
            yield return null;
        }
    }

    private void RotateTowardsTarget(Vector3 target, float speed)
    {
        Vector3 targetPos = target;
        Vector3 relativePos = targetPos - transform.position;

        Quaternion desiredRot = Quaternion.LookRotation(relativePos, Vector3.up);
        Quaternion newRot = Quaternion.Lerp(transform.rotation, desiredRot, speed * Time.deltaTime);
        transform.rotation = newRot;
    }
}