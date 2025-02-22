using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlow : MonoBehaviour
{
    public float slowDownFactor;
    public float slowDownDuration;
    public float neededMana;
    public bool TimeSlowActive;
    private float originalFixedDeltaTime;
    private void Start()
    {
        originalFixedDeltaTime = Time.fixedDeltaTime;
       TimeSlowActive = false;
        
    }
   
    public IEnumerator ActiveTimeSlow()
    {
        TimeSlowActive = true;
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = originalFixedDeltaTime*slowDownFactor;
        yield return new WaitForSecondsRealtime(slowDownDuration);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = originalFixedDeltaTime;
        TimeSlowActive=false;
    }
}
