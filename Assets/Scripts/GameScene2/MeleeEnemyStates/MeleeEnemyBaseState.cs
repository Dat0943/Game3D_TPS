using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeEnemyBaseState 
{
    public abstract void EnterState(MeleeEnemyStateManager enemy);

    public abstract void UpdateState(MeleeEnemyStateManager enemy);
}
