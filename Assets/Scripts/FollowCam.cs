using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public PlayerController target;
    private Camera cam;
    public Vector3 offset; //0, 1.32, -1.41
    public float cameraSpeed;
    public float minDistance;
    public float hoverDistance;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //position
        if (Vector3.Distance(transform.position, target.transform.position) > minDistance)
        {
            //this factor makes sure that the lerp slows down as the camera gets closer to stopping completly
            float desiredTravelFactor = Mathf.Min(cameraSpeed, Vector3.Distance(transform.position, target.transform.position) - minDistance);

            Vector3 desiredPos = target.transform.position - hoverDistance * target.transform.forward + offset;

            transform.position = Vector3.Lerp(transform.position, desiredPos, desiredTravelFactor * Time.deltaTime);
        }

        //rotation
        transform.LookAt(target.transform.position + hoverDistance * target.transform.forward);
        
        //update camera fov according to speed
        float desiredFov = Mathf.Lerp(cam.fieldOfView, Mathf.Min(110, 70f + target.speed), 4f * Time.deltaTime); //+ rb.velocity.magnitude
        cam.fieldOfView = desiredFov;
    }
}
