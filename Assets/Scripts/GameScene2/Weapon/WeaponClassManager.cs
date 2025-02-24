using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponClassManager : MonoBehaviour
{
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    public Transform recoilFollowPosition;
    ActionStateManager action;

    public WeaponManager[] weapons;
    int currentWeaponIndex;

    void Start()
    {
        currentWeaponIndex = 0;
        for (int i = 0; i < weapons.Length; i++)
        {
            if (i == 0)
                weapons[i].gameObject.SetActive(true);
            else
                weapons[i].gameObject.SetActive(false);
        }
    }

    public void SetCurrentWeapon(WeaponManager weapon)
    {
        if(action == null) 
            action = GetComponent<ActionStateManager>();

        leftHandIK.data.target = weapon.leftHandTarget;
        leftHandIK.data.hint = weapon.leftHandHint;

        action.SetWeapon(weapon);
    }

    public void ChangeWeapon(float direction)
    {
        weapons[currentWeaponIndex].gameObject.SetActive(false);

        if(direction < 0)
        {
            if (currentWeaponIndex == 0)
                currentWeaponIndex = weapons.Length - 1;
            else
                currentWeaponIndex--;
        }
        else
        {
            if (currentWeaponIndex == weapons.Length - 1)
                currentWeaponIndex = 0;
            else
                currentWeaponIndex++;
        }

        weapons[currentWeaponIndex].gameObject.SetActive(true);
    }

    #region Hoạt ảnh cất súng vào và rút súng ra
    public void WeaponPutAway()
    {
        ChangeWeapon(action.Default.scrollDirection);
    }

    public void WeaponPulledOut()
    {
        action.SwitchState(action.Default);
    }
    #endregion
}
