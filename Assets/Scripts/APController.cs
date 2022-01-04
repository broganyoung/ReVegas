using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class APController : MonoBehaviour
{

    public bool useAP = true;

    public float BaseAP = 100f;
    public static float currentAP = 100f;
    public float currentAPCheck = 100f;
    public float SprintDrain = 1f;
    public float baseAPRegen = 1f;
    public float currentAPRegen;
    public float currentAPDrain;
    public float secondsAP = 0.06f;

    public bool APAction = false;
    public bool APWaitCheck = false;
    public static bool APWait = false;

    public bool SprintCheck = false;

    public Slider APBar;

    // Start is called before the first frame update
    void Start()
    {
        //Pulls in the static varaibles from PlayerMovement (if player is sprinting)
        GameObject Player = GameObject.Find("First Person Player");
        PlayerMovement playerMovement = Player.GetComponent<PlayerMovement>();
        
        //Set Bar
        APBar = GameObject.Find("HUD/APBar").GetComponent<Slider>();

        //Sets max AP of the bar to the BaseAP, will need to change when player stats are introduced
        APBar.maxValue = BaseAP;

        if ( (useAP) )
        {
            InvokeRepeating("APDrain", 0f, 1f);
            InvokeRepeating("APRegen", 0f, secondsAP);
        }     
    }

    // Update is called once per frame
    void Update()
    {

        if ( (useAP) )
        {

            //Calls the waiting routine
            StartCoroutine(APWaiting());

            //Set Regen value
            currentAPRegen = baseAPRegen;

            //used to check if the player is doing an AP action
            //Only sprinting atm but will be used to also check for VATS
            if ( PlayerMovement.isSprinting )
            {
                APAction = true;
            } else {
                APAction = false;
                currentAPDrain = 0;
            }

            //If the player is sprinting then increase AP drain
            if ( PlayerMovement.isSprinting )
            {
                currentAPDrain = SprintDrain;
            } 

            //Debug
            if( !PlayerMovement.isSprinting ){
                SprintCheck = false;
            }

            //Sets AP bar value to current AP
            APBar.value = currentAP;

            //Debug
            currentAPCheck = currentAP;
        }

        
        IEnumerator APWaiting()
        {
            //If AP is drained to zero, prevent AP actions and wait 2 seconds
            if (currentAP == 0)
            {
                APWait = true;
                APWaitCheck = true;

                yield return new WaitForSeconds(2);

                APWait = false;
                APWaitCheck = false;
            }
        }
    }
    
    public void APDrain()
    {
        currentAP = currentAP - currentAPDrain;
    }
    
    public void APRegen()
    {
        if ( !(APAction) && (currentAP < BaseAP) )
        {
            currentAP = currentAP + currentAPRegen;
        }
    }
}
