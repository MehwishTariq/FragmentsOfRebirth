using UnityEngine;

public class Utility : MonoBehaviour
{
    public static readonly string INPUTX = "InputX";
    public static readonly string INPUTZ = "InputZ";
    public static readonly string PLAYERSTATE = "Mode";

    public enum AnimationStates
    {
        MoveX,
        MoveY,
        Attack,
        Jump,
    }

    public enum ParticleFx
    {
        JumpFx,
        FireFx,
        GlowFx,

    }

    public enum ObjectType
    {
        Debris,
        Bullet,
    }
    public enum SoundName
    {
        Jump,
        Collect,
        Click,
BGMusic
    }

    public enum Character
    {
        Player,
        SmallDemon,
    }


    [System.Serializable]
    public class ObjectSpawner
    {
        public Utility.Character ObjectToSpawn;
        public Transform PositionToSpawnAt;
    }

    public static bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return ((1 << obj.layer) & mask) != 0;
    }


    public static void SetAnimation(Animator anim, AnimationStates animState, float float_val)
    {
        anim.SetFloat(animState.ToString(), float_val);
    }

    public static void SetAnimation(Animator anim, AnimationStates animState, int int_val)
    {
        anim.SetInteger(animState.ToString(), int_val);
    }

    public static void SetAnimation(Animator anim, AnimationStates animState, bool bool_val)
    {
        anim.SetBool(animState.ToString(), bool_val);
    }
    public static void SetAnimation(Animator anim, AnimationStates animState)
    {
        anim.SetTrigger(animState.ToString());
    }

    public static void ResetAnimationTrigger(Animator anim, AnimationStates animState)
    {
        anim.ResetTrigger(animState.ToString());
    }
    public float GetAnimationFloat(Animator anim, AnimationStates animState)
    {
        return anim.GetFloat(animState.ToString());
    }

    public int GetAnimationInt(Animator anim, AnimationStates animState)
    {
        return anim.GetInteger(animState.ToString());
    }

    public bool GetAnimationBool(Animator anim, AnimationStates animState)
    {
        return anim.GetBool(animState.ToString());
    }
}

    public interface IPoolData<T> where T:Component
    {
        public string GetKey();
        public T GetObject();

        public int GetPoolCount();
    }
    
