using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPI311.GameEngine
{
    public class AStarSearch
    {
        public int Cols { get; set; }
        public int Rows { get; set; }
        public AStarNode[,] Nodes { get; set; }
        public AStarNode Start { get; set; }
        public AStarNode End { get; set; }

        private SortedDictionary<float, List<AStarNode>> openList;

        public AStarSearch(int rows, int cols)
        {
            openList = new SortedDictionary<float, List<AStarNode>>();
            Rows = rows;
            Cols = cols;
            Nodes = new AStarNode[Rows, Cols];
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Cols; c++)
                    Nodes[r, c] = new AStarNode(c, r, new Vector3(c, 0, r));
        }

        public void Search()
        {
            #region Initialize grid
            openList.Clear();
            foreach(AStarNode node in Nodes)
            {
                node.Closed = false;
                node.Cost = Single.MaxValue;
                node.Parent = null;
                node.Heuristic = Vector3.Distance(node.Position, End.Position);
            }
            #endregion

            AddToOpenList(Start);
            while(openList.Count > 0) // openlist is not empty
            {
                AStarNode node = GetBestNode();
                if (node == End)
                    break;
                if(node.Row < Rows -1)
                    AddToOpenList(Nodes[node.Row + 1, node.Col], node);
                if(node.Row > 0)
                    AddToOpenList(Nodes[node.Row - 1, node.Col], node);
                if(node.Col < Cols - 1)
                    AddToOpenList(Nodes[node.Row, node.Col + 1], node);
                if(node.Col > 0)
                    AddToOpenList(Nodes[node.Row, node.Col - 1], node);
            }
        }

        private void AddToOpenList(AStarNode node, AStarNode parent = null)
        {
            if (!node.Passable || node.Closed) return;
            if (parent == null) node.Cost = 0;
            else
            {
                float cost = parent.Cost + 1;
                if (node.Cost > cost)
                {
                    RemoveFromOpenList(node);
                    node.Cost = cost;
                    node.Parent = parent;
                }
                else
                    return;
            }
            if (!openList.ContainsKey(node.Cost+node.Heuristic))
                openList[node.Cost+node.Heuristic] = new List<AStarNode>();
            openList[node.Cost+node.Heuristic].Add(node);
        }

        private AStarNode GetBestNode()
        {
            AStarNode node = openList.ElementAt(0).Value[0];
            openList.ElementAt(0).Value.Remove(node);
            if (openList.ElementAt(0).Value.Count == 0)
                openList.Remove(node.Cost+node.Heuristic);
            node.Closed = true;
            return node;
        }

        private void RemoveFromOpenList(AStarNode node)
        {
            if (openList.ContainsKey(node.Cost+node.Heuristic))
            {
                openList[node.Cost+node.Heuristic].Remove(node);
                if (openList[node.Cost+node.Heuristic].Count == 0)
                    openList.Remove(node.Cost+node.Heuristic);
            }
        }
    }
}
