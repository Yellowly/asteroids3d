using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hud : MonoBehaviour
{
    public MouseController mouseController; 

    public RectTransform mousePos;
    public RectTransform shipAim;

    private Camera playerCam = null;
    void Start()
    {
        playerCam = mouseController.cam.GetComponent<Camera>();
    }

    void Update()
    {
        updateHud(mouseController);
    }

    //this just draws the hud
    void updateHud(MouseController controller)
    {
        if (shipAim != null)
        {
            shipAim.position = playerCam.WorldToScreenPoint(controller.shipAimPos);
        }
        if (mousePos != null)
        {
            mousePos.position = playerCam.WorldToScreenPoint(controller.pointer);
        }
    }
}
