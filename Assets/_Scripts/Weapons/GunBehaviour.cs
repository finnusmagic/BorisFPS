using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunBehaviour : MonoBehaviour {

    [Header("Weapon Config")]
    [SerializeField] float damage = 10f;
    [SerializeField] float range = 100f;
    [SerializeField] float weaponStrength = 1f;
    [SerializeField] float fireRate = 1f;
    [SerializeField] bool isAutomatic = false;

    [Header("Ammunition Config")]
    [SerializeField] Animator anim;
    [SerializeField] int maxAmmo = 10;
    [SerializeField] float reloadTime = 1f;

    [Header("General Config")]
    [SerializeField] Text ammunitionText;
    [SerializeField] Text modeText;
    [SerializeField] Camera weaponCamera;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] ParticleSystem impactEffect;

    int currentAmmo;
    float nextTimeToFire = 0f;
    bool isReloading = false;
    bool automaticMode;

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    private void OnEnable()
    {
        isReloading = false;
        anim.SetBool("Reloading", false);
    }

    void Update ()
    {
        ammunitionText.text = "Amn. : " + currentAmmo + "/" + maxAmmo;

        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            if (Input.GetKeyDown(KeyCode.R))
                StartCoroutine(Reload());
            return;
        }

        if (currentAmmo < maxAmmo && Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
        if (Input.GetKeyDown(KeyCode.B) && isAutomatic)
        {
            automaticMode = !automaticMode;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && isAutomatic && automaticMode)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        else if (Input.GetButtonDown("Fire1") && !automaticMode)
        {
            Shoot();
        }

        if (automaticMode)
            modeText.text = "Mode: Automatic";
        else
            modeText.text = "Mode: Singlefire";

    }

    void Shoot()
    {
        muzzleFlash.Play();

        currentAmmo--;

        RaycastHit hit;
        if (Physics.Raycast(weaponCamera.transform.position, weaponCamera.transform.forward, out hit, range))
        {
            Destructible target = hit.transform.GetComponent<Destructible>();

            if(target != null)
            {
                target.TakeDamage(damage);
            }
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * weaponStrength);
            }

            ParticleSystem bulletHole = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            bulletHole.transform.SetParent(hit.transform);
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        anim.SetBool("Reloading", true);
        yield return new WaitForSeconds(reloadTime - 0.25f);
        anim.SetBool("Reloading", false);
        yield return new WaitForSeconds(0.25f);
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
