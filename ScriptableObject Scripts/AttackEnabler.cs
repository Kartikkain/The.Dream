
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Attack/New Attack", order = 0)]
public class AttackEnabler : ScriptableObject
{
    [SerializeField] GameObject AttackPrefab;
}