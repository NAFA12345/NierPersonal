using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{
    public enum BGM
    {
        Flag, MainField,
    }

    public enum SFX
    {
        EnemyShot, Raser, GundamAttack, ToFlag, FlagBarrier, FlagAttack, Shot, Hit, ToGundam, Dash, Danger,
    }

    public enum EnemyTagType
    {
        Enemy, BulletSoft, BulletHard,
    }
}
