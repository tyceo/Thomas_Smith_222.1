using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : UnityEvent<Collider2D> { }

public static class EventSystem
{
    public static TriggerEvent onBallTriggerEnter = new TriggerEvent();
}