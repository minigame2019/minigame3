using System;
using System.Collections.Generic;
using System.Threading;
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
    public Eyecatchs GameEyecatchs;

    //CG Setting
    public Image startCg;
    public int id_nowCg = 0;
    public int id_nowEyecatch = 0;
    private int num_startCg = 5;
    public bool isStartCG = false;

    //Help
    public GameObject help;
    public int id_nowHelp = 0;
    public int num_help = 1;
    private bool inHelp = false;

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
    private HelpMessageStats helpMessageStats;

    public bool isAtNight;
    public bool isCheating = true;
    public bool bossExist = false;
    public bool bossIsAlive = false;

    
    public bool eyecatchShowing = false;
    public bool isEyecatching = false;
    public FadeInOut m_Fade;

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
        IsSwitchShowed = false;
        this.MapGenerator = base.GetComponent<NewMapGenerator>();
        info = GameObject.Find("/Canvas/PlayerInfo").GetComponent<PlayerInfo>();

        //自适应
        Vector2 scrV = new Vector2(Screen.width, Screen.height);
        startCg.rectTransform.sizeDelta = scrV;
        GameMenus.MainMenu.transform.Find("Background").gameObject.GetComponent<Image>().rectTransform.sizeDelta = scrV;
        GameMenus.BackBar.GetComponent<Image>().rectTransform.sizeDelta = scrV;
        GameObject.Find("/Canvas/Text").GetComponent<Text>().rectTransform.sizeDelta = scrV;
        info.transform.Find("Health").gameObject.GetComponent<Text>().rectTransform.sizeDelta = scrV;
        GameEyecatchs.Background.GetComponent<Image>().rectTransform.sizeDelta = scrV;
        GameEyecatchs.Eyecatch.GetComponent<Image>().rectTransform.sizeDelta = scrV;
        help.GetComponent<Image>().rectTransform.sizeDelta = scrV;
    }

    public void ClearProjectiles()
    {
        for (int i = 0; i < this.registeredProjectiles.Count; i++)
        {
            PoolingSystem.DestroyAPS(this.registeredProjectiles[i]);
        }
    }

    /* UI */
    public bool IsSwitchShowed { get => helpMessageStats.isSwitch; set => helpMessageStats.isSwitch = value; }

    /* Switch between day and night */
    public void Switch()
    {
        if (isAtNight)
        {
            // Change backgrond light, floor, player and projectiles
            // floor black -> white
            // player white -> black
            FindObjectOfType<PlayerCharacter>().SwitchMaterialToNight(false);
            GameObject.Find("Floor").GetComponent<Renderer>().material = Resources.Load<Material>("Materials/White");
            GameObject[] walls = GameObject.FindGameObjectsWithTag("Obstacle Wall");
            foreach (var wall in walls)
            {
                if (wall.GetComponent<DestructableWall>() != null)
                {
                    wall.GetComponent<DestructableWall>().SwitchMaterialToNight(false);
                }
                else if (wall.GetComponent<NormalWall>() != null)
                {
                    wall.GetComponent<NormalWall>().SwitchMaterialToNight(false);
                }
            }

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
            // player black -> white
            FindObjectOfType<PlayerCharacter>().SwitchMaterialToNight(true);
            GameObject.Find("Floor").GetComponent<Renderer>().material = Resources.Load<Material>("Materials/DarkFloor");
            GameObject[] walls = GameObject.FindGameObjectsWithTag("Obstacle Wall");
            foreach (var wall in walls)
            {
                if (wall.GetComponent<DestructableWall>() != null) {
                    wall.GetComponent<DestructableWall>().SwitchMaterialToNight(true);
                }
                else if (wall.GetComponent<NormalWall>() != null) {
                    wall.GetComponent<NormalWall>().SwitchMaterialToNight(true);
                }
            }

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
       PoolingSystem.Instance.InstantiateAPS("TempAudio", position, Quaternion.identity).GetComponent<TempAudio>().SetAudio(clip, (time <= 0f) ? clip.length : time);
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
        if(this.bossExist)
        {
            if (!this.bossIsAlive)
            {
                WaveOver();
                return;
            }
            this.levelBoss.CheckEnemyCount();           
        }
        else
        {
            if (this.CurrentEnemies <= 0)
            {
                WaveOver();
            }
        }
    }

    private void WaveOver()
    {
        //PoolingSystem.Instance.InstantiateAPS("PlayerExplode", base.transform.position, Quaternion.identity);
        this.StageCleared();
    }

    public void SetEyecatchActive(bool b = true)
    {
        GameEyecatchs.Background.SetActive(b);
        GameEyecatchs.Eyecatch.SetActive(b);
    }

    public void NextEyecatch()
    {
        int num = 0;
        switch (CurrentLevel)
        {
            case 2:
                num = GameEyecatchs.num2;
                break;
            case 3:
                num = GameEyecatchs.num3;
                break;
            case 4:
                num = GameEyecatchs.num4;
                break;
            case 5:
                num = GameEyecatchs.num5;
                break;
            case 6:
                num = GameEyecatchs.num6;
                break;
            case 7:
                num = GameEyecatchs.num7;
                break;
            case 8:
                num = GameEyecatchs.num8;
                break;
            default:
                break;
        }
        if (id_nowEyecatch < num)
        {
            id_nowEyecatch++;
            m_Fade.BackGroundControl(false);
            string path = "Sprites/"+ CurrentLevel.ToString() +"/" + id_nowEyecatch.ToString();
            GameEyecatchs.Eyecatch.GetComponent<Image>().sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
        }
        else
        {
            id_nowEyecatch = 0;
            SetEyecatchActive(false);
            isEyecatching = false;
            if(CurrentLevel<8)
            {
                Restart();
            }
            else
            {
                CurrentLevel = 1;
                stageFailed = true;
                ReturnToMenu();
            }
        }
    }

    public void Restart()
    {
        m_Fade.BackGroundControl(false);
        info.gameObject.SetActive(true);
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
        info.Renew();
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
        info.gameObject.SetActive(false);
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
    
    /*
    public void CGPlay()
    {
        startCg.gameObject.SetActive(true);
        for (int i=1;i<=num_startCg;i++)
        {
            id_nowCg = i;
            string path = "Sprites/startCG/" + i.ToString();
            Sprite spriteNow = Resources.Load(path, typeof(Sprite)) as Sprite;
            Debug.Log(spriteNow);
            startCg.GetComponent<Image>().sprite = spriteNow;
            Thread.Sleep(2000);
            m_Fade.BackGroundControl(false);
        }
        //isStartCG = false;
        //startCg.gameObject.SetActive(false);
    }
    */

    public void NextCG()
    {
        if (id_nowCg < 0)
            return;
        if(id_nowCg<num_startCg)
        {
            id_nowCg++;
            m_Fade.BackGroundControl(false);
            string path = "Sprites/startCG/" + id_nowCg.ToString();
            startCg.GetComponent<Image>().sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
        }
        else
        {
            isStartCG = false;
            startCg.gameObject.SetActive(false);
        }
    }

    public void NextHelp()
    {
        if(id_nowHelp<num_help)
        {
            id_nowHelp++;
            m_Fade.BackGroundControl(false);
            string path = "Sprites/help/" + id_nowHelp.ToString();
            help.GetComponent<Image>().sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
        }
        else
        {
            id_nowHelp = 0;
            inHelp = false;
            help.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        PoolingSystem.Instance.InitializePool();
        this.GameMenus.TopBar.canvasRenderer.SetAlpha(0f);
        this.GameMenus.BottomBar.canvasRenderer.SetAlpha(0f);
        this.GameMenus.BackBar.canvasRenderer.SetAlpha(0f);
        this.stageFailed = true;
        isStartCG = true;
        
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
        if (isCheating)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StageCleared();
            }
        }
        if (isStartCG)
        {
            if (id_nowCg == 0) 
            {
                startCg.gameObject.SetActive(true);
                NextCG();
            }
            if (Input.GetButtonDown("Start"))
            {
                NextCG();
            }
        }
        else if (isEyecatching)
        {
            if (id_nowEyecatch == 0)
            {
                SetEyecatchActive(true);
                NextEyecatch();
            }
            if (Input.GetButtonDown("Start"))
            {
                NextEyecatch();
                //SetEyecatchActive(false);
                //isEyecatching = false;
                //this.Restart();
            }

        }
        else if (inHelp)
        {
            if(id_nowHelp==0)
            {
                help.gameObject.SetActive(true);
                NextHelp();
            }
            if (Input.GetButtonDown("Start"))
            {
                NextHelp();
            }
        }
        else if (this.inMenu)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                inHelp = true;
            }
            
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
                isEyecatching = true;
                //this.Restart();
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

    private struct HelpMessageStats
    {
        public bool isSwitch;
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
        public GameObject MessagePanel;
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

        public GameObject AbsorberEnemy;

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

    [Serializable]
    public class Eyecatchs
    {
        public GameObject Background;
        public GameObject Eyecatch;

        //第x关前的图片数量
        public int num2 = 2;
        public int num3 = 2;
        public int num4 = 2;
        public int num5 = 2;
        public int num6 = 1;
        public int num7 = 2;
        public int num8 = 1;
    }



}
