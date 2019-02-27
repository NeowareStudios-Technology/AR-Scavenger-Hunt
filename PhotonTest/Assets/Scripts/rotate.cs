using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    private float speed = 30.0f;
    void Update()
    {
        transform.Rotate(new Vector3 (1, 0, 0), speed * Time.deltaTime);
    }
}
