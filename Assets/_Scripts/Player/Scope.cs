using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour {

    [SerializeField] Animator anim;
    [SerializeField] GameObject scopeOverlay;
    [SerializeField] GameObject crossHair;
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject weaponCamera;
    [SerializeField] float scopeSensitivity = 0.5f;
    [SerializeField] bool isSniper = false;

    [SerializeField] float sniperScopedFOV = 15f;
    [SerializeField] float assaultScopedFOV = 45f;
    float normalFOV;

    bool isScoped = false;

    PlayerController playerController;
    float currentLookSensivitiy;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
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
        if (isSniper)
            scopeOverlay.SetActive(false);
        crossHair.SetActive(true);
        weaponCamera.SetActive(true);

        mainCamera.fieldOfView = normalFOV;
        playerController.lookSensitivity = currentLookSensivitiy;
    }

    IEnumerator OnScoped()
    {
        yield return new WaitForSeconds(.15f);

        normalFOV = mainCamera.fieldOfView;
        currentLookSensivitiy = playerController.lookSensitivity;

        if (isSniper)
        {
            playerController.lookSensitivity = scopeSensitivity;
            scopeOverlay.SetActive(true);
            mainCamera.fieldOfView = sniperScopedFOV;
            weaponCamera.SetActive(false);
        }
        else
        {
            mainCamera.fieldOfView = assaultScopedFOV;
        }

        crossHair.SetActive(false);

    }
}
