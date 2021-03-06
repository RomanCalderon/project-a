﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent ( typeof ( BoxCollider2D ) )]
public class Pickup : MonoBehaviour
{
    public enum PickupTypes
    {
        COIN,
        POWERUP,
        ENERGY_DROP,
        EVO_DROP,
        LIFE_DROP
    }

    [SerializeField]
    private PickupTypes m_pickupType = PickupTypes.COIN;

    private BoxCollider2D m_boxCollider;

    private void Awake ()
    {
        m_boxCollider = GetComponent<BoxCollider2D> ();
    }

    private void OnEnable ()
    {
        GameManager.onLevelClosed += OnLevelClosed;
    }

    private void OnDisable ()
    {
        GameManager.onStartNewGame -= OnLevelClosed;
    }

    public PickupTypes GetPickupType ()
    {
        return m_pickupType;
    }

    private void OnLevelClosed ()
    {
        if ( this != null )
        {
            Destroy ( gameObject );
        }
    }
}
