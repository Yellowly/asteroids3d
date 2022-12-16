using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public Transform ship;
    public Transform aim;
    public Transform cameraRig;
    public Transform cam;
    public float mouseSensitivity;
    public float smoothSpeed;
    public float aimDistance = 500;
    public bool paused = false;
    public PauseMenu pauseMenu;
    IEnumerator fade;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

        if (paused == false)
        {
            updatePos();
            updateRotation();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                paused = true;
                pauseMenu.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape)||Input.GetMouseButtonDown(0))
            {
                paused = false;
                pauseMenu.gameObject.SetActive(false);
                Time.timeScale = 1;
            }
        }
        //Debug.Log(pointer);
    }

    public Vector3 shipAimPos //position for the ship's cursor. this is where bullet will travel to
    {
        get
        {
            return (ship.forward * aimDistance) + ship.transform.position;
        }
    }
    public Vector3 pointer //position of the mouse pointer itself. this is where the ship will try to rotate to face towards
    {
        get
        {
            return aim.position + (aim.forward * aimDistance);
        }
    }
    public RaycastHit cursorTarget //used for improved bullet aiming.
    {
        get
        {
            RaycastHit ray = new RaycastHit();
            Physics.Raycast(cam.position+ship.forward*3, shipAimPos.normalized, out ray, 200);
            return ray;
        }
    }
    //rotates the "pointer" gameobject to aim based on mouse movement
    void updateRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity;
        aim.Rotate(cam.right, mouseY, Space.World);
        aim.Rotate(cam.up, mouseX, Space.World);
        cameraRig.rotation = Quaternion.Slerp(cameraRig.rotation, Quaternion.LookRotation(aim.forward, Vector3.up), 1 - Mathf.Exp(-smoothSpeed * Time.deltaTime));
    } //this last part is a framerate independant lerp function that i found on google

    //moves the MouseController to the ship
    void updatePos()
    {
        transform.position=ship.position;
    }
}
