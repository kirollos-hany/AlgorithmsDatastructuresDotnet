using System;
using System.Collections.Generic;
using System.Linq;

namespace Kiro.Algorithms.Graph
{
    public class WeightedDirectedAcyclicGraph<TNode> 
    {
        private readonly Dictionary<TNode, Dictionary<TNode, int>> _graph = new Dictionary<TNode, Dictionary<TNode, int>>();

        public void AddNode(TNode node, Dictionary<TNode, int> neighbours)
        {
            _graph.TryAdd(node, neighbours);
        }

        public void RemoveNode(TNode node)
        {
            if (_graph.TryGetValue(node, out var value))
            {
                _graph.Remove(node);
            }
        }

        public (int cost, IReadOnlyList<TNode> cheapestPath) DijkstraAlgorithm(TNode startNode, TNode finishNode)
        {
            var costs = InitializeCosts(startNode);
            var parentsTable = InitializeParentTable(startNode);
            var visitedNodes = new HashSet<TNode>();
            var node = FindLowestCostNode(costs, visitedNodes);
            var cheapestPath = new List<TNode>();
            while (node != null &&  !node.Equals(default))
            {
                cheapestPath.Add(node);
                var cost = costs[node];
                var neighbours = _graph[node];
                foreach (var neighbour in neighbours.Keys)
                {
                    var newCost = cost + neighbours[neighbour];
                    if (costs[neighbour] <= newCost) continue;
                    costs[neighbour] = newCost;
                    parentsTable[neighbour] = node;
                    cheapestPath.Add(neighbour);
                }

                visitedNodes.Add(node);
                node = FindLowestCostNode(costs, visitedNodes);
            }

            return (costs[finishNode], cheapestPath);
        }
        private Dictionary<TNode, int> InitializeCosts(TNode startNode)
        {
            var startNeighbours = _graph[startNode];
            var remainingNodes = _graph.Keys.Where(k => !k.Equals(startNode) && !startNeighbours.ContainsKey(k)).ToArray();
            var costs = startNeighbours.ToDictionary(neighbour => neighbour.Key, neighbour => neighbour.Value);
            
            foreach (var node in remainingNodes)
            {
                costs.Add(node, int.MaxValue);
            }

            return costs;
        }

        private Dictionary<TNode, TNode?> InitializeParentTable(TNode startNode)
        {
            var startNeighbours = _graph[startNode];
            var table = startNeighbours.ToDictionary<KeyValuePair<TNode, int>, TNode, TNode?>(neighbour => neighbour.Key, _ => startNode);

            var remainingNodes = _graph.Keys.Where(k => k != null && !k.Equals(startNode) && !startNeighbours.ContainsKey(k)).ToArray();
            foreach (var node in remainingNodes)
            {
                table.Add(node, default);
            }

            return table;
        }

        private static TNode? FindLowestCostNode(Dictionary<TNode, int> costs, HashSet<TNode> visitedNodes)
        {
            return costs.FirstOrDefault(c => !visitedNodes.Contains(c.Key) && c.Value == costs.Select(keyValuePair => keyValuePair.Value).Min()).Key ?? default;
        }
    }
}