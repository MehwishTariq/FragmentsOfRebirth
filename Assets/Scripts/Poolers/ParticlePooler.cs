using System.Collections.Generic;
using UnityEngine;

public class ParticlePooler : ObjectPooler<ParticleSystem>
{
    public List<ParticlesObjectData> particleFxList;
    void Start()
    {
        ObjectsToPool = new List<IPoolData<ParticleSystem>>(particleFxList);
        InitializePool();
    }
    
    public void PlayFx(Utility.ParticleFx name, Vector3 localPos, Vector3 localRot, Vector3 localScale)
    {
        IPoolData<ParticleSystem> poolData = particleFxList.Find(item => item.fxName == name);
        ParticleSystem fx = SetPostion_Rotation(poolData,localPos, localRot, null,true);
        fx.transform.localScale = localScale;
        fx.gameObject.SetActive(true);
        fx.GetComponent<ParticleSystem>().Play(true);        
    }

    public void StopFx(Utility.ParticleFx name)
    {
        while (true)
        {
            ParticleSystem playingFx = GetEnabledOneFromPool(name.ToString());
            if (playingFx == null)
                break;
                
            if (playingFx.isPlaying)
                playingFx.Stop();
        }
    }
}

[System.Serializable]
public class ParticlesObjectData : IPoolData<ParticleSystem>
{
    public Utility.ParticleFx fxName;
    public ParticleSystem fxPrefab;
    public int NumberToPool;

    public string GetKey() => fxName.ToString();
    public ParticleSystem GetObject() => fxPrefab;
    public int GetPoolCount() => NumberToPool;
}
