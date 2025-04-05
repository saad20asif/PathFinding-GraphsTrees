using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class Vertex
{
    public string Name;
    public string Description;
    public Color Color = Color.black;

    public Vertex(string name, string des, Color col)
    {
        Name = name;
        Description = des;
        Color = col;    
    }
}
public class GraphTraversal : SerializedMonoBehaviour
{
    // Representing a simple undirected graph as an adjacency list.
    // Graph Structure:

    // Quetta -> Multan
    // Murree -> Multan
    // Multan -> Quetta, Murree, Lahore, Pindi 
    // Lahore -> Multan, Okara
    // Pindi -> Multan
    // Okara -> Lahore

    Vertex Quetta = new Vertex("Quetta", "Quetta is Awesome", Color.black);
    Vertex Murree = new Vertex("Murree", "Murree is Awesome", Color.green);
    Vertex Multan = new Vertex("Multan", "Multan is Awesome", Color.red);
    Vertex Lahore = new Vertex("Lahore","Lahore is Awesome", Color.grey);
    Vertex Pindi = new Vertex("Pindi", "Pindi is Awesome", Color.blue);
    Vertex Okara = new Vertex("Okara", "Okara is Awesome", Color.yellow);

    public Dictionary<Vertex, List<Vertex>> Graph = new Dictionary<Vertex, List<Vertex>>();

    [Button]
    void Start()
    {
        Graph.Clear();
        Graph.Add(Quetta, new List<Vertex> { Multan });
        Graph.Add(Murree, new List<Vertex> { Multan });
        Graph.Add(Multan, new List<Vertex> { Quetta, Murree,Lahore,Pindi });
        Graph.Add(Lahore, new List<Vertex> { Multan, Okara });
        Graph.Add(Pindi, new List<Vertex> { Multan });
        Graph.Add(Okara, new List<Vertex> { Lahore });

        //print("BFS ORDER : ");
        //BFS(Quetta);

        //print("\n \n \n");

        //print("DFS ORDER : ");
        //DFS(Quetta);

        //print("\n \n \n");

        //print("DFS RECURSIVE ORDER : ");
        //DFSRecursive(Quetta);
        List<Vertex> shortestPath = FindShortestPathUsingBFS(Multan, Okara);
        PrintVerticesList(shortestPath);
    }
    public void PrintVerticesList(List<Vertex> shortestPath)
    {
        for(int i=0;i<shortestPath.Count;i++)
        {
            print("->" + shortestPath[i].Name);
        }
    }

    public void BFS(Vertex start)
    {
        Queue<Vertex> queue = new Queue<Vertex>();
        HashSet<Vertex> visited = new HashSet<Vertex>();

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            Vertex current = queue.Dequeue();
            Debug.Log("->"+current.Name);
            if (Graph.ContainsKey(current))
            {
                List<Vertex> neighbors = Graph[current];
                for (int i = 0; i < neighbors.Count; i++)
                {
                    if (!visited.Contains(neighbors[i]))
                    {
                        queue.Enqueue(neighbors[i]);
                        visited.Add(neighbors[i]);
                    }
                        
                }
            }
        }
    }

    public void DFS(Vertex start)
    {
        Stack<Vertex> queue = new Stack<Vertex>();
        HashSet<Vertex> visited = new HashSet<Vertex>();

        queue.Push(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            Vertex current = queue.Pop();
            Debug.Log("->" + current.Name);
            if (Graph.ContainsKey(current))
            {
                List<Vertex> neighbors = Graph[current];
                for (int i = 0; i < neighbors.Count; i++)
                {
                    if (!visited.Contains(neighbors[i]))
                    {
                        queue.Push(neighbors[i]);
                        visited.Add(neighbors[i]);
                    }

                }
            }
        }
    }

    public void DFSRecursive(Vertex start)
    {
        HashSet<Vertex> visited = new HashSet<Vertex>();
        DFSHelper(start, visited);
    }

    private void DFSHelper(Vertex current, HashSet<Vertex> visited)
    {
        // If this vertex is already visited, exit.
        if (visited.Contains(current))
            return;

        // Mark the current vertex as visited and log it.
        visited.Add(current);
        Debug.Log("->" + current.Name);

        // If the current vertex has neighbors, iterate through them.
        if (Graph.ContainsKey(current))
        {
            foreach (Vertex neighbor in Graph[current])
            {
                DFSHelper(neighbor, visited);
            }
        }
    }


    [SerializeField]Dictionary<Vertex, Vertex> predecessor = new Dictionary<Vertex, Vertex>();
    // Finds the shortest path between start and target vertices using BFS.
    public List<Vertex> FindShortestPathUsingBFS(Vertex start, Vertex target)
    {
        // Dictionary to store the predecessor of each vertex.
        predecessor.Clear();

        Queue<Vertex> queue = new Queue<Vertex>();
        HashSet<Vertex> visited = new HashSet<Vertex>();

        queue.Enqueue(start);
        visited.Add(start);

        bool found = false;
        while (queue.Count > 0)
        {
            Vertex current = queue.Dequeue();

            if (current.Equals(target))
            {
                found = true;
                break;
            }

            if (Graph.ContainsKey(current))
            {
                foreach (Vertex neighbor in Graph[current])
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        predecessor[neighbor] = current;
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }

        if (!found)
        {
            // No path exists between start and target.
            return null;
        }

        // Reconstruct the path from target back to start.
        List<Vertex> path = new List<Vertex>();
        Vertex crawl = target;
        while (!crawl.Equals(start))
        {
            path.Add(crawl);
            crawl = predecessor[crawl];
        }
        path.Add(start);
        path.Reverse();

        return path;
    }

}
