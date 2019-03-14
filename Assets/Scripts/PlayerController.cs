using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player script. Needs to stay under 150 lines.
/// </summary>
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public float speed = 0f;
    float maxSpeed = 36f; // was 32 before
    float acceleration = 0.4f; //was 0.36f before
    float turnSpeed = 150f;
    private float jumpForce = 500f;
    public float jumpTorqueFactor = 2f;
    public bool tippedOver;
    public bool flipped;

    private Rigidbody rb;
    private Vector3 startPosition;

    [SerializeField] Camera mainCamera;

    private void Start()
    {
        Instance = this;

        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;

        UpdateDistanceUI();
    }

    // Update is called once per frame
    void Update()
    {
        //rotation decay (prevents the robot from tipping over)
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, 0)), 0.001f * Time.time);

        //accelerate
        if (Input.GetKey(KeyCode.W))
        {
            if (speed < maxSpeed)
            {
                speed = Mathf.Min(speed + acceleration, maxSpeed);
            }
        }
        else
        {  
        }

        //flip
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(0.06f * jumpForce * Vector3.up);
            rb.AddRelativeTorque(100f * Vector3.left);
        }

        //flying through the skies
        if (!Physics.Raycast(transform.position, Vector3.down, 1f))
        {
            //weak speed decay
            speed *= 0.98f;
        }
        else //must be grounded
        {
            //strong speed decay
            speed *= 0.97f;

            //figure out if tippedOver
            tippedOver = Physics.Raycast(transform.position, transform.right, 1f) || Physics.Raycast(transform.position, -transform.right, 1f);

            //action
            if (Input.GetKey(KeyCode.Space))
            {
                if (!tippedOver)
                {
                    rb.AddForce(jumpForce * Vector3.up);
                    rb.AddRelativeTorque(jumpTorqueFactor * Random.insideUnitSphere);
                }
            }

            //untip
            if (Input.GetKey(KeyCode.Return))
            {
                StartCoroutine(TurnUpright());
            }
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(turnSpeed * horizontalInput * Vector3.up * Time.deltaTime);

        //reload and respawn
        if (Input.GetKey(KeyCode.R) || transform.position.y < -4f)
        {
            transform.position = LevelManager.Instance.GetPlayerStart();
            transform.rotation = Quaternion.Euler(0f, -270f, 0f);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            ResetManager.Instance.ResetAll();
        }
    }

    private void FixedUpdate()
    {
        //move the player forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        //performant distance check (calls at most 50 times per second and does not call if buggy is still)
        //also has improved performance because it omits the square root call
        if (speed > 1f)
        {
            UpdateDistanceUI();
        }

        AudioManager.Instance.UpdateEngineSound(speed);
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

    private void UpdateDistanceUI()
    {
        Vector3 offset = Goal.Instance.transform.position - transform.position;
        float sqrLen = offset.sqrMagnitude;
        UiController.Instance.UpdateDistanceText(sqrLen);
    }
}