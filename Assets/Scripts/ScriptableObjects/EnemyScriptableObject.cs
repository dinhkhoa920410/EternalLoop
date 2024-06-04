using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    public GameObject prefab;
    public float health;
    public float attack;
    public float defense;
    public float moveSpeed;
    public float experience;
}