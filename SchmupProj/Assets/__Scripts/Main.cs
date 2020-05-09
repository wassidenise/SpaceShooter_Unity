using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    static public Main S;
    static public Dictionary<WeaponType, WeaponDefinition> W_DEFS;

    public GameObject[] prefabEnemies;
    static public int prefabEnemiesLength;
    public float enemySpawnPerSecond = 0.5f;
    public float enemySpawnPadding = 1.5f;
    public WeaponDefinition[] weaponDefinitions;
    public Text lvltxt;
    static public int countScore=0;
    public Text scoreTxt;
    static public int heroLife=3;
    public Text heroLifeTxt;
    

    //Create levels
    static int level = 0;



    public GameObject prefabPowerUp;
    public WeaponType[] powerUpFrequency = new WeaponType[] {
WeaponType.blaster, WeaponType.blaster,
WeaponType.spread,
WeaponType.shield };

    public bool _______________;
    public WeaponType[] activeWeaponTypes;
    public float enemySpawnRate;

    //private AudioSource audioSource;

    public static int Level
    {
        get { return level; }
        set { level = value; }

    }



    void Awake()
    {

        S = this;
        //Set Utils.cambounds
        Utils.SetCameraBounds(this.GetComponent<Camera>());
        //0.5 enemies/second = enemySpawnRate of 2
        enemySpawnRate = 1f / enemySpawnPerSecond;
        //Invoke call spawnenemy() once after a 2 second delay
        Invoke("SpawnEnemy", enemySpawnRate);
        lvlTxtOn();
        Invoke("lvlTxtOff",2f);


        // A generic Dictionary with WeaponType as the key
        W_DEFS = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions)
        {
            W_DEFS[def.type] = def;
        }
    }

    public void ShipDestroyed(Enemy e)
    {
        // Potentially generate a PowerUp
        if (Random.value <= e.powerUpDropChance)
        {
            // Random.value generates a value between 0 & 1 (though never == 1)
            // If the e.powerUpDropChance is 0.50f, a PowerUp will be generated
            // 50% of the time. For testing, it's now set to 1f.
            // Choose which PowerUp to pick
            // Pick one from the possibilities in powerUpFrequency
            int ndx = Random.Range(0, powerUpFrequency.Length);
            WeaponType puType = powerUpFrequency[ndx];
            // Spawn a PowerUp
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            // Set it to the proper WeaponType
            pu.SetType(puType);
            // Set it to the position of the destroyed ship
            pu.transform.position = e.transform.position;
        }
    }

    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        // Check to make sure that the key exists in the Dictionary
        // Attempting to retrieve a key that didn't exist, would throw an error,
        // so the following if statement is important.
        if (W_DEFS.ContainsKey(wt))
        {
            return (W_DEFS[wt]);
        }
        // This will return a definition for WeaponType.none,
        // which means it has failed to find the WeaponDefinition
        return (new WeaponDefinition());
    }

    void Start()
    {


        Screen.SetResolution(630, 900, false);

        // Not yet
        //GameObject scoreGO = GameObject.Find("ScoreCounter");
        activeWeaponTypes = new WeaponType[weaponDefinitions.Length];
        for (int i = 0; i < weaponDefinitions.Length; i++)
        {
            activeWeaponTypes[i] = weaponDefinitions[i].type;
        }
        prefabEnemiesLength = prefabEnemies.Length;

       
    }

    public void SpawnEnemy()
    {
        int Enemylevel;
        Enemylevel = level;



        int ndx;
        if(Enemylevel < prefabEnemies.Length-1)
        {
            print("EnemyLvel "+ Enemylevel);
            
            ndx = Random.Range(0, Enemylevel+1);
            GameObject go = Instantiate(prefabEnemies[ndx]) as GameObject;
            //position the Enemy above the screen with a random x positions
            Vector3 pos = Vector3.zero;
            float xMin = Utils.camBounds.min.x + enemySpawnPadding;
            float xMax = Utils.camBounds.max.x - enemySpawnPadding;
            pos.x = Random.Range(xMin, xMax);
            pos.y = Utils.camBounds.max.y + enemySpawnPadding;
            go.transform.position = pos;
            //call spawnEnemy() again in a couple of seconds
            Invoke("SpawnEnemy", enemySpawnRate);
        }
        else if (Enemylevel == (prefabEnemies.Length-1))
        {
            //Set LevelUpObj to inactive
            GameObject LevelUpObj = GameObject.FindWithTag("LevelUpObj");
            LevelUpObj.SetActive(false);

            ndx =Enemylevel;
            print("Last EnemyLvel ");
            GameObject go = Instantiate(prefabEnemies[ndx]) as GameObject;
            //position the Enemy above the screen with a random x positions
            Vector3 pos = Vector3.zero;
            float xMin = Utils.camBounds.min.x + enemySpawnPadding;
            float xMax = Utils.camBounds.max.x - enemySpawnPadding;
            pos.x = Random.Range(xMin, xMax);
            pos.y = Utils.camBounds.max.y + enemySpawnPadding;
            go.transform.position = pos;



        }

        else {
            print("Should be ending");

        }
    }

    public void DelayedRestart(float delay)
    {
        // Invoke the Restart() method in delay seconds
        Invoke("Restart", delay);
        
    }
    public void Restart()
    {
        
        // Reload _Scene_0 to restart the game
        //Application.LoadLevel("_Scene_0");
        SceneManager.LoadScene("_Scene_0");
    }
  
    public void Reset()
    {
        level = 0;
        heroLife = 3;
        countScore = 0;

    }

    public void GameOver()
    {
        SceneManager.LoadScene("_Scene_GameOver");
    }
    public void Victory()
    {
        SceneManager.LoadScene("_Scene_Victory");
    }


    public void lvlTxtOn()
    {
        GameObject lvlText = GameObject.FindWithTag("lvlTxt");
        lvltxt.text = "Level" + (Level + 1);
        lvlText.SetActive(true);
    }
    public void lvlTxtOff()
    {
        GameObject lvlText = GameObject.FindWithTag("lvlTxt");
        lvltxt.text = "Level " + (Level + 1);
        lvlText.SetActive(false);
    }
     public void Update()
    {
        
        scoreTxt.text = "Score: " + (countScore);
        heroLifeTxt.text = "Life: " + (heroLife);
        //audioSource.Play();
    }


}
