using System;
using UnityEngine;

public class LevelGoal : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnGoalReached?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler OnGoalReached;
}
