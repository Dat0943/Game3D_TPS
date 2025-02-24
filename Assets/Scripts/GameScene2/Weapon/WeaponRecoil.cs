using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [HideInInspector] public Transform recoilFollowPosition;
    [SerializeField] private float kickBackAmount = -1f;
    [SerializeField] private float kickBackSpeed = 10f, returnSpeed = 20f;
    float currentRecoilPosition, finalRecoilPosition;

    void Update()
    {
        currentRecoilPosition = Mathf.Lerp(currentRecoilPosition, 0, returnSpeed * Time.deltaTime);
        finalRecoilPosition = Mathf.Lerp(finalRecoilPosition, currentRecoilPosition, kickBackSpeed * Time.deltaTime);

        recoilFollowPosition.localPosition = new Vector3(0, 0, finalRecoilPosition);
    }

    // Hàm giật được gán vào Fire của súng
    public void TriggerRecoil()
    {
        currentRecoilPosition += kickBackAmount;
    }
}
