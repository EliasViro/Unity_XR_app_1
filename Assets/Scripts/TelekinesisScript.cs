using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class TelekinesisScript : MonoBehaviour
{
    public GameObject hand;
    public InputActionReference rotationReference = null;
    public TextMeshProUGUI textWindow;
    private GameObject grabbedTarget = null;

    // Update is called once per frame
    void Update()
    {
        float value = rotationReference.action.ReadValue<float>();
        RaycastHit hit;
        textWindow.text = "Angle: " + value.ToString();

        if (Physics.Raycast(hand.transform.position, hand.transform.forward, out hit, 10))
        {
            if (hit.transform.CompareTag("TelekinesisTarget"))
            {

                if ((value >= 0.50f || value <= -0.50f) && hit.transform.parent == null) // Grab
                {
                    grabbedTarget = hit.transform.gameObject;
                    grabbedTarget.transform.GetComponent<Rigidbody>().useGravity = false;
                    grabbedTarget.transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    grabbedTarget.transform.parent = hand.transform;
                    textWindow.text = "Force ON";

                }

                else if ((value < 0.50f && value > -0.50f) && hit.transform.parent != null) // Drop
                {
                    grabbedTarget.transform.GetComponent<Rigidbody>().useGravity = true;
                    grabbedTarget.transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    grabbedTarget.transform.parent = null;
                    grabbedTarget = null;
                    textWindow.text = "Object dropped";
                }
            }
            else if (grabbedTarget != null)
            {
                grabbedTarget.transform.GetComponent<Rigidbody>().useGravity = true;
                grabbedTarget.transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                grabbedTarget.transform.parent = null;
                grabbedTarget = null;
                textWindow.text = "Raycast lost";
            }
        }
        else if (grabbedTarget != null)
        {
            grabbedTarget.transform.GetComponent<Rigidbody>().useGravity = true;
            grabbedTarget.transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            grabbedTarget.transform.parent = null;
            grabbedTarget = null;
            textWindow.text = "Raycast inf";
        }
    }
}
