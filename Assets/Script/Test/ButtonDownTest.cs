using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDownTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnMouseDown()
    {
        Debug.Log("DOWN 0");
        if (Input.GetKey(KeyCode.P))
        {
            Debug.Log("DOWN P");
        }
    }
}
