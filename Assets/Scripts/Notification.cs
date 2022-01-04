using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    
    //Notification
    public bool useNotification = true;
    public CanvasGroup notificationCanvasGroup;
    public static Text notificationText;
    public static bool notificationChange;
    public bool notificationChangeCheck;
    public float notificationAlphaTarget;
    public float notificationAlphaStart;
    public float notificationLerpTimer;
    public float notificationWaitTimer;
    public float oldnotificationWaitTimer;
    public bool notificationWait;
    public bool notificationWaitRemove;
    public bool notificationCanDelete;
    public bool notificationChanged;
    public bool notificationAlphaChange;
    public bool notificationAlphaDown;

    public float actualAlpha;

    public List<string> statusNotificationQueue = new List<string>();
    public List<string> notificationQueue = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        //Sets Notification Area
        notificationCanvasGroup = GameObject.Find("HUD/Notification").GetComponent<CanvasGroup>();
        notificationText = GameObject.Find("HUD/Notification").GetComponent<Text>();
        notificationCanvasGroup.alpha = 0f;

        //Get PlayerStatus
        PlayerStatus playerStatus = GameObject.Find("First Person Player").GetComponent<PlayerStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        
        statusNotificationQueue = new List<string>();
        if ( (useNotification) )
        {
            QueueHandle();
            NotificationHandle();
        }
    }

    public void QueueHandle()
    {
        statusNotificationQueue = PlayerStatus.statusNotificationQueue;
        //itemNotificationQueue
        //radioNotificationQueue

        notificationQueue.AddRange(statusNotificationQueue); 

    }

    public void NotificationHandle()
    {
        
        if ( ( notificationQueue.Count > 0 ) )
        {
            
            notificationWaitRemove = false;
            //If we aren't waiting for one notification to finish displaying, 
            //then we can set the text to the new notification and reset the variables
            if( !( notificationWait ) )
            {
                notificationText.text = notificationQueue[0];
                notificationChange = true;
                notificationAlphaStart = notificationCanvasGroup.alpha;
                notificationAlphaTarget = 1;
                notificationWaitTimer = 0;
                notificationLerpTimer = 0;
                notificationWait = true;
            }

            //If we are waiting then we want to make sure the alpha is going to 1
            if ( ( notificationWait ) )
            {
                notificationLerpTimer = notificationLerpTimer + Time.deltaTime;
                notificationCanvasGroup.alpha = Mathf.Lerp(notificationAlphaStart, notificationAlphaTarget, notificationLerpTimer);

                //If the alpha is 1 then we can start waiting
                if ( ( notificationCanvasGroup.alpha == 1f ) )
                {
                    notificationWaitTimer = notificationWaitTimer + Time.deltaTime;

                    //Once the notification has been displayed for 5 seconds, 
                    //the wait variable can be set to false and the notification removed from the queue
                    if ( ( notificationWaitTimer > 5f ) )
                    {
                        notificationWait = false;
                        notificationQueue.RemoveAt(0);
                    }
                }
            }

        } else
        {
            if ( !(notificationWaitRemove) )
            {
                notificationAlphaStart = 1;
                notificationAlphaTarget = 0;
                notificationLerpTimer = 0;
            }
            notificationWaitRemove = true;
            notificationLerpTimer = notificationLerpTimer + Time.deltaTime;
            notificationCanvasGroup.alpha = Mathf.Lerp(notificationAlphaStart, notificationAlphaTarget, notificationLerpTimer);
        }
    }
}
