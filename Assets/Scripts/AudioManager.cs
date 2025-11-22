using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public AudioPooler AudioPool;
    public AudioListener audioListener;

    public void PlayClickSound()
    {
        AudioPool.PlayAudio(Utility.SoundName.Click);
    }

    public void PlayJumpSound()
    {
        AudioPool.PlayAudio(Utility.SoundName.Jump);
    }

    public void PlayMusic()
    {
        StopMusic();
        AudioPool.PlayMusic(Utility.SoundName.BGMusic);
    }

    public void StopMusic()
    {
        AudioPool.StopAudio(Utility.SoundName.BGMusic);
    }
}
