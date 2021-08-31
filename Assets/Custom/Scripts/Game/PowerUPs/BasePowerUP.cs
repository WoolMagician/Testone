using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class BasePowerUP : IPowerUP
{

    public float duration = 0f;

    protected bool countDownStarted = false;
    protected float currentCountdownValue;

    private IHasPowerUPs poweredUpObject;

    public virtual void ApplyPowerUP(IData data, IHasPowerUPs poweredUpObject)
    {
        this.poweredUpObject = poweredUpObject;

        if (!countDownStarted)
        {
            GameManager.Instance.StartCoroutine(StartCountdown(duration));
        }
    }

    public void StackDuration()
    {
        currentCountdownValue += duration;
    }

    public void ResetDuration()
    {
        currentCountdownValue = duration;
    }

    private IEnumerator StartCountdown(float countdownValue)
    {
        if (countdownValue > 0)
        {
            countDownStarted = true;
            currentCountdownValue = countdownValue;

            //Yield till object is expired
            while (currentCountdownValue > 0)
            {
                yield return new WaitForSeconds(1.0f);
                currentCountdownValue--;
            }     

            //Countdown is over, remove powerup
            if(poweredUpObject != null)
            {
                poweredUpObject.PowerUPs.Remove(this);
            }
        }
        else
        {
            yield return null;
        }
    }

    
}