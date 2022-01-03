using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{

    //Health
    public bool useHealth = true;
    public float baseHealth = 100f;
    public float currentHealth = 100f;
    public float currentHealthRegen;
    public Slider HealthBar;

    //Hunger
    public bool useHunger = true;
    public float baseHunger = 100f;
    public float currentHunger;
    public float walkingHungerDrain = 1f;
    public float sprintingHungerDrain = 2f;
    public float currentHungerDrain;
    public Slider HungerBar;

    //Thirst
    public bool useThirst = true;
    public float baseThirst = 100f;
    public float currentThirst;
    public float walkingThirstDrain = 1f;
    public float sprintingThirstDrain = 2f;
    public float currentThirstDrain;
    public Slider ThirstBar;

    //Radiation
    public bool useRadiation = true;
    public float baseRadiation = 0f;
    public float maxRadiation = 100f;
    public float currentRadiation;
    public float currentRadiationDrain;
    public Slider RadiationBar;
    public Transform radiationCheck;
    public float radiationDistance = 0.4f;
    public LayerMask radiationMask;
    public bool isRadiated;
    public Text RadiationCount;

    // Start is called before the first frame update
    void Start()
    {
        //Rest stats
        currentHunger = baseHunger;
        currentThirst = baseThirst;
        currentHealth = baseThirst;
        currentRadiation = baseRadiation;
        
        //Pulls in the static varaibles from PlayerMovement (if player is sprinting)
        GameObject Player = GameObject.Find("First Person Player");
        PlayerMovement playerMovement = Player.GetComponent<PlayerMovement>();

        //Set bars
        HealthBar = GameObject.Find("HUD/HealthBar").GetComponent<Slider>();
        HungerBar = GameObject.Find("HUD/HungerBar").GetComponent<Slider>();
        ThirstBar = GameObject.Find("HUD/ThirstBar").GetComponent<Slider>();
        RadiationBar = GameObject.Find("HUD/RadiationBar").GetComponent<Slider>();
        RadiationCount = GameObject.Find("HUD/RadiationBar/RadCount").GetComponent<Text>();

        //Set Bar max values
        HungerBar.maxValue = baseHunger;
        ThirstBar.maxValue = baseThirst;
        HealthBar.maxValue = baseHealth;
        RadiationBar.maxValue = maxRadiation;

        //Sets radiation check and mask
        radiationCheck = Player.GetComponent<Transform>();
        radiationMask = LayerMask.GetMask("Radiation");
    }

    // Update is called once per frame
    void Update()
    {
         if ( (useHunger) )
         {
             Hunger();
         }         
         
         if ( (useThirst) )
         {
             Thirst();
         }         

         if ( (useRadiation) )
         {
             Radiation();
         }

         if ( (useHealth) )
         {
             Health();
         }
    }

    public void Health()
    {
        //Reloads scene if health is less than or equal to 0
        if ( (currentHealth <= 0) )
        {
            SceneManager.LoadScene("Scene");
        }
    }

    public void Hunger()
    {
        //Checks if player is sprinting so uses more hunger
        if ( (PlayerMovement.isSprinting) ){
            currentHungerDrain = sprintingHungerDrain * Time.deltaTime;
        } else
        {
            currentHungerDrain = walkingHungerDrain * Time.deltaTime;
        }
        currentHunger = currentHunger - currentHungerDrain;

        //Changes HungerBar value
        HungerBar.value = currentHunger;

        //Sets health to 0 if Hunger is less than or equal to 0
        if ( (currentHunger <= 0) )
        {
            currentHealth = 0;
        }
    }
    
    public void Thirst()
    {
        //Checks if player is sprinting so uses more thirst
        if ( (PlayerMovement.isSprinting) ){
            currentThirstDrain = sprintingThirstDrain * Time.deltaTime;
        } else
        {
            currentThirstDrain = walkingThirstDrain * Time.deltaTime;
        }
        currentThirst = currentThirst - currentThirstDrain;

        //Changes ThirstBar value
        ThirstBar.value = currentThirst;

        //Sets health to 0 if Thirst is less than or equal to 0
        if ( (currentThirst <= 0) )
        {
            currentHealth = 0;
        }
    }
    
    public void Radiation()
    {
        //Check if player is irradiated
        isRadiated = Physics.CheckSphere(radiationCheck.position, radiationDistance, radiationMask);

        //Sets Drain to 0 if not irradiated
        if ( !(isRadiated) )
        {
            currentRadiationDrain = 0;
        }

        //Starts increasing radiation
        currentRadiation = currentRadiation + (currentRadiationDrain * Time.deltaTime);
        
        //Changes RadiationBar value
        RadiationBar.value = currentRadiation;
        RadiationCount.text = string.Concat(string.Format("{0:N0}", currentRadiationDrain), " RADS");

        //Sets health to 0 if Radiation is greater than or equal to max Radiation
        if ( (currentRadiation >= maxRadiation) )
        {
            currentHealth = 0;
        }

    }
    
    public void OnTriggerEnter(Collider Object)
    {

        //Checks if player is irradiated
        if ( (isRadiated) )
        {
        
        //Gets RadiationEffect component
        RadiationEffect RadiationEffect = Object.gameObject.GetComponent<RadiationEffect>();

        //Sets Radiation Drain
        currentRadiationDrain = currentRadiationDrain + RadiationEffect.radiationValue;

        }
    }    

    //Resets radiation drain when leaving a radiation area so that the new area applies?
    public void OnTriggerExit(Collider Object)
    {

        //Checks if player is irradiated
        if ( (isRadiated) )
        {
        
        //Gets RadiationEffect component
        RadiationEffect RadiationEffect = Object.gameObject.GetComponent<RadiationEffect>();

        //Sets Radiation Drain
        currentRadiationDrain = currentRadiationDrain - RadiationEffect.radiationValue;

        }
    }

}
