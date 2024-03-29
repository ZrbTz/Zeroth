﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Inventory inventory;
    //public KeyCode placeTurret = KeyCode.E;
    //public KeyCode throwSecondary = KeyCode.Q;
    private GameManager gameManager;

    // Weapon Inputs
    public bool WeaponSwitched { get; set; }
    public bool WeaponFire { get; set; }
    public bool WeaponFireDown { get; set; }

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        inventory = gameObject.GetComponent<Inventory>();
    }

    void Update()
    {
        GetPauseMenuInput();
        if (!gameManager.IsPaused())
        {
            GetInventoryInput();
            GetDashInput();
            GetWeaponInput();
        }
    }

    private void GetInventoryInput()
    {
        if (!this.GetComponent<InteractionController>().TryInteracting() && Input.GetButtonDown("Interact"))
        {
            if (inventory.UseItem(0))
            {
                return;
            }
        }
        if (Input.GetButtonDown("Throw"))
        {
            if (inventory.ShowThrowableTrajectory(1))
            {
                return;
            }
        }
        if (Input.GetButtonUp("Throw"))
        {
            if (inventory.UseItem(1))
            {
                return;
            }
        }
    }

    private void GetDashInput()
    {
        if (Input.GetButtonDown("Dash"))
        {
            this.GetComponent<ThirdPersonControllerDash>().Dash();
        }
    }

    private void GetPauseMenuInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            gameManager.HandlePause();
        }
    }

    private bool firingWithController = false;

    private void GetWeaponInput()
    {
        DoActionSuggestion action;
        if (Input.GetButtonDown("WeaponSwitch"))
        {
            WeaponSwitched = true;
            if (SuggestionController.Instance.actions.TryGetValue("WeaponSwitch", out action)) action.incrementPressCounter();
        }
        else
        {
            WeaponSwitched = false;
        }

        WeaponFireDown = false;

        if (!Input.GetButton("Fire1")
            && WeaponFire
            && !firingWithController) {
            WeaponFire = false;
        }

        if (Input.GetButtonDown("Fire1")) {
            WeaponFire = WeaponFireDown = true;
            firingWithController = false;
        }

        if (Input.GetAxis("Fire1") == 0 
            && firingWithController)  {
            firingWithController = false;
            WeaponFire = false;
            WeaponFireDown = false;
        }

        if(Input.GetAxis("Fire1") != 0
            && !WeaponFireDown
            && !WeaponFire
            && !firingWithController) {
            WeaponFire = WeaponFireDown = true;
            firingWithController = true;
        }

        if (WeaponFireDown && SuggestionController.Instance.actions.TryGetValue("Fire1", out action)) action.incrementPressCounter();  
    }
}
