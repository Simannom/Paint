using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace algocs_paint
{
    public class Tree2D
    {
        // root of 2D-tree
        private Node2D m_root;

        // count of nodes
        private int m_count;


        //Creates a 2D-tree with two dimensions.

        public Tree2D()
        {
            m_root = null;
        }


        /** 
         * Insert a node in a 2D-tree.  Uses algorithm translated from 352.ins.c of
         *
         *   <PRE>
         *   &#064;Book{GonnetBaezaYates1991,                                   
         *     author =    {G.H. Gonnet and R. Baeza-Yates},
         *     title =     {Handbook of Algorithms and Data Structures},
         *     publisher = {Addison-Wesley},
         *     year =      {1991}
         *   }
         *   </PRE>
         *
         * @param key key for 2D-tree node
         * @param value value at that key
         *
         * @throws KeySizeException if key.length mismatches 2
         * @throws KeyDuplicateException if key already in tree
         */
        public void insert(PointF key, int value)
        {
            //пишем в файл последовательность вершин, через которые проходим при добавлении точек
            string path = @"..\tmp\Sequence.txt";
            try
            {
                m_root = Node2D.ins(key, value, m_root, 0);
                File.AppendAllText(path, " \n");
            }

            catch (KeyDuplicateException e)
            {
                throw e;
            }

            m_count++;
        }

        /** 
         * Find  2D-tree node whose key is identical to key.  Uses algorithm 
         * translated from 352.srch.c of Gonnet & Baeza-Yates.
         *
         * @param key key for 2D-tree node
         *
         * @return object at key, or null if not found
         */
        public int search(PointF key)
        {
            Node2D kd = Node2D.srch(key, m_root);

            return (kd == null ? -1 : kd.v);
        }



        /** 
         * Delete a node from a 2D-tree.  Instead of actually deleting node and
         * rebuilding tree, marks node as deleted.  Hence, it is up to the caller
         * to rebuild the tree as needed for efficiency.
         *
         * @param key key for 2D-tree node
         *
         * @throws KeySizeException if key.length mismatches 2
         * @throws KeyMissingException if no node in tree has key
         */
        public void delete(PointF key)
        {
            bool deleted = false;
            m_root = Node2D.delete(key, m_root, 0, ref deleted);
            if (deleted == false)
            {
                throw new KeyMissingException();
            }
            m_count--;
        }

        /** 
         * Range search in a 2D-tree.  Uses algorithm translated from
         * 352.range.c of Gonnet & Baeza-Yates.
         *
         * @param lowk lower-bounds for key
         * @param uppk upper-bounds for key
         *
         * @return array of Objects whose keys fall in range [lowk,uppk]
         */

        public int[] range(PointF lowk, PointF uppk)
        {
            List<Node2D> v = new List<Node2D>();
            Node2D.rsearch(lowk, uppk, m_root, 0, v);
            int[] o = new int[v.Count];
            for (int i = 0; i < v.Count; ++i)
            {
                Node2D n = (Node2D)v[i];
                o[i] = n.v;
            }
            return o;

        }

        public String toString()
        {
            if (m_root == null) return "";
            return m_root.toString(0);
        }

        //public 



        /// <summary>
        /// 2-D Tree node class
        /// </summary>
        public class Node2D
        {
            // these are seen by Tree2D
            protected PointF k;
            public int v;
            protected Node2D left, right;
            public bool deleted;

            // Method ins translated from 352.ins.c of Gonnet & Baeza-Yates
            public static Node2D ins(PointF key, int val, Node2D t, int lev)
            {
                string path = @"..\tmp\Sequence.txt";
                File.AppendAllText(path, " " + t.v);

                if (t == null) {
                    t = new Node2D(key, val);
                }
                else if (key.Equals(t.k))
                {
                    // "re-insert"
                    if (t.deleted)
                    {
                        t.deleted = false;
                        t.v = val;
                    }
                    else
                    {
                        throw (new KeyDuplicateException());
                    }
                }

                else
                {
                    //lev для выбора по какой координате разбиваем пространство
                    bool lr = lev == 0 ? key.X > t.k.X : key.Y > t.k.Y;
                    if (lr)
                    {
                        t.right = ins(key, val, t.right, (lev + 1) % 2);
                    }
                    else
                    {
                        t.left = ins(key, val, t.left, (lev + 1) % 2);
                    }
                }
                return t;
            }


            // Method srch translated from 352.srch.c of Gonnet & Baeza-Yates
            public static Node2D srch(PointF key, Node2D t)
            {

                for (int lev = 0; t != null; lev = (lev + 1) % 2)
                {

                    if (!t.deleted && key.Equals(t.k))
                    {
                        return t;
                    }
                    else
                    {
                        bool lr = lev == 0 ? key.X > t.k.X : key.Y > t.k.Y;
                        if (lr)
                        {
                            t = t.right;
                        }
                        else
                        {
                            t = t.left;
                        }
                    }
                }

                return null;
            }

            public static Node2D delete(PointF key, Node2D t, int lev, ref bool deleted)
            {
                if (t == null) return null;
                if (!t.deleted && key.Equals(t.k))
                {
                    t.deleted = true;
                    deleted = true;
                }
                else
                {
                    bool lr = lev == 0 ? key.X > t.k.X : key.Y > t.k.Y;
                    if (lr)
                    {
                        t.right = delete(key, t.right, (lev + 1) % 2, ref deleted);
                    }
                    else
                    {
                        t.left = delete(key, t.left, (lev + 1) % 2, ref deleted);
                    }
                }

                if (!t.deleted || t.left != null || t.right != null)
                {
                    return t;
                }
                else
                {
                    return null;
                }
            }

            // Method rsearch translated from 352.range.c of Gonnet & Baeza-Yates
            public static void rsearch(PointF lowk, PointF uppk, Node2D t, int lev,
                     List<Node2D> v)
            {

                if (t == null) return;
                bool lr = lev%2 == 0 ? lowk.X <= t.k.X : lowk.Y <= t.k.Y;
                if (lr)
                {
                    rsearch(lowk, uppk, t.left, (lev + 1) % 2, v);
                }
                
                if (!t.deleted && (lowk.X <= t.k.X && uppk.X >= t.k.X) && (lowk.Y <= t.k.Y && uppk.Y >= t.k.Y))
                    v.Add(t);
                lr = lev == 0 ? uppk.X > t.k.X : uppk.Y > t.k.Y;
                if (lr)
                {
                    rsearch(lowk, uppk, t.right, (lev + 1) % 2, v);
                }
            }


            // constructor is used only by class; other methods are static
            private Node2D(PointF key, int val)
            {

                k = key;
                v = val;
                left = null;
                right = null;
                deleted = false;
            }


            public String toString(int depth)
            {
                String s = k + "  " + v + (deleted ? "*" : "");
                if (left != null)
                {
                    s = s + "\n" + pad(depth) + "L " + left.toString(depth + 1);
                }
                if (right != null)
                {
                    s = s + "\n" + pad(depth) + "R " + right.toString(depth + 1);
                }
                return s;
            }

            private static String pad(int n)
            {
                String s = "";
                for (int i = 0; i < n; ++i)
                {
                    s += " ";
                }
                return s;
            }
        }
    }
}