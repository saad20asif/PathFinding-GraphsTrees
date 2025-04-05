using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

// Definition for a binary tree node.
public class TreeNode
{
    public int val;
    public TreeNode left;
    public TreeNode right;
    public TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
    {
        this.val = val;
        this.left = left;
        this.right = right;
    }
}

public class Solution
{
    public IList<IList<int>> LevelOrder(TreeNode root)
    {
        IList<IList<int>> result = new List<IList<int>>();
        if (root == null)
        {
            return result;
        }

        Queue<TreeNode> queue = new Queue<TreeNode>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            int levelSize = queue.Count;
            List<int> currentLevel = new List<int>();

            for (int i = 0; i < levelSize; i++)
            {
                TreeNode node = queue.Dequeue();
                currentLevel.Add(node.val);

                if (node.left != null)
                {
                    queue.Enqueue(node.left);
                }
                if (node.right != null)
                {
                    queue.Enqueue(node.right);
                }
            }

            result.Add(currentLevel);
        }

        return result;
    }
    public bool IsSameTree(TreeNode p, TreeNode q)
        {
            Queue<TreeNode> queueP = new Queue<TreeNode>();
            Queue<TreeNode> queueQ = new Queue<TreeNode>();

            queueP.Enqueue(p);
            queueQ.Enqueue(q);

            while (queueP.Count > 0 && queueQ.Count > 0)
            {
                TreeNode nodeP = queueP.Dequeue();
                TreeNode nodeQ = queueQ.Dequeue();

                if (nodeP == null && nodeQ == null) continue;
                if (nodeP == null || nodeQ == null) return false;
                if (nodeP.val != nodeQ.val) return false;

                // Enqueue both children, even if they are null.
                queueP.Enqueue(nodeP.left);
                queueP.Enqueue(nodeP.right);
                queueQ.Enqueue(nodeQ.left);
                queueQ.Enqueue(nodeQ.right);
            }

            return queueP.Count == 0 && queueQ.Count == 0;
        }
    }



// This MonoBehaviour script can be attached to a GameObject in your Unity scene.
public class LeetCode : SerializedMonoBehaviour
{
    [Button]
    void Start()
    {
    // Create sample trees:
    // Tree :    1
    //          /   \
    //         2     2
    //       / \    / \
    //          3       3    Input: root = [1,2,2,null,3,null,3]
    //                       Output: false

    // Create sample trees:
    // Tree :    1
    //          /   \
    //         2     2
    //       / \    / \
    //      3   4  4   3     Input: root = [1,2,2,3,4,4,3]
    //                       Output: true



        TreeNode p = new TreeNode(1, null, new TreeNode(2));

        // Tree q:    1
        //          /   \
        //         2     3
        TreeNode q = new TreeNode(1, null, new TreeNode(2));

        Solution sol = new Solution();
        bool result = sol.IsSameTree(p, q);
        Debug.Log("IsSameTree: " + result);

        // Optionally, print out the BFS order for both trees.
        //List<TreeNode> pList = sol.BFS(p);
        //List<TreeNode> qList = sol.BFS(q);
        //Debug.Log("Tree p BFS:");
        //sol.PrintTree(pList);
        //Debug.Log("Tree q BFS:");
        //sol.PrintTree(qList);


    }
    [SerializeField]Dictionary<int, int> numToIndex = new Dictionary<int, int>();
    [Button]
    public int[] TwoSum(int[] nums, int target)
    {
        numToIndex.Clear();
        for(int i=0;i<nums.Length;i++)
        {
            int complement = target- nums[i];
            if (numToIndex.ContainsKey(complement))
            {
                return new int[] { numToIndex[complement],i };
            }
            numToIndex.Add(nums[i], i);
        }
        throw new ArgumentException("No solution found.");
    }
    [Button]
    public IList<IList<int>> ThreeSum(int[] nums)
    {
        IList<IList<int>> result = new List<IList<int>>();
        Array.Sort(nums); // Sort to handle duplicates and two-pointer

        for (int i = 0; i < nums.Length - 2; i++)
        {
            // Skip duplicates for the first number
            if (i > 0 && nums[i] == nums[i - 1]) continue;

            int left = i + 1;
            int right = nums.Length - 1;
            int target = -nums[i]; // Because nums[i] + nums[left] + nums[right] = 0

            while (left < right)
            {
                int sum = nums[left] + nums[right];
                if (sum == target)
                {
                    result.Add(new List<int> { nums[i], nums[left], nums[right] });
                    // Skip duplicates for left and right
                    while (left < right && nums[left] == nums[left + 1]) left++;
                    while (left < right && nums[right] == nums[right - 1]) right--;
                    left++;
                    right--;
                }
                else if (sum < target)
                {
                    left++;
                }
                else
                {
                    right--;
                }
            }
        }
        return result;
    }

}
