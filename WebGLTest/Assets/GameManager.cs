using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text Debugger;
    
    public Camera cam;

    public void GetDeviceOrientation(string deviceOrientation)
    {
        Debug.Log(deviceOrientation);
     //    Debugger.text = deviceOrientation;
    }

    public void GetDeviceZXYQuaternion(string value)
    {
        string[] rot = value.Split(';');
        Debugger.text = value;

        SetNewRotation(float.Parse(rot[0]), float.Parse(rot[1]), float.Parse(rot[2]), float.Parse(rot[3]));
    }

    public void SetNewRotation(float w, float x, float y, float z)
    {
        //cam.transform.rotation = Quaternion.Inverse( new Quaternion(x, y, z, w));
        Quaternion q = new Quaternion(x, y, -z, -w);
        cam.transform.rotation = q;
        cam.transform.Rotate(90f, 0f, 0f, Space.World);
    }

    private void LateUpdate()
    {
      
    }
}
