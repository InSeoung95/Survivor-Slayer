using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGrid : MonoBehaviour
{
   public LayerMask WallMask;
   public Vector2 gridWorldSize;
   public float nodeRadius;
   public float Distance;

   private Node[,] grid;
   public List<Node> FinalPath;

   private float nodeDiameter;
   private int gridSizeX, gridSizeY;

   private void Awake()
   {
      nodeDiameter = nodeRadius * 2;
      gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
      gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
      CreateGrid();
   }

   public int MaxSize
   {
      get
      {
         return gridSizeX * gridSizeY;
      }
   }

   private void CreateGrid()
   {
      grid = new Node[gridSizeX, gridSizeY];
      Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 -
                           Vector3.forward * gridWorldSize.y / 2;
      Vector3 worldPoint;
      for (int y = 0; y < gridSizeY; y++)
      {
         for (int x = 0; x < gridSizeX; x++)
         {
            worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) +
                                 Vector3.forward * (y * nodeDiameter + nodeRadius);
            bool Wall = !Physics.CheckSphere(worldPoint, nodeRadius, WallMask);

            grid[x, y] = new Node(Wall, worldPoint, x, y);
         }
      }
   }

   public Node NodeFromWorldPosition(Vector3 a_WorldPosition)
   {
      float xPoint = (a_WorldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
      float yPoint = (a_WorldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
      xPoint = Mathf.Clamp01(xPoint);
      yPoint = Mathf.Clamp01(yPoint);

      int x = Mathf.RoundToInt((gridSizeX - 1) * xPoint);
      int y = Mathf.RoundToInt((gridSizeY - 1) * yPoint);
      return grid[x, y];
      
   }

   public List<Node> GetNeighboringNodes(Node a_Node)
   {
      List<Node> NeighboringNodes = new List<Node>();

      for (int x = -1; x <= 1; x++)
      {
         for (int y = -1; y <=1; y++)
         {
            if (x == 0 && y == 0) continue;     // 자기자신이면 스킵

            int checkX = a_Node.gridX + x;
            int checkY = a_Node.gridY + y;
            
            if(checkX>=0 && checkX < gridSizeX && checkY>=0 && checkY <gridSizeY)
               NeighboringNodes.Add(grid[checkX,checkY]);
         }
      }

      return NeighboringNodes;
   }


   private void OnDrawGizmos()
   {
      Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1 , gridWorldSize.y));

      if (grid != null)
      {
         foreach (Node node in grid)
         {
            Gizmos.color = (node.IsWall) ? Color.white : Color.red;
            if(FinalPath != null)
               if (FinalPath.Contains(node))
                  Gizmos.color = Color.black;
            
            Gizmos.DrawCube(node.Position, Vector3.one * (nodeDiameter - Distance));
         }
      }
   }
}
