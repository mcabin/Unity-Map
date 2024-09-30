using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Script
{
    public class BinaryHeap
    {

        private TileNode[] data;
        private int[] positions;
        int count;

        /// <summary>
        /// Creates a new, empty priority queue with the specified capacity.
        /// </summary>
        /// <param name="capacity">The maximum number of nodes that will be stored in the queue.</param>
        public BinaryHeap(int capacity)
        {
            data = new TileNode[capacity];
            positions = new int[capacity];
            count = 0;
        }

        /// <summary>
        /// Adds an item to the queue.  Is position is determined by its priority relative to the other items in the queue.
        /// aka HeapInsert
        /// </summary>
        /// <param name="item">Item to add</param>
        public void Add(TileNode item)
        {
            if (count == data.Length)
                throw new Exception("Heap capacity exceeded");

            // Add the item to the heap in the end position of the array (i.e. as a leaf of the tree)
            int position = count++;
            data[position] = item;
            item.positionInHeap=position; 
            // Move it upward into position, if necessary
            MoveUp(position);

        }

        /// <summary>
        /// Extracts the item in the queue with the minimal priority value.
        /// </summary>
        /// <returns></returns>
        public TileNode ExtractMin() // Probably THE most important function... Got everything working
        {
            TileNode minNode = data[0];
            Swap(0, count - 1);
            count--;
            MoveDown(0);
            return minNode;
        }



        /// <summary>
        /// Moves the node at the specified position upward, it it violates the Heap Property.
        /// This is the while loop from the HeapInsert procedure in the slides.
        /// </summary>
        /// <param name="position"></param>
        void MoveUp(int position)
        {
            while ((position > 0) && (data[Parent(position)].CompareTo(data[position]) > 0))
            {
                int original_parent_pos = Parent(position);
                Swap(position, original_parent_pos);
                position = original_parent_pos;
            }
        }

        /// <summary>
        /// Moves the node at the specified position down, if it violates the Heap Property
        /// aka Heapify
        /// </summary>
        /// <param name="position"></param>
        void MoveDown(int position)
        {
            int lchild = LeftChild(position);
            int rchild = RightChild(position);
            int largest;
            
            if ((lchild < count) && (data[lchild].CompareTo(data[position]) < 0))
            {
                largest = lchild;
            }
            else
            {
                largest = position;
            }
            if ((rchild < count) && (data[rchild].CompareTo(data[largest]) < 0))
            {
                largest = rchild;
            }
            if (largest != position)
            {
                Swap(position, largest);
                MoveDown(largest);
            }
        }

        /// <summary>
        /// Number of items waiting in queue
        /// </summary>
        public int Count
        {
            get
            {
                return count;
            }
        }

        /// <summary>
        /// Swaps the nodes at the respective positions in the heap
        /// Updates the nodes' QueuePosition properties accordingly.
        /// </summary>
        void Swap(int position1, int position2)
        {
            TileNode temp = data[position1];
            data[position1] = data[position2];
            data[position2] = temp;
            data[position1].positionInHeap= position1;
            data[position2].positionInHeap= position2;
        }

        public void DecreasePriority(TileNode item)
        {
            int position = item.positionInHeap;
            if (position != -1)
            {
                MoveUp(position);
            }
        }



        /// <summary>
        /// Gives the position of a node's parent, the node's position in the queue.
        /// </summary>
        static int Parent(int position)
        {
            return (position - 1) / 2;
        }

        /// <summary>
        /// Returns the position of a node's left child, given the node's position.
        /// </summary>
        static int LeftChild(int position)
        {
            return 2 * position + 1;
        }

        /// <summary>
        /// Returns the position of a node's right child, given the node's position.
        /// </summary>
        static int RightChild(int position)
        {
            return 2 * position + 2;
        }

        /// <summary>
        /// Checks all entries in the heap to see if they satisfy the heap property.
        /// </summary>
        public void TestHeapValidity()
        {
            for (int i = 1; i < count; i++)
                if (data[Parent(i)].CompareTo(data[i]) > 0)
                    throw new Exception("Heap violates the Heap Property at position " + i);
        }
    }
}