using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AudioPooler : ObjectPooler<AudioSource>
{
    public List<Sounds> soundList;

    private void Start()
    {
        ObjectsToPool = new List<IPoolData<AudioSource>>(soundList);
        InitializePool();
    }

    AudioSource SetAudioData(Utility.SoundName name)
    {
        Sounds data = soundList.Find(data => data.name == name);
        AudioSource audioToPlay = GetFromPool(data);
        audioToPlay.clip = data.clip;
        audioToPlay.volume = data.volume;
        audioToPlay.pitch = data.pitch;
        audioToPlay.loop = data.loop;
        audioToPlay.playOnAwake = data.playOnAwake;
        return audioToPlay;
    }

    public void PlayAudio(Utility.SoundName name)
    {
        StartCoroutine(PlayAudioRoutine(name));
    }
    
    IEnumerator PlayAudioRoutine(Utility.SoundName name)
    {
        AudioSource audioToPlay = SetAudioData(name);
        audioToPlay.Play();
        yield return new WaitForSeconds(audioToPlay.clip.length);
        ReturnToPool(audioToPlay);
    }

    public void PlayMusic(Utility.SoundName name)
    {
        AudioSource audioToPlay = SetAudioData(name);
        float correctVol = audioToPlay.volume;
        audioToPlay.volume = 0;
        audioToPlay.DOFade(correctVol, 0.3f);
        audioToPlay.Play();
    }

    public void StopAudio(Utility.SoundName name)
    {
        while (true)
        {
            AudioSource playingAudio = GetEnabledOneFromPool(name.ToString());
            if (playingAudio == null)
                break;

            if (playingAudio.isPlaying)
                playingAudio.Stop();

            ReturnToPool(playingAudio);
        }
    }
}

[System.Serializable]
public class Sounds : IPoolData<AudioSource>
{
    public Utility.SoundName name;

    public AudioClip clip;

    [Range(0f, 1f)] public float volume = 0.5f;

    [Range(-3f, 3f)] public float pitch;

    public bool loop = false;
    public bool playOnAwake = false;
    public int PoolCount;
    public string GetKey()
    {
        return name.ToString();
    }

    public AudioSource GetObject()
    {
        return null;
    }

    public int GetPoolCount()
    {
        return PoolCount;
    }
}