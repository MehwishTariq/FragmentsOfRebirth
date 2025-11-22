using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FallingPlatform : MonoBehaviour
{
    Vector3 startPos;
    Rigidbody rb;
    Coroutine crt;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        rb.useGravity = false;
    }

    void OnTriggerEnter(Collider other)    
    {
        if (other.CompareTag(Utility.Character.Player.ToString()))
        {
            rb.useGravity = true;
            if (crt == null)
                crt = StartCoroutine(PlacePlatformBack());
        }
    }

    IEnumerator PlacePlatformBack()
    {
        yield return new WaitForSeconds(3f);
        rb.isKinematic = true;
        rb.useGravity = false;
        transform.DOMove(startPos, 0.2f).OnComplete(() =>
        {
            rb.isKinematic = false;            
        });
        crt = null;
    }
}
