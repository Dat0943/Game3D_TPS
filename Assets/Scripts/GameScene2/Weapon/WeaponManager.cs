using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header ("Fire Rate")]
    [SerializeField] private float fireRate;
    [SerializeField] private bool semiAuto;
    float fireRateTimer;

    [Header ("Bullet Properties")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform barrelPosition;
    [SerializeField] private float bulletVelocity;
    [SerializeField] private int bulletsPerShot;
    public float damage = 10f;
    AimStateManager aim;

    [Header("Muzzle Flash")]
    [SerializeField] private float lightReturnSpeed = 20f;
    ParticleSystem muzzleFlashParticle;
    Light muzzleFlashLight;
    float lightIntensity; // Để phát sáng to hơn khi người chơi bắn

    [Header("Audio Source")]
    [SerializeField] private AudioClip gunShotAudio;
    [HideInInspector] public AudioSource audioSource;

    [Header("Left Hand")]
    public Transform leftHandTarget, leftHandHint;
    WeaponClassManager weaponClass;

    [HideInInspector] public WeaponAmmo ammo;
    ActionStateManager action;
    WeaponRecoil recoil;
    WeaponBloom bloom;

    void Awake()
    {
        aim = GetComponentInParent<AimStateManager>();
        muzzleFlashParticle = GetComponentInChildren<ParticleSystem>();
        muzzleFlashLight = GetComponentInChildren<Light>();
        action = GetComponentInParent<ActionStateManager>();
        bloom = GetComponent<WeaponBloom>();
    }

    void OnEnable()
    {
        if(weaponClass == null)
        {
            weaponClass = GetComponentInParent<WeaponClassManager>();
            ammo = GetComponent<WeaponAmmo>();
            recoil = GetComponent<WeaponRecoil>();
            audioSource = GetComponent<AudioSource>();
            recoil.recoilFollowPosition = weaponClass.recoilFollowPosition;
        }

        weaponClass.SetCurrentWeapon(this);
    }

    void Start()
    {
        fireRateTimer = fireRate;

        lightIntensity = muzzleFlashLight.intensity; // Ban đầu bằng 1
        muzzleFlashLight.intensity = 0;
    }

    void Update()
    {
        if(ShouldFire()) Fire();
        muzzleFlashLight.intensity = Mathf.Lerp(muzzleFlashLight.intensity, 0, lightReturnSpeed * Time.deltaTime);
    }

    bool ShouldFire()
    {
        fireRateTimer += Time.deltaTime;
        if (fireRateTimer < fireRate)
            return false;

        if (ammo.currentAmmo == 0)
            return false;

        if (action.currentState == action.Reload) return false;

        if (action.currentState == action.Swap) return false;

        // Chế độ bắn từng viên 1
        if (semiAuto && Input.GetKeyDown(KeyCode.Mouse0))
            return true;

        // Chế độ bắn liên tục
        if (!semiAuto && Input.GetKey(KeyCode.Mouse0))
            return true;

        return false;
    }

    void Fire()
    {
        fireRateTimer = 0;

        barrelPosition.LookAt(aim.aimPosition);
        barrelPosition.localEulerAngles = bloom.BloomAngle(barrelPosition);

        audioSource.PlayOneShot(gunShotAudio);
        recoil.TriggerRecoil();
        TriggerMuzzleFlash();

        ammo.currentAmmo--;

        for (int i = 0; i < bulletsPerShot; i++)
        {
            GameObject currentBullet = Instantiate(bulletPrefab, barrelPosition.position, barrelPosition.rotation);

            Bullet bullet = currentBullet.GetComponent<Bullet>();
            bullet.weapon = this;

            bullet.direction = barrelPosition.transform.forward;

            Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
            rb.AddForce(barrelPosition.forward * bulletVelocity, ForceMode.Impulse);
        }
    }

    void TriggerMuzzleFlash()
    {
        muzzleFlashParticle.Play();
        muzzleFlashLight.intensity = lightIntensity;
    }
}
