using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Hero : MonoBehaviour
{
    static public Hero S;

    private float gameRestartDelay = 5f;

    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;

    // Ship status information
    [SerializeField]
    private float _shieldLevel = 1; // Add the underscore!
    // Weapon fields
    public Weapon[] weapons;

    public bool _____________________;
    public Bounds bounds;

    // Declare a new delegate type WeaponFireDelegate
    public delegate void WeaponFireDelegate();
    // Create a WeaponFireDelegate field named fireDelegate.
    public WeaponFireDelegate fireDelegate;

    public AudioSource efxSource;
    public AudioClip powerUpSound;
    public AudioClip fireSound1;
    public AudioClip fireSound2;
    public AudioClip fireSound3;
    
    public AudioClip shieldDownSound;
    public static float lowPitchRange = .95f;
    public static float highPitchRange = 1.05f;

    void Awake()
    {
        S = this;
        bounds = Utils.CombineBoundsOfChildren(this.gameObject);
   }


    // Use this for initialization
    void Start()
    {
        // Reset the weapons to start _Hero with 1 blaster
        ClearWeapons();
        weapons[0].SetType(WeaponType.blaster);

        
    }

    // Update is called once per frame
    void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        bounds.center = transform.position;

        // constrain to screen
        Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.onScreen);
        if (off != Vector3.zero)
        {  // we need to move ship back on screen
            pos -= off;
            transform.position = pos;
        }

        // rotate the ship to make it feel more dynamic
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
        // Use the fireDelegate to fire Weapons
        // First, make sure the Axis("Jump") button is pressed
        // Then ensure that fireDelegate isn't null to avoid an error
        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        { // 1
            fireDelegate();
            RandomizeSfx(fireSound1, fireSound2);
        }

    }
    // This variable holds a reference to the last triggering GameObject
    public GameObject lastTriggerGo = null;

    void OnTriggerEnter(Collider other)
    {
        // Find the tag of other.gameObject or its parent GameObjects
        GameObject go = Utils.FindTaggedParent(other.gameObject);
        // If there is a parent with a tag
        if (go != null)
        {
            print("Triggered: " + go.name);
            // Make sure it's not the same triggering go as last time
            if (go == lastTriggerGo)
            { // 2
                return;
            }
            lastTriggerGo = go; // 3
            if (go.tag == "Enemy")
            {
                PlaySingle(shieldDownSound);

                // If the shield was triggered by an enemy
                // Decrease the level of the shield by 1
                shieldLevel--;
                // Destroy the enemy
                Destroy(go); // 4
               
            }
            else if (go.tag == "PowerUp")
            {
                // If the shield was triggerd by a PowerUp
                AbsorbPowerUp(go);
                print("PowerUp!");
                PlaySingle(powerUpSound);
            }
            else if (go.tag == "LevelUpObj")
            {
                               
                // If the shield was triggerd by LevelUpObj
                Destroy(go);
                Main.Level++;
                Main.countScore += 50;
                
                print("LevelUp triggered!"+Main.Level);

                if (Main.Level>=Main.prefabEnemiesLength)
                    {
                    
                }
                else { Main.S.Restart();
                    print("Restart! Level" + Main.Level) ;

                }
                
            }
            else
            {
                print("Triggered: " + go.name); 
            }
        }
    }



    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type)
        {
            case WeaponType.shield: // If it's the shield
                shieldLevel++;
                break;
            default: // If it's any Weapon PowerUp
                     // Check the current weapon type
                if (pu.type == weapons[0].type)
                {
                    // then increase the number of weapons of this type
                    Weapon w = GetEmptyWeaponSlot(); // Find an available weapon
                    if (w != null)
                    {
                        // Set it to pu.type
                        w.SetType(pu.type);
                    }
                }
                else
                {
                    // If this is a different weapon
                    ClearWeapons();
                    weapons[0].SetType(pu.type);
                }
                break;
        }
        pu.AbsorbedBy(this.gameObject);
    }
    Weapon GetEmptyWeaponSlot()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].type == WeaponType.none)
            {
                return (weapons[i]);
            }
        }
        return (null);
    }
    void ClearWeapons()
    {
        foreach (Weapon w in weapons)
        {
            w.SetType(WeaponType.none);
        }
    }
    public float shieldLevel
    {
        get
        {
            return (_shieldLevel); // 1
        }
        set
        {
            _shieldLevel = Mathf.Min(value, 4); // 2
            // If the shield is going to be set to less than zero
            if (value < 0)
            {
                // Tell Main.S to restart the game after a delay

                Main.S.DelayedRestart(gameRestartDelay);   
                Destroy(this.gameObject);
                Main.heroLife--;
                if (Main.heroLife == 0) 
                    {Main.S.GameOver(); }
                
            }
        }
    }

    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();

    }

    public void RandomizeSfx(params AudioClip[] clip)
    {
        int randomIndex = Random.Range(0, clip.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        efxSource.pitch = randomPitch;
        efxSource.clip = clip[randomIndex];
        efxSource.Play();
    }

}