﻿using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public TextAsset gridConfigFile = null;
    public TextAsset nodeConfigFile = null;

    private GridConfigData gridData = null;
    private int [,] nodeData = null;

    public Node [,] NodeArray { get; private set; } = null;
    public int GridSizeX { get; private set; }
    public int GridSizeY { get; private set; }
    public int MaxSize
    {
        get
        {
            return GridSizeX * GridSizeY;
        }
    }

    public List<Node> Nodes { get; private set; }
    private List<Node> m_loopNodes = null;

    private void OnEnable ()
    {
        gridData = GridDataReader.ReadData ( gridConfigFile );
        nodeData = NodeDataReader.ReadData ( nodeConfigFile );

        CreateGrid ( gridData, nodeData );
    }

    public void CreateGrid ( GridConfigData gridData, int [,] nodeData )
    {
        Debug.Log ( "Building grid..." );

        if ( gridData == null )
        {
            throw new Exception ( "Grid data is null." );
        }

        NodeArray = new Node [ gridData.Size.X, gridData.Size.Y ];
        GridSizeX = gridData.Size.X;
        GridSizeY = gridData.Size.Y;
        Nodes = new List<Node> ();
        m_loopNodes = new List<Node> ();

        for ( int x = 0; x < gridData.Size.X; x++ )
        {
            for ( int y = 0; y < gridData.Size.Y; y++ )
            {
                if ( nodeData == null )
                {
                    throw new Exception ( "nodeData is null." );
                }

                Vector3 worldPos = new Vector3 ( x * gridData.Spacing.X + gridData.Offset.X, y * gridData.Spacing.Y + gridData.Offset.Y, 0 );
                NodeArray [ x, y ] = new Node ( x, y, worldPos, nodeData [ x, y ] );
                Nodes.Add ( NodeArray [ x, y ] );
            }
        }

        foreach ( Node n in NodeArray )
        {
            n.AssignNeighbors ( NodeArray );
            if ( n.Type == Node.NodeType.LOOP_POINT )
            {
                m_loopNodes.Add ( n );
            }
        }

        // Set loop nodes
        if ( m_loopNodes.Count >= 2 )
        {
            for ( int i = 1; i < m_loopNodes.Count; i++ )
            {
                m_loopNodes [ i ].SetLoopNode ( m_loopNodes [ i - 1 ] );
            }
            m_loopNodes [ 0 ].SetLoopNode ( m_loopNodes [ m_loopNodes.Count - 1 ] );
        }

        // Done building grid
        Debug.Log ( "Done building grid." );
    }

    public Node GetNode ( int posX, int posY )
    {
        if ( posX >= 0 && posX < GridSizeX && posY >= 0 && posY < GridSizeY )
        {
            return NodeArray [ posX, posY ];
        }
        return null;
    }

    public Node GetNode ( Node.NodeType nodeType )
    {
        return Nodes.First ( n => n.Type == nodeType );
    }

    public Node GetRandomNode ( Node.NodeType nodeType )
    {
        List<Node> filteredNodes = Nodes.FindAll ( n => n.Type == nodeType );
        return filteredNodes [ UnityEngine.Random.Range ( 0, filteredNodes.Count ) ];
    }

    public Node NodeFromWorldPoint ( Vector3 worldPosition )
    {
        int xIndex = Mathf.RoundToInt ( ( worldPosition.x - gridData.Offset.X ) / gridData.Spacing.X );
        int yIndex = Mathf.RoundToInt ( ( worldPosition.y - gridData.Offset.Y ) / gridData.Spacing.Y );
        return NodeArray [ xIndex, yIndex ];
    }

}
