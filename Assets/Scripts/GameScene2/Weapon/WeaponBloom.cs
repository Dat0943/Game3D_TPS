using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBloom : MonoBehaviour
{
    [SerializeField] private float defaultBloomAngle = 3f;
    [SerializeField] private float walkBloomMultiplier = 1.5f;
    [SerializeField] private float crouchBloomMultiplier = 0.5f;
    [SerializeField] private float sprintBloomMultiplier = 2f;
    [SerializeField] private float adsBloomMultiplier = 0.5f;

    MovementStateManager movement;
    AimStateManager aim;

    float currentBloom;

    void Awake()
    {
        movement = GetComponentInParent<MovementStateManager>();
        aim = GetComponentInParent<AimStateManager>();
    }

    public Vector3 BloomAngle(Transform barrelPosition)
    {
        if (movement.currentState == movement.Idle)
            currentBloom = defaultBloomAngle;
        else if (movement.currentState == movement.Walk)
            currentBloom = defaultBloomAngle * walkBloomMultiplier;
        else if (movement.currentState == movement.Run)
            currentBloom = defaultBloomAngle * sprintBloomMultiplier;
        else if(movement.currentState == movement.Crouch)
        {
            if (movement.direction.magnitude == 0)
                currentBloom = defaultBloomAngle * crouchBloomMultiplier;
            else
                currentBloom = defaultBloomAngle * walkBloomMultiplier * crouchBloomMultiplier;
        }

        if (aim.currentState == aim.Aim)
            currentBloom *= adsBloomMultiplier;

        float randX = Random.Range(-currentBloom, currentBloom);
        float randY = Random.Range(-currentBloom, currentBloom);
        float randZ = Random.Range(-currentBloom, currentBloom);

        Vector3 randomRotation = new Vector3(randX, randY, randZ);
        return barrelPosition.localEulerAngles + randomRotation;
    }
}
