using UnityEngine;

public interface IMusicEntity
{
    string Name { get; }
    Music Music { get; }
    Transform Transform { get; }
}
