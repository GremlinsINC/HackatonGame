using UnityEngine;

public class Enemy : MonoBehaviour, IMusicEntity
{
    [SerializeField] private Music music;
    public string Name => "Enemy";
    public Music Music => music;
    public Transform Transform => transform;
}
