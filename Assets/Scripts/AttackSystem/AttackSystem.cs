﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    //public KeyCode keyShoot = KeyCode.Q;
    public float fireDelay = 0.0001f;
    float fireElapsedTime;
    private GameObject projectile;

    [SerializeField] GameObject standardProjectile;
    [SerializeField] GameObject berserkProjectile;
    [SerializeField] float velocity = 1.0f;
    [SerializeField] float damage = 30.0f;
    [SerializeField] float berserkTime = 15.0f;
    float elapsedBerserkTime = 0.0f;
    [SerializeField] GameObject shooter;
    [SerializeField] float shotgunHoldTime = 1.0f;
    float holdTime = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        fireElapsedTime = fireDelay;
        projectile = standardProjectile;
    }

    // Update is called once per frame
    void Update()
    {
        if(projectile == berserkProjectile) {
            elapsedBerserkTime += Time.deltaTime;
            if(elapsedBerserkTime >= berserkTime) {
                switchMode();
                elapsedBerserkTime = 0.0f;
            }
        }

        fireElapsedTime += Time.deltaTime;
        if (Input.GetButton("Fire1")){
            holdTime += Time.deltaTime;
        }

        if (Input.GetButtonUp("Fire1")) {
            if (fireElapsedTime >= fireDelay) {
                if (holdTime < shotgunHoldTime) {
                    fireElapsedTime = 0.0f;
                    holdTime = 0;
                    GameObject projectile_shooted = Instantiate(projectile);
                    projectile_shooted.transform.position = shooter.transform.TransformPoint(Vector3.zero);
                    projectile_shooted.transform.rotation = Quaternion.Euler(projectile_shooted.transform.eulerAngles.x + shooter.transform.rotation.eulerAngles.x, shooter.transform.rotation.eulerAngles.y, shooter.transform.rotation.eulerAngles.z);
                    projectile_shooted.GetComponent<Shot>().damage = damage;
                    projectile_shooted.GetComponent<Rigidbody>().AddForce(velocity * 1000 * shooter.transform.forward, ForceMode.Force);
                }
                else if (holdTime >= shotgunHoldTime) {
                    //shotgun
                    fireElapsedTime = 0.0f;
                    holdTime = 0;
                    for (int i = 0; i < 10; i++) {
                        GameObject projectile_shooted = Instantiate(projectile);
                        projectile_shooted.GetComponent<Shot>().timeToLive = projectile_shooted.GetComponent<Shot>().shotgunTimeToLive;
                        projectile_shooted.transform.position = shooter.transform.TransformPoint(Vector3.zero);
                        projectile_shooted.transform.rotation = Quaternion.Euler(projectile_shooted.transform.eulerAngles.x + shooter.transform.rotation.eulerAngles.x, shooter.transform.rotation.eulerAngles.y, shooter.transform.rotation.eulerAngles.z);
                        projectile_shooted.GetComponent<Shot>().damage = damage;
                        Vector3 forwardForce = shooter.transform.forward * 1000 * velocity;
                        Vector3 rightJitter = shooter.transform.right * Random.Range(-100f, 100f);
                        Vector3 upJitter = shooter.transform.up * Random.Range(-100f, 100f);
                        projectile_shooted.GetComponent<Rigidbody>().AddForce(forwardForce + rightJitter + upJitter, ForceMode.Force);
                    }
                }
            }
        }
    }

    public void switchMode() {
        if (projectile == standardProjectile) {
            projectile = berserkProjectile;
            velocity = velocity*1.5f;
            fireDelay = fireDelay/1.5f;
            damage = damage * 10;
        }
        else {
            projectile = standardProjectile;
            velocity = velocity/1.5f;
            fireDelay = fireDelay*1.5f;
            damage = damage / 10;
        }
    }
}
