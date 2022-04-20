using UnityEngine;
using RpgAdventure;

[System.Serializable]
public class PlayerScanner
{

    public float detectionRadius = 10;
    public float detectionAngle = 90.0f;

    public PlayerController Detect(Transform detector)
    {
        if (PlayerController.instance == null)
            return null;

        Vector3 toPlayer = PlayerController.instance.transform.position - detector.position;
        toPlayer.y = 0;

        if (toPlayer.magnitude <= detectionRadius)
        {
            if (Vector3.Dot(toPlayer.normalized, detector.forward) > Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
            {
                return PlayerController.instance;
            }
        }
        return null;
    }
}
