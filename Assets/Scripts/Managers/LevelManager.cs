using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public GameObject playerPrefab;
    static LevelSettings _cl;
    public GameObject spawnedLevel;
    public Transform levelParent;
    float playerGone;
    public static LevelSettings CurrentLevel
    {
        get => _cl;
        set
        {
            if (_cl == value)
                return;
            _cl = value;
            if (Instance)
                Instance.StartLevel();
        }
    }

    protected void Awake()
    {
        if (!levelParent)
            levelParent = transform;
        if (CurrentLevel)
            StartLevel();
    }

    void Update()
    {
        if (!PlayerController.Instance)
        {
            playerGone += Time.deltaTime;
            if (playerGone > 2)
            {
                if (CurrentLevel.RestartOnDeath)
                    StartLevel();
                else
                    SpawnPlayer();
            }
        }
        else playerGone = 0;
    }

    public void SpawnPlayer()
    {
        if (PlayerSpawn.ActiveSpawn)
            Instantiate(playerPrefab, PlayerSpawn.ActiveSpawn.transform.position, PlayerSpawn.ActiveSpawn.transform.rotation);
    }

    public void StartLevel(bool respawnPlayer = true)
    {
        if (spawnedLevel)
        {
            if (respawnPlayer && PlayerController.Instance)
                ((IDamageable)PlayerController.Instance).Kill();
            spawnedLevel.SetActive(false);
            Destroy(spawnedLevel);
            HUDManager.Instance.ClearInformation();
        }
        if (CurrentLevel)
        {
            ProgressManager.Instance.RelockTemporaryAbilities();
            spawnedLevel = Instantiate(CurrentLevel.Prefab, levelParent);
            if (respawnPlayer && PlayerSpawn.ActiveSpawn)
                Instantiate(playerPrefab, PlayerSpawn.ActiveSpawn.transform.position, PlayerSpawn.ActiveSpawn.transform.rotation);
        }
    }
}
