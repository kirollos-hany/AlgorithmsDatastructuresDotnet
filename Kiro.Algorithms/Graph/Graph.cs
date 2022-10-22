using System;
using System.Collections.Generic;

namespace Kiro.Algorithms.Graph
{
    public class Graph<T> where T : class
    {
        private readonly IDictionary<T, T[]> _graph;

        public Graph()
        {
            _graph = new Dictionary<T, T[]>();
        }

        public void Add(T node, T[] connections)
        {
            _graph.Add(node, connections);
        }

        public void Remove(T node)
        {
            _graph.Remove(node);
        }

        public T? Bfs(T startNode, Func<T, bool> predicate)
        {
            var visitedNodes = new HashSet<T>();
            var searchQueue = new Queue<T>();
            EnqueueConnections(_graph[startNode], searchQueue);
            while (searchQueue.Count != 0)
            {
                var node = searchQueue.Dequeue();
                if (visitedNodes.Contains(node))
                {
                    continue;
                }
                
                if (predicate(node))
                {
                    return node;
                }
                visitedNodes.Add(node);

                if (_graph.TryGetValue(node, out var connections))
                {
                    EnqueueConnections(connections, searchQueue);
                }
            }

            return default;
        }

        private static void EnqueueConnections(IEnumerable<T> connections, Queue<T> queue)
        {
            foreach (var connection in connections)
            {
                queue.Enqueue(connection);
            }
        }
    }
}