using UnityEngine;

public class WeaponData : ScriptableObject
{
    public string weaponName;
    public float damage;
    public float range;
    public float cooldown;
    public GameObject projectilePrefab; // null para melee
    public AnimationClip attackAnimation;
}