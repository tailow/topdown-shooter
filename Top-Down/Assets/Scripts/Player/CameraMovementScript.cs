using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour {

    #region Variables

    public float smoothTime;

    Vector3 velocity = Vector3.zero;

    Transform playerTransform;
    Transform cameraNewPosition;

	#endregion

	void Start () 
	{
        // PLAYER TRANSFORM
        playerTransform = GameObject.Find("PlayerModel").GetComponent<Transform>();

        // CAMERA NEW POSITION
        cameraNewPosition = GameObject.Find("CameraNewPosition").GetComponent<Transform>();

    }
	
	void Update () 
	{
        // LOOK AT PLAYER
        transform.LookAt(playerTransform);

        // MOVE CAMERA
        transform.position = Vector3.SmoothDamp(transform.position, cameraNewPosition.position, ref velocity, smoothTime);
    }
}
