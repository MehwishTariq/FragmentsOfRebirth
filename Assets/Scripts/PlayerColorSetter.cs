using System.Collections.Generic;
using UnityEngine;

public class PlayerColorSetter : MonoBehaviour
{
    public List<Color> levelWiseColors;
    public Material PlayerMat;

    void Start()
    {
        SetColor();
    }
    
    void SetColor()
    {
        PlayerMat.SetColor("_EmissionColor", levelWiseColors[GameManager.Instance.LevelNo]);
    }
}
