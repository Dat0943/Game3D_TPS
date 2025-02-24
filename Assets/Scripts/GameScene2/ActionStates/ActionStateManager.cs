using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ActionStateManager : MonoBehaviour
{
    [Header("Action State Animation")]
    [HideInInspector] public ActionBaseState currentState;
    public DefaultState Default = new DefaultState();
    public ReloadState Reload = new ReloadState();
    public SwapState Swap = new SwapState();

    [HideInInspector] public WeaponManager currentWeapon; // Tiền đề để khi có nhiều súng thì sẽ đổi
    [HideInInspector] public WeaponAmmo ammo;
    [HideInInspector] public Animator anim;

    public MultiAimConstraint rHandAim;
    public TwoBoneIKConstraint lHandIK;

    AudioSource audioSource;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        SwitchState(Default);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(ActionBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    // Sử kiện Animation Reloading
    public void WeaponReloaded()
    {
        ammo.Reload();
        rHandAim.weight = 1;
        lHandIK.weight = 1;
        SwitchState(Default);
    }

    #region Sự kiện Audio
    // Sound lấy băng đạn ra
    public void MagOut()
    {
        audioSource.PlayOneShot(ammo.magOutSound);
    }

    // Lắp băng đạn vào
    public void MagIn()
    {
        audioSource.PlayOneShot(ammo.magInSound);
    }

    // Kéo nạp đạn
    public void ReleaseSlide()
    {
        audioSource.PlayOneShot(ammo.releaseSlideSound);
    }
    #endregion

    public void SetWeapon(WeaponManager weapon)
    {
        currentWeapon = weapon;
        audioSource = weapon.audioSource;
        ammo = weapon.ammo;
    }
}
