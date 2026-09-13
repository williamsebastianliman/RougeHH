using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(waitDestroy());
    }

    IEnumerator waitDestroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
