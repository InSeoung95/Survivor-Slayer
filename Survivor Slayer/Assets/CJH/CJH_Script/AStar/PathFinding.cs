using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
   private PathRequestManager _requestManager;
   private PathGrid _grid;

   private void Awake()
   {
      _requestManager = GetComponent<PathRequestManager>();
      _grid = GetComponent<PathGrid>();
   }

   public void StartFindPath(Vector3 startPos, Vector3 targetPos)
   {
      StartCoroutine(FindPath(startPos, targetPos));
   }

   IEnumerator FindPath(Vector3 a_StartPos, Vector3 a_TargetPos)
   {
      Vector3[] waypoints = new Vector3[0];
      bool pathSuccess = false;
      Node StartNode = _grid.NodeFromWorldPosition(a_StartPos);
      Node TargetNode = _grid.NodeFromWorldPosition(a_TargetPos);


      if (StartNode.IsWall && TargetNode.IsWall)
      {
         List<Node> OpenList = new List<Node>();
         HashSet<Node> ClosedList = new HashSet<Node>();
         OpenList.Add(StartNode);

         while (OpenList.Count > 0)
         {
            Node CurrentNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
               if (OpenList[i].FCost < CurrentNode.FCost ||
                   OpenList[i].FCost == CurrentNode.FCost && OpenList[i].hCost < CurrentNode.hCost)
               {
                  CurrentNode = OpenList[i];
               }
            }

            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            // 탐색노드가 목표면 탐색종료
            if (CurrentNode == TargetNode)
            {
               pathSuccess = true;
               break;
            }

            foreach (Node NeighborNode in _grid.GetNeighboringNodes(CurrentNode))
            {
               if (!NeighborNode.IsWall || ClosedList.Contains(NeighborNode))
               {
                  continue;
               }

               int MoveCost = CurrentNode.gCost + GetManhattenDistance(CurrentNode, NeighborNode);
               if (MoveCost < NeighborNode.gCost || !OpenList.Contains(NeighborNode))
               {
                  NeighborNode.gCost = MoveCost;
                  NeighborNode.hCost = GetManhattenDistance(NeighborNode, TargetNode);
                  NeighborNode.Parent = CurrentNode;

                  if (!OpenList.Contains(NeighborNode))
                  {
                     OpenList.Add(NeighborNode);
                  }
               }
            }

         }
      }

      yield return null;
      if (pathSuccess)
      {
         waypoints = RetracePath(StartNode, TargetNode);
         pathSuccess = waypoints.Length > 0;
      }
      _requestManager.FinishedProcessingPath(waypoints, pathSuccess);
   }

   // 탐색 종료후 최종노드의 parentNode를 추적할 리스트에 담기
   // 최종경로의 노드 WorldPosition을ㄹ 순차적으로 리턴
   Vector3[] RetracePath(Node a_StartingNode, Node a_EndNode)
   {
      List<Node> FinalPath = new List<Node>();
      Node CurrentNode = a_EndNode;

      while (CurrentNode != a_StartingNode)
      {
         FinalPath.Add(CurrentNode);
         CurrentNode = CurrentNode.Parent;
      }

      Vector3[] waypoints = SimplifyPath(FinalPath);
      Array.Reverse(waypoints);
      return waypoints;
   }

   Vector3[] SimplifyPath(List<Node> path)
   {
      List<Vector3> waypoints = new List<Vector3>();
      Vector2 directionOld = Vector2.zero;

      for (int i = 1; i < path.Count; i++)
      {
         Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
         if (directionNew != directionOld)
         {
            waypoints.Add(path[i].Position);
         }

         directionOld = directionNew;
      }

      return waypoints.ToArray();
   }

   void GetFinalPath(Node a_StartingNode, Node a_EndNode)
   {
      List<Node> FinalPath = new List<Node>();
      Node CurrentNode = a_EndNode;

      while (CurrentNode != a_StartingNode)
      {
         FinalPath.Add(CurrentNode);
         CurrentNode = CurrentNode.Parent;
      }
      FinalPath.Reverse();
      _grid.FinalPath = FinalPath;
   }

   int GetManhattenDistance(Node a_nodeA, Node a_nodeB)
   {
      int ix = Mathf.Abs(a_nodeA.gridX - a_nodeB.gridX);
      int iy = Mathf.Abs(a_nodeA.gridY - a_nodeB.gridY);

      if (ix > iy)
         return 14 * iy + 10 * (ix - iy);
      return 14 * ix + 10 * (iy - ix);
   }
}
