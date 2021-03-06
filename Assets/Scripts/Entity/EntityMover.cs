﻿using System.Linq;
using System.Collections;
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

    public bool IsMoving { get; private set; }
    private Directions m_direction;
    private Directions m_desiredDirection;
    private Node.NodeType [] m_invalidNodeTypes;

    public EntityMover ( Node defaultPosition, Directions defaultDirection, Node.NodeType [] invalidNodeTypes ) : base ( defaultPosition )
    {
        m_direction = defaultDirection;
        m_desiredDirection = defaultDirection;
        m_invalidNodeTypes = invalidNodeTypes;
    }

    public Node GetCurrentPosition ()
    {
        return m_position;
    }

    public void SetCurrentPosition ( Grid grid, Vector3 worldPosition )
    {
        m_position = grid.NodeFromWorldPoint ( worldPosition );
    }

    public void Move ()
    {
        Node desiredStepNode = GetStepNode ( m_desiredDirection );
        Node stepNode = GetStepNode ( m_direction );

        if ( IsValidStep ( desiredStepNode ) )
        {
            IsMoving = true;

            // Check if desired step node is a loop node
            if ( IsLoopStep ( desiredStepNode ) )
            {
                m_position = GetLoopDestination ( desiredStepNode );
            }
            else
            {
                m_direction = m_desiredDirection;
                m_position = desiredStepNode;
            }
        }
        else if ( IsValidStep ( stepNode ) )
        {
            IsMoving = true;

            // Check if step node is a loop node
            if ( IsLoopStep ( stepNode ) )
            {
                m_position = GetLoopDestination ( stepNode );
            }
            else
            {
                m_position = stepNode;
            }
        }
        else
        {
            IsMoving = false;
        }
    }

    public void Stop ()
    {
        IsMoving = false;
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
        if ( stepNode == null )
        {
            return false;
        }
        bool invalidStep = m_invalidNodeTypes.Contains ( stepNode.Type );
        return stepNode != null && !invalidStep;
    }

    private bool IsLoopStep ( Node stepNode )
    {
        if ( stepNode == null )
        {
            return false;
        }
        return stepNode.Type == Node.NodeType.LOOP_POINT;
    }

    private Node GetLoopDestination ( Node loopNode )
    {
        return loopNode.LoopNode;
    }
}
