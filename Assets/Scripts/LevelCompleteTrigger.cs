using UnityEngine;

public class LevelCompleteTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Utility.Character.Player.ToString()))
        {
            GameManager.Instance.LevelComplete?.Invoke();
        }
    }
}
