using UnityEngine;
using System.Collections;

public class Escopeta2Canones : MonoBehaviour
{
    private float range = 30, impactForce = 30f, fireRate = 150f;
    private int damage = 300, maxAmmoLoad = 2, ammoLoad = 2, reloadTime = 1;
    private bool Recargando = false;
    private float nextTimetoFire = 0f;

    public AudioClip recarga;
    public AudioClip clip;
    public Animator animator;
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private void Start()
    {
        
        if (ammoLoad == -1)
        {
            ammoLoad = maxAmmoLoad;
        }
    }

    private void OnEnable()
    {
        Recargando = false;
        animator.SetBool("Reloading", false);
    }

    // Update is called once per frame
    void Update()
    {

        if (Recargando)
        {
            return;
        }
        if (ammoLoad <= 0 || (Input.GetKeyDown(KeyCode.R) && ammoLoad != maxAmmoLoad))
        {
            StartCoroutine(Reload());
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimetoFire) //Input.GetButton("Fire1") // para automatico
        {
            nextTimetoFire = Time.time + 1f / fireRate;
            Shoot();

        }
    }

    IEnumerator Reload()
    {
        reproducirAudio(recarga);
        Recargando = true;
        //Debug.Log("Reloading...");
        animator.SetBool("Reloading", true);
        yield return new WaitForSeconds(reloadTime-0.25f);
        animator.SetBool("Reloading", false);
        ammoLoad = maxAmmoLoad;
        Recargando = false;
    }

    void Shoot()
    {
        reproducirAudio(clip);
        

        muzzleFlash.Play();
        ammoLoad--;
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            //Debug.Log(hit.transform.name);

            ZombieAI target = hit.transform.GetComponent<ZombieAI>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(hit.normal * impactForce);
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 1f);
        }

    }

    void reproducirAudio(AudioClip clip1)
    {
        GameObject tempAudioSource = new GameObject("TempAudio");
        AudioSource audioSource = tempAudioSource.AddComponent<AudioSource>();
        audioSource.clip = clip1;
        audioSource.volume = 0.1f;
        audioSource.Play();

    }

    public int GetAmmoLoad()
    {
        return ammoLoad;
    }
}
