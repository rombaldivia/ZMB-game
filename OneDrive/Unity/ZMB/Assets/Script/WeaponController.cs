﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("General")]
    public LayerMask hittableLayers;
    public GameObject bulletHolePrefab;
    public Transform shootPoint;
    [Header("Shoot Paramaters")]
    public float fireRange = 200;
    public float Range = 10f ;
    public Vida vida;
    public float daño = 25;
    private Transform cameraPlayerTransform;

    private void Start()
    {
 
        cameraPlayerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    private void Update()
    {
        HandleShoot();
    }

    private void HandleShoot()
    {
        if (Input.GetButton("Fire1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(cameraPlayerTransform.position, cameraPlayerTransform.forward, out hit, fireRange, hittableLayers))
            {
                GameObject bulletHoleClone = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.01f, Quaternion.LookRotation(hit.normal));
                Destroy(bulletHoleClone, 4f);
                if (hit.transform.CompareTag("Enemigo"))
                    Vida.vida = hit.transform.GetComponent<Vida>();
                if (Vida.vida != null)
                { Vida.vida.RecibirDaño(daño); }

            }
        }
       
    }

}
