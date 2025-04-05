using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DogProblem : SerializedMonoBehaviour
{

    [Button]
    void TestCase1()
    {
        string[] grid = { "OOOO", "OOFF", "OCHO", "OFOO" };
        int result = ShortestPath(grid);
        Debug.Log("Test Case  : " + result);
    }
    [Button]
    void TestCase2()
    {
        string[] grid = { "FOOF", "OCOO", "OOOH", "FOOO" };
        int result = ShortestPath(grid);
        Debug.Log("Test Case  : " + result);
    }
    [Button]
    void TestCase3()
    {
        string[] grid = { "OOOO", "OCOO", "OOOH", "OOOO" };
        int result = ShortestPath(grid);
        Debug.Log("Test Case  : " + result);
    }
    public List<(int, int)> foods = new List<(int, int)>();
    public Dictionary<(int, int), int> foodIndices = new Dictionary<(int, int), int>();
    public int ShortestPath(string[] grid)
    {
        int rows = grid.Length;
        int cols = grid[0].Length;

        // Parse the grid to find C, H, and F positions
        (int, int) start = (-1, -1);
        (int, int) home = (-1, -1);
        foods.Clear();
        foodIndices.Clear();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                char c = grid[i][j];
                if (c == 'C')
                {
                    start = (i, j);
                }
                else if (c == 'H')
                {
                    home = (i, j);
                }
                else if (c == 'F')
                {
                    foods.Add((i, j));
                    foodIndices[(i, j)] = foods.Count - 1;
                }
            }
        }

        int targetMask = (1 << foods.Count) - 1;

        // If no food, directly find path from C to H
        if (foods.Count == 0)
        {
            return BFS(start, home, grid, true);
        }

        // BFS initialization
        bool[,,] visited = new bool[rows, cols, targetMask + 1];
        Queue<(int, int, int, int)> queue = new Queue<(int, int, int, int)>();
        queue.Enqueue((start.Item1, start.Item2, 0, 0));
        visited[start.Item1, start.Item2, 0] = true;

        while (queue.Count > 0)
        {
            var (r, c, mask, steps) = queue.Dequeue();

            // Check if reached home with all food collected
            if (r == home.Item1 && c == home.Item2)
            {
                if (mask == targetMask)
                {
                    return steps;
                }
                continue;
            }

            // Explore all four directions
            int[] dr = { -1, 1, 0, 0 };
            int[] dc = { 0, 0, -1, 1 };
            for (int d = 0; d < 4; d++)
            {
                int nr = r + dr[d];
                int nc = c + dc[d];

                if (nr < 0 || nr >= rows || nc < 0 || nc >= cols) continue;

                char cell = grid[nr][nc];
                int newMask = mask;

                // Handle H cell
                if (cell == 'H')
                {
                    if (mask == targetMask)
                    {
                        if (!visited[nr, nc, mask])
                        {
                            visited[nr, nc, mask] = true;
                            queue.Enqueue((nr, nc, mask, steps + 1));
                        }
                    }
                    continue;
                }

                // Handle F cell
                if (cell == 'F' && foodIndices.ContainsKey((nr, nc)))
                {
                    int idx = foodIndices[(nr, nc)];
                    if ((mask & (1 << idx)) == 0)
                    {
                        newMask = mask | (1 << idx);
                    }
                }

                // Check if visited
                if (!visited[nr, nc, newMask])
                {
                    visited[nr, nc, newMask] = true;
                    queue.Enqueue((nr, nc, newMask, steps + 1));
                }
            }
        }

        return -1; // No path found (as per problem statement, a solution exists)
    }

    // Helper BFS for direct path from start to end when no F's
    private int BFS((int, int) start, (int, int) end, string[] grid, bool allowH)
    {
        int rows = grid.Length;
        int cols = grid[0].Length;
        bool[,] visited = new bool[rows, cols];
        Queue<(int, int, int)> queue = new Queue<(int, int, int)>();
        queue.Enqueue((start.Item1, start.Item2, 0));
        visited[start.Item1, start.Item2] = true;

        int[] dr = { -1, 1, 0, 0 };
        int[] dc = { 0, 0, -1, 1 };

        while (queue.Count > 0)
        {
            var (r, c, steps) = queue.Dequeue();

            if (r == end.Item1 && c == end.Item2)
            {
                return steps;
            }

            for (int d = 0; d < 4; d++)
            {
                int nr = r + dr[d];
                int nc = c + dc[d];

                if (nr < 0 || nr >= rows || nc < 0 || nc >= cols) continue;
                char cell = grid[nr][nc];
                if (cell == 'H' && !allowH) continue;
                if (!visited[nr, nc])
                {
                    visited[nr, nc] = true;
                    queue.Enqueue((nr, nc, steps + 1));
                }
            }
        }

        return -1;
    }
}
