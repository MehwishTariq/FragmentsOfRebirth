using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float MaxHealth = 20f;
    private float currentHealth = 0;
    public static Action<float> PlayerHit;

    void OnEnable()
    {
        PlayerHit += DecreaseHealth;
        PlayerHealth.PlayerHit?.Invoke(-MaxHealth);
    }

    void OnDisable()
    {
        PlayerHit -= DecreaseHealth;
    }

    void DecreaseHealth(float val)
    {
        currentHealth -= val;
        if(currentHealth <= 0)
        {
            GameManager.Instance.LevelFail?.Invoke();
        }
    }
}
