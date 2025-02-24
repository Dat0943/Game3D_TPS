using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadState : ActionBaseState
{
    public override void EnterState(ActionStateManager action)
    {
        /* Khi Animation Reload được thực hiện thì nó sẽ thực hiện Animation chuẩn chỉnh chứ không bị
           cản trở bởi các khớp tay của IK nữa */
        action.rHandAim.weight = 0;
        action.lHandIK.weight = 0;

        action.anim.SetTrigger("Reload");
    }

    public override void UpdateState(ActionStateManager action)
    {
        
    }
}
