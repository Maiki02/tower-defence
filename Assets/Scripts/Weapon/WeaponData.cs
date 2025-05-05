using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public float damage;
    public float range;
    public float cooldown;
    public GameObject projectilePrefab; // null para melee
    public AnimationClip attackAnimation; //TODO Analizar si lo uso, si lo uso en el prefab o en el script
}