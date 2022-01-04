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
    public float baseHunger = 1000f;
    public float currentHunger;
    public float walkingHungerDrain = 1f;
    public float sprintingHungerDrain = 1f;
    public float currentHungerDrain;
    public float secondsHunger = 25f;
    public Slider HungerBar;
    public float levelnewHunger;
    public float leveloldHunger;
    public string queueAddHunger;

    //Thirst
    public bool useThirst = true;
    public float baseThirst = 1000f;
    public float currentThirst;
    public float walkingThirstDrain = 1f;
    public float sprintingThirstDrain = 1f;
    public float currentThirstDrain;
    public float secondsThirst = 10f;
    public Slider ThirstBar;
    public float levelnewThirst;
    public float leveloldThirst;
    public string queueAddThirst;

    //Sleep
    public bool useSleep = true;
    public float baseSleep = 1000f;
    public float currentSleep;
    public float walkingSleepDrain = 1f;
    public float sprintingSleepDrain = 1f;
    public float currentSleepDrain;
    public float secondsSleep = 50f;
    public Slider SleepBar;
    public float levelnewSleep;
    public float leveloldSleep;
    public string queueAddSleep;

    //Radiation
    public bool useRadiation = true;
    public float baseRadiation = 0f;
    public float maxRadiation = 1000f;
    public float currentRadiation;
    public float currentRadiationDrain;
    public float secondsRadiation = 1f;
    public Slider RadiationBar;
    public Transform radiationCheck;
    public float radiationDistance = 0.4f;
    public LayerMask radiationMask;
    public bool isRadiated;
    public Text RadiationCount;
    public float levelnewRadiation;
    public float leveloldRadiation;
    public string queueAddRadiation;

    public CanvasGroup radiationCanvasGroup;
    public float radiationAlphaOld;
    public float radiationAlphaNew;

    //Poison
    public bool usePoison = true;
    public bool isPoisoned;
    public CanvasGroup indicatorPoisonCanvasGroup;
    public float levelnewPoison;
    public float leveloldPoison;
    public string queueAddPoison;

    //Limb Status
    public bool useLimb = true;
    public float crippledLimbs;
    public CanvasGroup indicatorLimbCanvasGroup;
    public Text indicatorLimbText;
    public Color32 indicatorLimbColour1 = new Color32(153, 153, 0, 255);
    public Color32 indicatorLimbColour2 = new Color32(154, 20, 0, 255);
    public Color32 indicatorLimbColour3 = new Color32(196, 26, 0, 255);
    public Color32 indicatorLimbColour4 = new Color32(243, 32, 0, 255);
    public float levelnewLimb;
    public float leveloldLimb;
    public string queueAddLimb;

    //Notification
    public bool useNotification = true;
    public float countNotification;

    //Sets Notification Queue
    public static List<string> statusNotificationQueue = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        //Rest stats
        currentHunger = baseHunger;
        currentThirst = baseThirst;
        currentSleep = baseSleep;
        currentHealth = baseThirst;
        currentRadiation = baseRadiation;
        
        //Pulls in the static varaibles from PlayerMovement (if player is sprinting)
        GameObject Player = GameObject.Find("First Person Player");
        PlayerMovement playerMovement = Player.GetComponent<PlayerMovement>();

        //Set bars
        HealthBar = GameObject.Find("HUD/HealthBar").GetComponent<Slider>();
        HungerBar = GameObject.Find("HUD/HungerBar").GetComponent<Slider>();
        ThirstBar = GameObject.Find("HUD/ThirstBar").GetComponent<Slider>();
        SleepBar = GameObject.Find("HUD/SleepBar").GetComponent<Slider>();
        RadiationBar = GameObject.Find("HUD/RadiationBar").GetComponent<Slider>();
        RadiationCount = GameObject.Find("HUD/RadiationBar/RadCount").GetComponent<Text>();

        //Set Bar max values
        HungerBar.maxValue = baseHunger;
        ThirstBar.maxValue = baseThirst;
        SleepBar.maxValue = baseSleep;
        HealthBar.maxValue = baseHealth;
        RadiationBar.maxValue = maxRadiation;

        //Sets radiation check and mask
        radiationCheck = Player.GetComponent<Transform>();
        radiationMask = LayerMask.GetMask("Radiation");

        //Sets radiation canvas group
        radiationCanvasGroup = GameObject.Find("HUD/RadiationBar").GetComponent<CanvasGroup>();

        //Sets Status Indicators 
        indicatorPoisonCanvasGroup = GameObject.Find("HUD/HealthBar/PoisonIndicator").GetComponent<CanvasGroup>(); 
        indicatorLimbCanvasGroup = GameObject.Find("HUD/HealthBar/LimbIndicator").GetComponent<CanvasGroup>();
        indicatorLimbText = GameObject.Find("HUD/HealthBar/LimbIndicator").GetComponent<Text>();

        //Sets Notification Area
        Notification notification = Player.GetComponent<Notification>();


         if ( (useHunger) )
         {
             InvokeRepeating("HungerDrain", secondsHunger, secondsHunger);
         }         
         
         if ( (useThirst) )
         {
             InvokeRepeating("ThirstDrain", secondsThirst, secondsThirst);
         }         
         
         if ( (useSleep) )
         {
             InvokeRepeating("SleepDrain", secondsSleep, secondsSleep);
         }            

         if ( (useRadiation) )
         {
             InvokeRepeating("RadiationDrain", secondsRadiation, secondsRadiation);
         }

         if ( (useHealth) )
         {
             Health();
         }
    }

    // Update is called once per frame
    void Update()
    {
        //Resets Queue on each frame
        statusNotificationQueue = new List<string>();
        countNotification = 0f;
        
         if ( (useHunger) )
         {
             Hunger();
         }         
         
         if ( (useThirst) )
         {
             Thirst();
         }         
         
         if ( (useSleep) )
         {
             Sleep();
         }            

         if ( (useRadiation) )
         {
             Radiation();
         }

         if ( (useHealth) )
         {
             Health();
         }

         if ( (usePoison) )
         {
             Poison();
         }

         if ( (useLimb) )
         {
             Limb();
         }


         NotificationCheck();
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
            currentHungerDrain = sprintingHungerDrain;
        } else
        {
            currentHungerDrain = walkingHungerDrain;
        }
        
        //Changes HungerBar value
        HungerBar.value = currentHunger;

        //Sets status effects based on Hunger Level
        if ( (currentHunger <= 0) )
        {
            currentHealth = 0;
            levelnewHunger = 0;
        }
        if ( (currentHunger <= 800) )
        {
            queueAddHunger = "You are now sick with Minor Starvation";
            levelnewHunger = 1;
        }
        if ( (currentHunger <= 600) )
        {
            queueAddHunger = "You are now sick with Advanced Starvation";
            levelnewHunger = 2;
        }
        if ( (currentHunger <= 400) )
        {
            queueAddHunger = "You are now sick with Critical Starvation";
            levelnewHunger = 3;
        }
        if ( (currentHunger <= 200) )
        {
            queueAddHunger = "You are now sick with Deadly Starvation";
            levelnewHunger = 4;
        }

        if ( (levelnewHunger != leveloldHunger) )
        {
            countNotification = countNotification + 1f;
            statusNotificationQueue.Add(queueAddHunger);
        }
        leveloldHunger = levelnewHunger;
    }
    
    public void HungerDrain()
    {
        currentHunger = currentHunger - currentHungerDrain;
    }

    public void Thirst()
    {
        //Checks if player is sprinting so uses more thirst
        if ( (PlayerMovement.isSprinting) ){
            currentThirstDrain = sprintingThirstDrain;
        } else
        {
            currentThirstDrain = walkingThirstDrain;
        }

        //Changes ThirstBar value
        ThirstBar.value = currentThirst;

        //Sets status effects based on Thirst Level
        if ( (currentThirst <= 0) )
        {
            currentHealth = 0;
            levelnewThirst = 0;
        }
        if ( (currentThirst <= 800) )
        {
            queueAddThirst = "You are now sick with Minor Dehydration";
            levelnewThirst = 1;
        }
        if ( (currentThirst <= 600) )
        {
            queueAddThirst = "You are now sick with Advanced Dehydration";
            levelnewThirst = 2;
        }
        if ( (currentThirst <= 400) )
        {
            queueAddThirst = "You are now sick with Critical Dehydration";
            levelnewThirst = 3;
        }
        if ( (currentThirst <= 200) )
        {
            queueAddThirst = "You are now sick with Deadly Dehydration";
            levelnewThirst = 4;
        }

        if ( (levelnewThirst != leveloldThirst) )
        {
            countNotification = countNotification + 1f;
            statusNotificationQueue.Add(queueAddThirst);
        }

        leveloldThirst = levelnewThirst;
    }
    
    public void ThirstDrain()
    {
        currentThirst = currentThirst - currentThirstDrain;
    }
    
    public void Sleep()
    {
        //Checks if player is sprinting so uses more thirst
        if ( (PlayerMovement.isSprinting) ){
            currentSleepDrain = sprintingSleepDrain;
        } else
        {
            currentSleepDrain = walkingSleepDrain;
        }

        //Changes SleepBar value
        SleepBar.value = currentSleep;

        //Sets health to 0 if Sleep is less than or equal to 0
        if ( (currentSleep <= 0) )
        {
            currentHealth = 0;
        }

        //Sets status effects based on Sleep Level
        if ( (currentSleep <= 0) )
        {
            currentHealth = 0;
            levelnewSleep = 0;
        }
        if ( (currentSleep <= 800) )
        {
            queueAddSleep = "You are now sick with Minor Sleep Deprivation";
            levelnewSleep = 1;
        }
        if ( (currentSleep <= 600) )
        {
            queueAddSleep = "You are now sick with Advanced Sleep Deprivation";
            levelnewSleep = 2;
        }
        if ( (currentSleep <= 400) )
        {
            queueAddSleep = "You are now sick with Critical Sleep Deprivation";
            levelnewSleep = 3;
        }
        if ( (currentSleep <= 200) )
        {
            queueAddSleep = "You are now sick with Deadly Sleep Deprivation";
            levelnewSleep = 4;
        }

        if ( (levelnewSleep != leveloldSleep) )
        {
            countNotification = countNotification + 1f;
            statusNotificationQueue.Add(queueAddSleep);
        }

        leveloldSleep = levelnewSleep;
        
    }
    
    public void SleepDrain()
    {
        currentSleep = currentSleep - currentSleepDrain;
    }
    
    public void Radiation()
    {
        //Check if player is irradiated
        isRadiated = Physics.CheckSphere(radiationCheck.position, radiationDistance, radiationMask);

        //Sets Drain and alpha to 0 if not irradiated
        if ( (isRadiated) )
        {
            radiationAlphaNew = 1;   
        } else 
        {
            radiationAlphaNew = 0;   
            currentRadiationDrain = 0;
        }
        
        //Fade radiation bar using alpha
        radiationCanvasGroup.alpha = Mathf.Lerp(radiationCanvasGroup.alpha, radiationAlphaNew, 2 * Time.deltaTime);
        
        //Changes RadiationBar value
        RadiationBar.value = currentRadiation;
        RadiationCount.text = string.Concat(string.Format("{0:N0}", currentRadiationDrain), " RADS");

        //Sets status effects based on Radiation Level
        if ( (currentRadiation >= 1000) )
        {
            currentHealth = 0;
            levelnewRadiation = 0;
        }
        if ( (currentRadiation >= 200) )
        {
            queueAddRadiation = "You are now sick with Minor Radiation Poisoning";
            levelnewRadiation = 1;
        }
        if ( (currentRadiation >= 400) )
        {
            queueAddRadiation = "You are now sick with Advanced Radiation Poisoning";
            levelnewRadiation = 2;
        }
        if ( (currentRadiation >= 600) )
        {
            queueAddRadiation = "You are now sick with Critical Radiation Poisoning";
            levelnewRadiation = 3;
        }
        if ( (currentRadiation >= 800) )
        {
            queueAddRadiation = "You are now sick with Deadly Radiation Poisoning";
            levelnewRadiation = 4;
        }

        if ( (levelnewRadiation != leveloldRadiation) )
        {
            countNotification = countNotification + 1f;
            statusNotificationQueue.Add(queueAddRadiation);
        }

        leveloldRadiation = levelnewRadiation;

    }
    
    public void RadiationDrain()
    {
        currentRadiation = currentRadiation + currentRadiationDrain;
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

    public void Poison()
    {
        if ( (isPoisoned) )
        {
            indicatorPoisonCanvasGroup.alpha = Mathf.Lerp(indicatorPoisonCanvasGroup.alpha, 1, 2 * Time.deltaTime);
            queueAddPoison = "You feel a little woozy";
            levelnewPoison = 1;
        }
        if ( !(isPoisoned) )
        {
            indicatorPoisonCanvasGroup.alpha = Mathf.Lerp(indicatorPoisonCanvasGroup.alpha, 0, 2 * Time.deltaTime);
            queueAddPoison = "You are no longer poisoned";
            levelnewPoison = 0;
        }

        if ( (levelnewPoison != leveloldPoison) )
        {
            countNotification = countNotification + 1f;
            statusNotificationQueue.Add(queueAddPoison);
        }

        leveloldPoison = levelnewPoison;
    }

    public void Limb()
    {
        if ( (crippledLimbs == 0) )
        {
            indicatorLimbCanvasGroup.alpha = Mathf.Lerp(indicatorLimbCanvasGroup.alpha, 0f, 2 * Time.deltaTime);
            indicatorLimbText.color = Color.Lerp(indicatorLimbText.color, indicatorLimbColour1, 2 * Time.deltaTime);
            queueAddLimb = "PLACEHOLDER";
            levelnewLimb = crippledLimbs;
        }
        if ( (crippledLimbs == 1) )
        {
            indicatorLimbCanvasGroup.alpha = Mathf.Lerp(indicatorLimbCanvasGroup.alpha, 0.25f, 2 * Time.deltaTime);
            indicatorLimbText.color = Color32.Lerp(indicatorLimbText.color, indicatorLimbColour1, 2 * Time.deltaTime);
            queueAddLimb = "PLACEHOLDER";
            levelnewLimb = crippledLimbs;
        }
        if ( (crippledLimbs == 2) )
        {
            indicatorLimbCanvasGroup.alpha = Mathf.Lerp(indicatorLimbCanvasGroup.alpha, 0.5f, 2 * Time.deltaTime);
            indicatorLimbText.color = Color32.Lerp(indicatorLimbText.color, indicatorLimbColour1, 2 * Time.deltaTime);
            queueAddLimb = "PLACEHOLDER";
            levelnewLimb = crippledLimbs;
        }
        if ( (crippledLimbs == 3) )
        {
            indicatorLimbCanvasGroup.alpha = Mathf.Lerp(indicatorLimbCanvasGroup.alpha, 1, 2 * Time.deltaTime);
            indicatorLimbText.color = Color32.Lerp(indicatorLimbText.color, indicatorLimbColour1, 2 * Time.deltaTime);
            queueAddLimb = "PLACEHOLDER";
            levelnewLimb = crippledLimbs;
        }
        if ( (crippledLimbs == 4) )
        {
            indicatorLimbCanvasGroup.alpha = Mathf.Lerp(indicatorLimbCanvasGroup.alpha, 1, 2 * Time.deltaTime);
            indicatorLimbText.color = Color32.Lerp(indicatorLimbText.color, indicatorLimbColour2, 2 * Time.deltaTime);
            queueAddLimb = "PLACEHOLDER";
            levelnewLimb = crippledLimbs;
        }
        if ( (crippledLimbs == 5) )
        {
            indicatorLimbCanvasGroup.alpha = Mathf.Lerp(indicatorLimbCanvasGroup.alpha, 1, 2 * Time.deltaTime);
            indicatorLimbText.color = Color32.Lerp(indicatorLimbText.color, indicatorLimbColour3, 2 * Time.deltaTime);
            queueAddLimb = "PLACEHOLDER";
            levelnewLimb = crippledLimbs;
        }
        if ( (crippledLimbs == 6) )
        {
            indicatorLimbCanvasGroup.alpha = Mathf.Lerp(indicatorLimbCanvasGroup.alpha, 1, 2 * Time.deltaTime);
            indicatorLimbText.color = Color32.Lerp(indicatorLimbText.color, indicatorLimbColour4, 2 * Time.deltaTime);
            queueAddLimb = "PLACEHOLDER";
            levelnewLimb = crippledLimbs;
        }

        if ( (levelnewLimb != leveloldLimb) )
        {
            countNotification = countNotification + 1f;
            statusNotificationQueue.Add(queueAddLimb);
        }

        leveloldLimb = levelnewLimb;
    }

    public void NotificationCheck()
    {
        if ( (countNotification > 0) )
        {
            Notification.notificationChange = true;
        } else {
            Notification.notificationChange = false;
        }
    }

}
