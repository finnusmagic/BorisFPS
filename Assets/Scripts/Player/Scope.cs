using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Scope : MonoBehaviour {

    [SerializeField] Animator anim;
    [SerializeField] GameObject scopeOverlay;
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject weaponCamera;
    [SerializeField] float scopeSensitivity = 0.5f;

    [SerializeField] float scopedFOV = 15f;
    float normalFOV;

    bool isScoped = false;

    PlayerController playerController;
    float currentLookSensivitiy;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire2"))
        {
            isScoped = !isScoped;
            anim.SetBool("Scoped", isScoped);

            if (isScoped)
                StartCoroutine(OnScoped());
            else
                OnUnScope();
        }
    }

    void OnUnScope()
    {
        scopeOverlay.SetActive(false);
        weaponCamera.SetActive(true);

        mainCamera.fieldOfView = normalFOV;
        playerController.lookSensitivity = currentLookSensivitiy;
        Debug.Log(playerController.lookSensitivity);
    }

    IEnumerator OnScoped()
    {
        yield return new WaitForSeconds(.15f);
        scopeOverlay.SetActive(true);
        weaponCamera.SetActive(false);

        normalFOV = mainCamera.fieldOfView;
        mainCamera.fieldOfView = scopedFOV;

        currentLookSensivitiy = playerController.lookSensitivity;
        playerController.lookSensitivity = scopeSensitivity;
        Debug.Log(playerController.lookSensitivity);
    }
}
