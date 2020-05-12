﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMover : Entity
{
    public enum Directions
    {
        UP,
        RIGHT,
        DOWN,
        LEFT
    }

    private Directions m_direction;
    private Directions m_desiredDirection;

    public EntityMover ( Node defaultPosition, Directions defaultDirection ) : base ( defaultPosition )
    {
        m_direction = defaultDirection;
        m_desiredDirection = defaultDirection;
    }

    public Node GetCurrentPosition ()
    {
        return m_position;
    }

    public void SetCurrentPosition ( Grid grid, Vector3 worldPosition )
    {
        m_position = grid.GetClosestNode ( worldPosition );
    }

    public void Move ()
    {
        Node desiredStepNode = GetStepNode ( m_desiredDirection );
        Node stepNode = GetStepNode ( m_direction );

        if ( IsValidStep ( desiredStepNode ) )
        {
            m_direction = m_desiredDirection;
            m_position = desiredStepNode;
        }
        else if ( IsValidStep ( stepNode ) )
        {
            m_position = stepNode;
        }
    }

    public void UpdateDirection ( Directions desiredDirection )
    {
        m_desiredDirection = desiredDirection;
    }

    public Node GetStepNode ( Directions direction )
    {
        switch ( direction )
        {
            case Directions.UP:
                return m_position.UpNode;
            case Directions.RIGHT:
                return m_position.RightNode;
            case Directions.DOWN:
                return m_position.DownNode;
            case Directions.LEFT:
                return m_position.LeftNode;
            default:
                break;
        }
        return null;
    }

    public bool IsValidStep ( Node stepNode )
    {
        return stepNode != null && stepNode.Type != Node.NodeType.WALL;
    }
}
