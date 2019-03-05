using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateGameobjectAfterXSeconds : MonoBehaviour
{
    
    private bool rise = false;

    void OnEnable()
    {
        StartCoroutine(WaitThenMoveUp());
    }
    
    void Update()
    {
        if (rise)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * 100);    
        }
    }
    private IEnumerator WaitThenMoveUp()
    {
        yield return new WaitForSeconds(3.0f);
        rise = true;
        yield return new WaitForSeconds(3.0f);
        this.gameObject.SetActive(false);
    }
}
