using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class ThirdPersonShooterController : MonoBehaviour {

    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    private ThirdPersonController thirdPersonController;

    private void Awake() {
        thirdPersonController = GetComponent<ThirdPersonController>();
    }

    private void Update() {
        if (Input.GetMouseButton(1)) {
            //thirdPersonController.SetSensitivty(aimSensitivity);
            aimVirtualCamera.gameObject.SetActive(true);
        } else {
            //thirdPersonController.SetSensitivty(normalSensitivity);
            aimVirtualCamera.gameObject.SetActive(false);
        }
    }

}
