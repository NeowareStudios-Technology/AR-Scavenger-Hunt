using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
	
	public float rotateSpeed = 10f;
    public float angle = 0;

    void Update()
    {
        transform.Rotate(new Vector3(0, 180, 0), angle * Time.deltaTime, Space.World );
    }
}
