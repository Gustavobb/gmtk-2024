using UnityEngine;

public class CustomStopwatch
{
    private float startTime = 0f;
    private float elapsedTime = 0f;
    private bool created = false;

    public float ElapsedTimeSec()
    {
        return Time.time - startTime;
    }
    
    public bool LessThan(float seconds)
    {
        return ElapsedTimeSec() < seconds && created;
    }

    public void Restart()
    {
        created = true;
        startTime = Time.time;
    }
}
