using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PestleMove : MonoBehaviour
{
    bool moveUp = true;

    private void OnEnable()
    {
        StartCoroutine(MovePestleUpAndDown());
    }
    private IEnumerator MovePestleUpAndDown()
    {
        while(true)
        {
            moveUp = true;
            yield return new WaitForSeconds(1.0f);
            moveUp = !moveUp;
            yield return new WaitForSeconds(1.0f);
        }
       
    }
    private void FixedUpdate()
    {
        if (moveUp)
        {
            // Move the object forward along its z axis 1 unit/second.
            transform.Translate(Vector3.forward * Time.deltaTime);
        }
        else
        {
            // Move the object forward along its z axis 1 unit/second.
            transform.Translate(Vector3.back * Time.deltaTime);
        }
    }
}
