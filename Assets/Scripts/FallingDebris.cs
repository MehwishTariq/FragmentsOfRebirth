using System.Collections;
using UnityEngine;

public class FallingDebris : MonoBehaviour
{
    public bool isParent;
    public float decreaseHealth = 0.002f;
    bool triggered;

    void OnEnable()
    {
        if (!isParent)
        {
            StartCoroutine(CheckIfNoCollision());
        }
    }

    IEnumerator CheckIfNoCollision()
    {
        yield return new WaitForSeconds(5f);
        FallingObjectPooler.Instance.ReturnToPool(GetComponent<Rigidbody>());
        triggered = false;        

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Utility.Character.Player.ToString()))
        {
            if (!triggered)
            {
                triggered = true;
                FallingObjectPooler.Instance.StartFallingObjects(Utility.ObjectType.Debris, 4, new Vector3(Random.Range(-1f, 1f), Random.Range(-2f, 2f), 0) , Quaternion.identity, transform);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Utility.Character.Player.ToString()))
            triggered = false;        
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag(Utility.Character.Player.ToString()))
        {
            if (!triggered)
            {
                triggered = true;
                PlayerHealth.PlayerHit?.Invoke(decreaseHealth);
            }
        }
    }
}
