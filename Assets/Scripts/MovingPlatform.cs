using DG.Tweening;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 moveToPos;
    public float movSpeed;
    public float decreaseHealth = 0.03f;

    void Start()
    {
        transform.DOLocalMove(moveToPos, movSpeed).SetLoops(-1, LoopType.Yoyo);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(Utility.Character.Player.ToString()))
        {
            PlayerHealth.PlayerHit?.Invoke(decreaseHealth);
        }
    }
}
