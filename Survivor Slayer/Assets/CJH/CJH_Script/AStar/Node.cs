using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
   public int gridX;
   public int gridY;

   public bool IsWall;
   public Vector3 Position;

   public Node Parent;

   public int gCost;
   public int hCost;
   private int heapIndex;
   
   public int FCost
   {
      get { return gCost + hCost; }
   }

   public Node(bool a_IsWall, Vector3 a_Pos, int a_gridX, int a_gridY)
   {
      IsWall = a_IsWall;
      Position = a_Pos;
      gridX = a_gridX;
      gridY = a_gridY;
   }

   public int HeapIndex
   {
      get { return heapIndex; }
      set { heapIndex = value; }
   }

   public int CompareTo(Node nodeToCompare)
   {
      int compare = FCost.CompareTo(nodeToCompare.FCost);
      if (compare == 0)
      {
         compare = hCost.CompareTo(nodeToCompare.hCost);
      }

      return -compare;
   }
}
