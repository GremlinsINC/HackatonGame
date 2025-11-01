using UnityEngine;

[System.Serializable]
public class Music
{
    public string instrumentName;
    public AudioClip[] layers;

    [Range(0f, 1f)] public float volume = 1f;
}

