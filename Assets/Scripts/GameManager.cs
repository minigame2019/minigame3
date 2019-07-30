﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using static PoolingSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string Version = "1.0";
    public NewMapGenerator MapGenerator;
    public Menus GameMenus;
    public Prefabs GamePrefabs;
    public Sounds GameSounds;
    public LayerMask CharacterLayer;
    public LayerMask EnemyLayer;
    public LayerMask ProjectileLayer;
    public Transform LevelContainer;
    public int TotalEnemies;
    public int CurrentEnemies;
    private bool inMenu = true;
    private bool isPaused;
    private bool stageFailed;
    private bool stageClear;
    private BossCharacter levelBoss;
    public int CurrentLevel = 1;
    private string currentSeed = string.Empty;
    private int ssn;
    private List<GameObject> registeredProjectiles = new List<GameObject>();
    private PlayerInfo info;
    public bool isAtNight;

    public void AddEnemy()
    {
        this.TotalEnemies++;
        this.CurrentEnemies++;
    }

    private void Awake()
    {
        Instance = this;
        Debug.Log(this.Version);
        this.CurrentLevel = 1;
        isAtNight = false;
        this.MapGenerator = base.GetComponent<NewMapGenerator>();
        info = GameObject.Find("/Canvas/PlayerInfo").GetComponent<PlayerInfo>();
    }

    public void ClearProjectiles()
    {
        for (int i = 0; i < this.registeredProjectiles.Count; i++)
        {
            PoolingSystem.DestroyAPS(this.registeredProjectiles[i]);
        }
    }

    public void Switch()
    {
        if (isAtNight)
        {
            // Change backgrond light, floor, player and projectiles
            // floor black -> white
            // player white -> black
            FindObjectOfType<PlayerCharacter>().SwitchMaterialToNight(false);
            GameObject.Find("Floor").GetComponent<Renderer>().material = Resources.Load<Material>("Materials/White");

            // projectiles white -> black
            for (int i = 0; i < PoolingSystem.Instance.pooledItems.Length; i++)
            {
                GameObject firstItem = PoolingSystem.Instance.pooledItems[i][0];
                if (firstItem.GetComponent<IMChangeable>() == null) continue;
                if (firstItem.GetComponent<Projectile>() != null && firstItem.layer != 11) continue;
                for (int j = 0; j < PoolingSystem.Instance.pooledItems[i].Count; j++)
                {
                    IMChangeable item = PoolingSystem.Instance.pooledItems[i][j].GetComponent<IMChangeable>();
                    if (item != null)
                    {
                        item.SwitchMaterialToNight(false);
                    }
                }
            }
            for (int i = 0; i < PoolingSystem.Instance.poolingItems.Length; i++)
            {
                PoolingItems item = PoolingSystem.Instance.poolingItems[i];
                if (item.prefab.GetComponent<IMChangeable>() == null) continue;
                if (item.prefab.GetComponent<Projectile>() != null && item.prefab.name != "Projectile") continue;
                item.SwitchMaterialToNight(false);
            }
            // set black part destructable
            Debug.Log("Change to day");
        }
        else
        {
            // Change backgrond light, floor, player and projectiles
            // floor white -> black
            //GameObject.Find("Floor").
            // player black -> white
            FindObjectOfType<PlayerCharacter>().SwitchMaterialToNight(true);
            GameObject.Find("Floor").GetComponent<Renderer>().material = Resources.Load<Material>("Materials/DarkFloor");

            for (int i = 0; i < PoolingSystem.Instance.pooledItems.Length; i++)
            {
                GameObject firstItem = PoolingSystem.Instance.pooledItems[i][0];
                if (firstItem.GetComponent<IMChangeable>() == null) continue;
                if (firstItem.GetComponent<Projectile>() != null && firstItem.layer != 11) continue;
                for (int j = 0; j < PoolingSystem.Instance.pooledItems[i].Count; j++)
                {
                    IMChangeable item = PoolingSystem.Instance.pooledItems[i][j].GetComponent<IMChangeable>();
                    if (item != null)
                    {
                        item.SwitchMaterialToNight(true);
                    }
                }
            }
            for (int i = 0; i < PoolingSystem.Instance.poolingItems.Length; i++)
            {
                PoolingItems item = PoolingSystem.Instance.poolingItems[i];
                if (item.prefab.GetComponent<IMChangeable>() == null) continue;
                if (item.prefab.GetComponent<Projectile>() != null && item.prefab.name != "Projectile") continue;
                item.SwitchMaterialToNight(true);
            }

            // projectiles black -> white
            // set white part destructable
            Debug.Log("Change to night");
        }
        isAtNight = !isAtNight;
        //throw new NotImplementedException();
    }

    private void FadeBlackBars(bool fadeIn)
    {
        this.GameMenus.TopBar.CrossFadeAlpha(!fadeIn ? 0f : 1f, 0.15f, true);
        this.GameMenus.BottomBar.CrossFadeAlpha(!fadeIn ? 0f : 1f, 0.15f, true);
        this.GameMenus.BackBar.CrossFadeAlpha(!fadeIn ? 0f : 1f, 0.15f, true);
    }

    public bool GamePaused() =>
        this.isPaused;

    public bool GameRunning() =>
        (!this.stageClear && !this.stageFailed);

    public void PlayAudio(Vector3 position, AudioClip clip, float time = -1f)
    {
        //PoolingSystem.Instance.InstantiateAPS("TempAudio", position, Quaternion.identity).GetComponent<TempAudio>().SetAudio(clip, (time <= 0f) ? clip.length : time);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    public void RegisterProjectile(GameObject obj)
    {
        this.registeredProjectiles.Add(obj);
    }

    public void RemoveEnemy()
    {
        this.CurrentEnemies--;
        this.levelBoss.CheckEnemyCount();
        if (this.CurrentEnemies <= 0)
        {
            this.StageCleared();
        }
    }

    public void Restart()
    {
        this.ClearProjectiles();
        this.TotalEnemies = 0;
        this.CurrentEnemies = 0;
        this.MapGenerator.GenerateMap();
        this.inMenu = false;
        this.isPaused = false;
        this.stageClear = false;
        this.stageFailed = false;
        Time.timeScale = 1f;
        this.GameMenus.MainMenu.SetActive(false);
        this.GameMenus.PauseMenu.SetActive(false);
        this.GameMenus.StageClearMenu.SetActive(false);
        this.GameMenus.StageFailedMenu.SetActive(false);
        this.GameMenus.TopBar.canvasRenderer.SetAlpha(0f);
        this.GameMenus.BottomBar.canvasRenderer.SetAlpha(0f);
        this.GameMenus.BackBar.canvasRenderer.SetAlpha(0f);
        this.Switch();
        //info.Renew();
    }

    private void ReturnToMenu()
    {
        this.ClearProjectiles();
        this.isPaused = false;
        Time.timeScale = 1f;
        this.stageFailed = false;
        this.stageClear = false;
        this.inMenu = true;
        this.GameMenus.GameStats.text = "";
        this.GameMenus.MainMenu.SetActive(true);
        this.GameMenus.PauseMenu.SetActive(false);
        this.GameMenus.StageClearMenu.SetActive(false);
        this.GameMenus.StageFailedMenu.SetActive(false);
        this.FadeBlackBars(false);
        if (this.LevelContainer.gameObject)
        {
            Destroy(this.LevelContainer.gameObject);
        }
    }

    public void SetGameStats()
    {
        object[] objArray1 = new object[] { "Level: ", this.CurrentLevel, "\nSeed: ", this.MapGenerator.Seed };
        this.GameMenus.GameStats.text = string.Concat(objArray1);
    }

    public void SetLevelBoss(GameObject boss)
    {
        this.levelBoss = boss.GetComponent<BossCharacter>();
    }

    public void StageCleared()
    {
        this.CurrentLevel++;
        this.stageClear = true;
        this.GameMenus.StageClearMenu.SetActive(true);
        this.FadeBlackBars(true);
    }

    public void StageFailed()
    {
        this.CurrentLevel = 1;
        this.stageFailed = true;
        this.GameMenus.StageFailedMenu.SetActive(true);
        this.FadeBlackBars(true);
    }

    private void Start()
    {
        PoolingSystem.Instance.InitializePool();
        this.GameMenus.TopBar.canvasRenderer.SetAlpha(0f);
        this.GameMenus.BottomBar.canvasRenderer.SetAlpha(0f);
        this.GameMenus.BackBar.canvasRenderer.SetAlpha(0f);
        this.stageFailed = true;
    }

    public void TogglePause()
    {
        this.isPaused = !this.isPaused;
        Time.timeScale = !this.isPaused ? ((float)1) : ((float)0);
        this.GameMenus.PauseMenu.SetActive(this.isPaused);
        this.FadeBlackBars(this.isPaused);
    }

    private void Update()
    {
        if (this.inMenu)
        {
            if (Input.GetButtonDown("Quit"))
            {
                this.QuitGame();
            }
            if (Input.GetButtonDown("Start"))
            {
                this.Restart();
            }
        }
        else if (this.stageClear)
        {
            if (Input.GetButtonDown("Start"))
            {
                this.Restart();
            }
        }
        else if (this.stageFailed)
        {
            if (Input.GetButtonDown("Start"))
            {
                this.ReturnToMenu();
            }
        }
        else
        {
            if (Input.GetButtonDown("Pause"))
            {
                this.TogglePause();
            }
            if (this.isPaused)
            {
                if (Input.GetButtonDown("Quit"))
                {
                    this.ReturnToMenu();
                }
                if (Input.GetButtonDown("Restart"))
                {
                    this.Restart();
                }
            }
        }
    }

    [Serializable]
    public class Menus
    {
        public Image TopBar;
        public Image BottomBar;
        public Image BackBar;
        public GameObject MainMenu;
        public GameObject PauseMenu;
        public GameObject StageClearMenu;
        public GameObject StageFailedMenu;
        public Text GameStats;
    }

    [Serializable]
    public class Prefabs
    {
        public GameObject Floor;
        public GameObject Player;
        public GameObject BasicEnemy;
        public GameObject ArmorEnemy;
        public GameObject PillarEnemy;
        public GameObject PillarArmorEnemy;
        public GameObject BossEnemy;

        public GameObject Switch;
        public GameObject Hunter;

        public GameObject RegularWall;
        public GameObject DestructableWall;
        public GameObject TempAudio;
    }

    [Serializable]
    public class Sounds
    {
        public AudioClip Shoot;
        public AudioClip Hit;
        public AudioClip Explode;
        public AudioClip Armor;
    }
}
