﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Unit : MonoBehaviour {
    public float health = 100f;
    public bool enemy;
    public bool dead = false;
    public Transform body;

    private new Collider collider;

    protected virtual void Start() {
        collider = GetComponent<Collider>();
        if (collider == null)
            Debug.Log(name);
    }

    protected virtual void Update() {
        if(health <= 0 && !dead)
        {
            DropItem di = this.GetComponent<DropItem>();
            if (di != null)
            {
                di.Drop();
            }
            Die();
            dead = true;
        }
    }

    public float Distance(Unit other) {
        Vector3 p1 = collider.ClosestPoint(other.transform.position);
        Vector3 p2 = other.collider.ClosestPoint(transform.position);
        return Vector3.Distance(p1, p2);
    }

    protected virtual void Die() {
        Destroy(gameObject);
    }

    protected virtual void Die(float timeToDie)
    {
        Destroy(gameObject, timeToDie);
    }
}
