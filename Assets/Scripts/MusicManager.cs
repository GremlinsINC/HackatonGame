using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Sound settings")]
    public float maxDistance = 10f;

    [Header("Object refs")]
    public PlayerInventory player;
    public List<Enemy> enemies = new();

    private AudioSource playerSource;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        if (player != null)
            player.OnItemAdded += OnPlayerCollectedItem;
    }

    private void OnDisable()
    {
        if (player != null)
            player.OnItemAdded -= OnPlayerCollectedItem;
    }

    private void Update()
    {
        if (player == null) return;


        foreach (var enemy in enemies)
        {
            float dist = Vector3.Distance(player.Transform.position, enemy.Transform.position);
            float volume = Mathf.Clamp01(1f - dist / maxDistance);
            if(!UiController.isPaused) UpdateEnemyVolume(enemy, volume);
        }
    }

    private void OnPlayerCollectedItem()
    {
        if(playerSource == null)
            playerSource = player.gameObject.GetComponent<AudioSource>();

        PlayMusicLayer(player.Music);
    }

    private void PlayMusicLayer(Music music)
    {
        if (music == null) return;


        int layer = Mathf.Min(player.Items.Count - 1, music.layers.Length - 1);
        var clip = music.layers[layer];

        playerSource.clip = clip;
        playerSource.volume = music.volume;
        playerSource.loop = true;
        playerSource.Play();
    }

    private void UpdateEnemyVolume(Enemy enemy, float volume)
    {
        var enemySource = enemy.gameObject.GetComponent<AudioSource>();
        if (enemySource.clip != enemy.Music.layers[0])
        {
            enemySource.clip = enemy.Music.layers[0];
            enemySource.loop = true;
            enemySource.Play();
        }

        enemySource.volume = volume * enemy.Music.volume;
    }
}

