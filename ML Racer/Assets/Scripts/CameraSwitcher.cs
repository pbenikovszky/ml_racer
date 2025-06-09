using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{

    [SerializeField] private Camera[] cameras;
    private int currentCameraIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (cameras.Length == 0)
        {
            Debug.LogError("No cameras assigned to CameraSwitcher.");
            return;
        }

        cameras[0].gameObject.SetActive(true); // Enable the first camera

        // Disable all cameras except the first one
        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchCamera();
        }
    }

    private void SwitchCamera()
    {
        currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == currentCameraIndex);
        }
    }
}
