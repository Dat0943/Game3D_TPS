using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultState : ActionBaseState
{
    public float scrollDirection;

    public override void EnterState(ActionStateManager action)
    {

    }

    public override void UpdateState(ActionStateManager action)
    {
        action.rHandAim.weight = Mathf.Lerp(action.rHandAim.weight, 1, 10 * Time.deltaTime);

        if (action.lHandIK.weight == 0)
            action.lHandIK.weight = 1;

        if ((Input.GetKeyDown(KeyCode.R) && CanReload(action)) || (action.ammo.currentAmmo <= 0 && CanReload(action)))
            action.SwitchState(action.Reload);

        if(Input.mouseScrollDelta.y != 0)
        {
            scrollDirection = Input.mouseScrollDelta.y;
            action.SwitchState(action.Swap);
        }
    }

    bool CanReload(ActionStateManager action)
    {
        if (action.ammo.currentAmmo == action.ammo.clipSize)
            return false;
        else if(action.ammo.extraAmmo <= 0)
            return false;
        else 
            return true;
    }
}
