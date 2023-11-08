using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Temporary script for moving camera view in Unity Editor.
/// </summary>
public class CameraTempMove : MonoBehaviour {

    // Rotate speed.
    public float speed = 10f;
	
	// Update is called once per frame
	void Update () {

        gameObject.transform.parent.Rotate(Vector3.up, Input.GetAxis("Horizontal") * Time.deltaTime * speed);

        gameObject.transform.Rotate(Vector3.left, Input.GetAxis("Vertical") * Time.deltaTime * speed);
    }
}
