using System;
using System.Collections.Generic;

namespace HeroLib
{
    /// <summary>
    /// Provides extensions to arrays
    /// </summary>
    public static partial class ArrayExt
    {
        public static void Init<T>(this List<T> list)
        {
            if (list == null) list = new List<T>();
            else list.Clear();
        }
        
        /// <summary>
        /// Array extension for finding an item in an array
        /// </summary>
        /// <typeparam name="T">Type of object being test for</typeparam>
        /// <param name="rArray">Array to test</param>
        /// <param name="rValue">Item to search for</param>
        /// <returns></returns>
        public static bool Contains<T>(this T[] rArray, T rValue)
        {
            return Array.Exists<T>(rArray, item => item.Equals(rValue));
        }
        
        /// <summary>
        /// Adds an element to a list but checking for duplicated.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="element"></param>
        /// <returns>True only if the item was added. False if duplicate was found.</returns>
        public static bool AddOnce<T>(this List<T> list, T element)
        {
            if (element == null) return false;
            if (list.Contains(element)) return false;
            list.Add(element);
            return true;
        }

        /// <summary>
        /// CAUSES GC. Use non-ref version.
        /// Updates the array by removing the specified index
        /// </summary>
        /// <typeparam name="T">Type of array to modify</typeparam>
        /// <param name="rSource">Source that is being modified</param>
        /// <param name="rIndex">Index ot remove</param>
        /// <returns>New array with the index removed</returns>
        public static void RemoveAt<T>(ref T[] rSource, int rIndex)
        {
            if (rSource.Length == 0)
            {
                return;
            }

            if (rIndex < 0 || rIndex >= rSource.Length)
            {
                return;
            }

            T[] lNewArray = new T[rSource.Length - 1];

            if (rIndex > 0)
            {
                Array.Copy(rSource, 0, lNewArray, 0, rIndex);
            }

            if (rIndex < rSource.Length - 1)
            {
                Array.Copy(rSource, rIndex + 1, lNewArray, rIndex, rSource.Length - rIndex - 1);
            }

            rSource = lNewArray;
        }

        /// <summary>
        /// Updates the array by removing the specified index
        /// </summary>
        /// <typeparam name="T">Type of array to modify</typeparam>
        /// <param name="rSource">Source that is being modified</param>
        /// <param name="rIndex">Index ot remove</param>
        /// <returns>New array with the index removed</returns>
        public static T[] RemoveAt<T>(T[] rSource, int rIndex)
        {
            int lLength = rSource.Length;

            if (lLength == 0)
            {
                return null;
            }

            if (rIndex < 0 || rIndex >= lLength)
            {
                return null;
            }

            int i = 0;
            int j = 0;
            T[] lNewArray = new T[lLength - 1];

            while (i < lLength)
            {
                if (i != rIndex)
                {
                    lNewArray[j] = rSource[i];
                    j++;
                }

                i++;
            }

            return lNewArray;
        }

        /// <summary>
        /// Provides an abstracted way to sort. We do it this way so we can change the method
        /// (ie using Linq) if we need to.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rSource"></param>
        /// <param name="rComparison"></param>
        public static void Sort<T>(this T[] rSource, Comparison<T> rComparison)
        {
            if (rSource.Length <= 1)
            {
                return;
            }

            Array.Sort(rSource, rComparison);
        }

        public static void Shuffle<T>(ref T[] array)
        {
            // Knuth shuffle algorithm :: courtesy of Wikipedia :)
            for (int t = 0; t < array.Length; t++)
            {
                T tmp = array[t];
                int r = UnityEngine.Random.Range(t, array.Length);
                array[t] = array[r];
                array[r] = tmp;
            }
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            //Añade funcion Shuffle() (ordenar "aleatoriamente") a los metodos de las listas
            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        
        public static T GetRandom<T>(this List<T> list)
        {
            if (list == null || list.Count == 0) return default(T);
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static T GetRandom<T>(this T[] list)
        {
            return list[UnityEngine.Random.Range(0, list.Length)];
        }

        #region NON_GENERIC

        #region STRING_ARRAYS

        public static bool HasArg(this string[] args, string desiredArg)
        {
            return args.Contains(desiredArg);
        }

        public static bool HasArg(this string[] args, int desiredArg)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (desiredArg == (int.Parse(args[i])))
                    return true;
            }

            return false;
        }

        public static bool HasArg(this string[] args, float desiredArg)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (desiredArg == (float.Parse(args[i])))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// ret is filled with the string found in that index
        /// if the array is too small returns false else true
        /// 0 = args[0]
        /// </summary>
        /// <param name="ret">the arg returned if there are enough elements</param>
        /// <param name="index">index inside the array that you want to get</param>
        public static bool GetArg(this string[] args, ref string ret, int index = 0)
        {
            if (args.Length > index)
            {
                ret = args[index];
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// ret is filled with the string found in that index
        /// if the array is too small returns false else true
        /// 0 = args[0]
        /// </summary>
        /// <param name="ret">the arg returned if there are enough elements</param>
        /// <param name="index">index inside the array that you want to get</param>
        public static bool GetArg(this string[] args, ref float ret, int index = 0)
        {
            if (args.Length > index)
            {
                ret = float.Parse(args[index]);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// ret is filled with the string found in that index
        /// if the array is too small returns false else true
        /// 0 = args[0]
        /// </summary>
        /// <param name="ret">the arg returned if there are enough elements</param>
        /// <param name="index">index inside the array that you want to get</param>
        public static bool GetArg(this string[] args, ref int ret, int index = 0)
        {
            if (args.Length > index)
            {
                ret = int.Parse(args[index]);
                return true;
            }
            else
                return false;
        }

        public static bool GetArg(this string[] args, ref bool ret, int index = 0)
        {
            if (args.Length > index)
            {
                ret = int.Parse(args[index]) > 0;
                return true;
            }
            else
                return false;
        }

        #endregion

        #endregion
    }
}