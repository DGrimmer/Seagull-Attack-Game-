using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    
    [SerializeField] private Camera cam;
    private float fireTimer = 0;
    private float fireRate = 0.2f;
    [SerializeField] private int bulletMaxCap = 10;
    private int bullets{
        get{return RAW_bullets;} 
        set
        { 
            RAW_bullets = value;
            if(bulletText != null)
                bulletText.UpdateUITextWithValue(RAW_bullets);            
        }
    }
    private int RAW_bullets = 0;
    private int baitGrenade{
        get{return RAW_baitGrenade;} 
        set
        { 
            RAW_baitGrenade = value;
            if(baitGrenadeText != null)
                baitGrenadeText.UpdateUITextWithValue(RAW_baitGrenade);            
        }
    }
    private int RAW_baitGrenade = 0;
    [SerializeField] int damage = 1;

    
    [SerializeField] private GameObject baitGrenadePrefab;
    [SerializeField] private float throwStrength = 20;
    private Animator animator = null;
    private AudioSource audioSource = null;

    [SerializeField] private GameObject gun;
    [SerializeField] private AudioClip gunShot = null;
    [SerializeField] private AudioClip gunShotDry = null;
    [SerializeField] private AudioClip gunReload = null;
    [SerializeField] private UI_UpdateText bulletText = null;
    [SerializeField] private UI_UpdateText baitGrenadeText = null;
    [SerializeField] private ParticleSystem muzzleFlash = null;
    
    [SerializeField] private GameObject minigun;
    [SerializeField] private ParticleSystem muzzleFlashMinigun = null;
    [SerializeField] private AudioClip minigunShot = null;
    private Animator minigunAnimator;
    private bool isMinigunActive = false; 
    private bool hasStartedChargingMinigun = false;
    [SerializeField] private float minigunChargeUpDelay = 1f;
    private float minigunChargeUpDelayTimer = 0;
    [SerializeField] private float minigunShotRate = 0.2f;
    private float minigunShotRateTimer = 0;

    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        bullets = bulletMaxCap;
    }

    private void Start() 
    {
        if(bulletText != null)
            bulletText.UpdateUITextWithValue(bullets);
        if(bulletText != null)
            baitGrenadeText.UpdateUITextWithValue(RAW_baitGrenade);
        minigunAnimator = minigun.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        // Pistol timer
        fireTimer -= Time.deltaTime;
        // Minigun timers
        minigunChargeUpDelayTimer -= Time.deltaTime;
        minigunShotRateTimer -= Time.deltaTime;

        if(Input.GetButtonDown("Fire1"))
        {
            // If minigun is activ skip normal shooting
            if(isMinigunActive){
                // Setup minigun timers
                minigunChargeUpDelayTimer = minigunChargeUpDelay;
                minigunAnimator.SetBool("isFiring", true);
                hasStartedChargingMinigun = true;
                return;
            }
            
            if(FireGun())
                Shoot();
            
        }
        if(Input.GetButton("Fire1"))
        {
            if(!isMinigunActive)
                return;
            if(minigunChargeUpDelayTimer < 0 && minigunShotRateTimer < 0 && hasStartedChargingMinigun){
                minigunShotRateTimer = minigunShotRate;
                FireMinigun();
                Shoot();
            }
        }
        if(Input.GetButtonUp("Fire1"))
        {
            if(isMinigunActive){
                minigunAnimator.SetBool("isFiring", false);
                hasStartedChargingMinigun = false;
            }
        }

        if(Input.GetButtonDown("Throw"))
        {
            if(baitGrenade > 0){
                baitGrenade--;
                GameObject go = Instantiate(baitGrenadePrefab, cam.transform.position + cam.transform.forward * 2, Quaternion.identity);
                go.GetComponent<Rigidbody>().AddForce(cam.transform.forward * throwStrength);
            }
        }
    }

    private bool FireGun(){
        if(bullets <= 0){
            audioSource.PlayOneShot(gunShotDry, 0.4f);
            return false;
        }
        // Reset timer
        fireTimer = fireRate;

        // One bullet less..
        bullets -= 1;

        // Play animation and sound
        animator.SetTrigger("Fire");
        muzzleFlash.Play();
        audioSource.PlayOneShot(gunShot, 0.5f);
        return true;
    }

    private bool FireMinigun(){
        if(minigunChargeUpDelayTimer > 0 && minigunShotRateTimer > 0)
            return false;

        muzzleFlashMinigun.Play();
        audioSource.PlayOneShot(minigunShot, 0.5f);

        minigunShotRateTimer = minigunShotRate;
        return true;
    }

    private void Shoot(){
        // raycast, stop here if missed.
        if(!Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit))
            return;
        
        // For every component with iDamageable interface, call TakeDamage()
        MonoBehaviour[] list = hit.transform.gameObject.GetComponentsInChildren<MonoBehaviour>();
        foreach (var mb in list){
            if(mb is iDamageable)
            {
                iDamageable temp = mb as iDamageable;
                temp.TakeDamage(damage, cam.transform.forward);
            }
        }
    }

    public void AddBullets(int amount){
        bullets += amount;
    }
    public void AddBaitGrenades(int amount){
        baitGrenade += amount;
    }

    public void StartMinigunPowerup(float duration){
        StartCoroutine(StartMinigun(duration));
    }
    private IEnumerator StartMinigun(float duration)
    {
        // Swap for minigun
        gun.SetActive(false);
        minigun.SetActive(true);
        isMinigunActive = true;


        yield return new WaitForSeconds(duration);

        // Swap back
        gun.SetActive(true);
        minigun.SetActive(false);
        isMinigunActive = false;
        // Reset animation, in case of player shooting while duration goes off.
        minigunAnimator.SetBool("isFiring", false);
        hasStartedChargingMinigun = false;

    }
}
