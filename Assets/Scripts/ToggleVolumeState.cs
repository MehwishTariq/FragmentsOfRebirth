using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class ToggleVolumeState : MonoBehaviour
{
    public Sprite volOn, volOff;

    void Start()
    {
        Button volBtn = GetComponent<Button>();
        Image volImg = GetComponent<Image>();
        volBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.audioListener.enabled = !AudioManager.Instance.audioListener.enabled;
            if (AudioManager.Instance.audioListener.enabled)
                volImg.sprite = volOn;
            else
                volImg.sprite = volOff;
        });
    }
}
