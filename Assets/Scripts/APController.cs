using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class APController : MonoBehaviour
{

    public bool UseAP = true;

    public float BaseAP = 100f;
    public static float CurrentAP = 100f;
    public float CurrentAPCheck = 100f;
    public float SprintDrain = 100f;
    public float APRegen = 100f;

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
    }

    // Update is called once per frame
    void Update()
    {

        if ( (UseAP) )
        {

            //Calls the waiting routine
            StartCoroutine(APWaiting());

            //Used to check if the player is doing an AP action
            //Only sprinting atm but will be used to also check for VATS
            if ( PlayerMovement.isSprinting )
            {
                APAction = true;
            } else {
                APAction = false;
            }

            //If the player is sprinting then reduce the current AP
            if ( PlayerMovement.isSprinting )
            {
                if ( (CurrentAP -= Mathf.RoundToInt(SprintDrain * Time.deltaTime)) > 0 ){
                    CurrentAP -= Mathf.RoundToInt(SprintDrain * Time.deltaTime);
                }
                SprintCheck = true;
            }

            //Debug
            if( !PlayerMovement.isSprinting ){
                SprintCheck = false;
            }

            //If no AP action is being performed and the Current AP is less than Base AP
            //then start the regen of AP
            if ( !(APAction) && (CurrentAP < BaseAP) )
            {
                CurrentAP += Mathf.RoundToInt(APRegen * Time.deltaTime);
            }

            //Sets AP bar value to current AP
            APBar.value = CurrentAP;

            //Debug
            CurrentAPCheck = CurrentAP;
        }

        IEnumerator APWaiting()
        {
            //If AP is drained to zero, prevent AP actions and wait 2 seconds
            if (CurrentAP == 0)
            {
                APWait = true;
                APWaitCheck = true;

                yield return new WaitForSeconds(2);

                APWait = false;
                APWaitCheck = false;
            }
        }
    }
}
