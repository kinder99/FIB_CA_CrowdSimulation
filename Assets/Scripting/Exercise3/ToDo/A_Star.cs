using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PathFinding{

	public class A_Star<TNode,TConnection,TNodeConnection,TGraph,THeuristic> : PathFinder<TNode,TConnection,TNodeConnection,TGraph,THeuristic>
	where TNode : Node
	where TConnection : Connection<TNode>
	where TNodeConnection : NodeConnections<TNode,TConnection>
	where TGraph : FiniteGraph<TNode,TConnection,TNodeConnection>
	where THeuristic : Heuristic<TNode>
	{
	// Class that implements the A* pathfinding algorithm
	// You have to implement the findpath function.
	// You can add whatever you need.
				
		protected Dictionary<TNode, NodeRecord> visitedNodes; // list of visited nodes 
		
		protected NodeRecord currentBest; // current best node found
		
		protected enum NodeRecordCategory{ OPEN, CLOSED, UNVISITED };

		protected SortedList<NodeRecord, NodeRecord> openNodes;
				
		protected class NodeRecord{	
		// You can use (or not) this structure to keep track of the information that we need for each node
			
			public NodeRecord(){}
			
			public TNode node; 
			public NodeRecord connection;	// connection traversed to reach this node 
			public float costSoFar; // cost accumulated to reach this node
			public float estimatedTotalCost; // estimated total cost to reach the goal from this node
			public NodeRecordCategory category; // category of the node: open, closed or unvisited
			public int depth; // depth in the search graph
		};

        protected class NodeRecordComparer : Comparer<NodeRecord>
        {
            public override int Compare(NodeRecord x, NodeRecord y)
            {
                if (ReferenceEquals(x, y)) return 0;

                float costX = 0f, costY = 0f;

                // take density into account too
                if (x != null) costX = x.costSoFar + x.estimatedTotalCost + 1000f;
                if (y != null) costY = y.costSoFar + y.estimatedTotalCost + 1000f;
                if (costX.CompareTo(costY) == 0)
                {
                    return 1;
                }

                return costX.CompareTo(costY);
            }
        };

        public A_Star(int maxNodes, float maxTime, int maxDepth):base(){ 
			
			visitedNodes = new Dictionary<TNode, NodeRecord> ();
			
		}

		public virtual List<TNode> GetVisitedNodes(){
			return visitedNodes.Keys.ToList();
		}
		
		public override List<TNode> findpath(TGraph graph, TNode start, TNode end, THeuristic heuristic, ref int found)
		{
			List<TNode> path = new List<TNode>();
			
			openNodes = new SortedList<NodeRecord, NodeRecord>(new NodeRecordComparer());
			List<NodeRecord> closed = new List<NodeRecord>();

            NodeRecord startRecord = new NodeRecord();
            startRecord.node = start;
            startRecord.connection = null;
            startRecord.costSoFar = 0f;
            startRecord.estimatedTotalCost = heuristic.estimateCost(start);
            startRecord.category = NodeRecordCategory.OPEN;
            startRecord.depth = 0;

            openNodes.Add(startRecord, startRecord);
            visitedNodes[start] = startRecord;

			while (openNodes.ElementAt(0).Value.node != null) {
				NodeRecord cur = openNodes.ElementAt(0).Value;
				openNodes.RemoveAt(0);
				closed.Add(cur);

				for(int i = 0; i < graph.connections[cur.node.id].Count(); i++)
				{
					TConnection conn = graph.connections[cur.node.id].connections[i];

					float cost = cur.costSoFar + conn.getCost();

					bool visitedNeighbor
				}
			}

            return path;
		}

	};

}