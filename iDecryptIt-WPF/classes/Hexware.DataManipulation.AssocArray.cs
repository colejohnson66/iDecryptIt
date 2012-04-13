using System;

// TODO:
// Implement ".Delete"
// and
// throw an exception if array gets too big (2 billion keys seems extreme though)

namespace Hexware.DataManipulation
{
    /// <summary>
    /// Associative Array manipulation class
    /// </summary>
    /// <typeparam name="T">The type of array (string, byte, bool, etc.)</typeparam>
    public class AssocArray<T>
    {
        private object[,] data;
        
        /// <summary>
        /// Empty AssocArray Constructor
        /// </summary>
        public AssocArray()
        {
            data = new object[0, 2] { };
        }

        /// <summary>
        /// AssocArray Constructor from [x, 2] System.Object array
        /// </summary>
        /// 
        /// <param name="arr">A [x, 2] System.Object array</param>
        /// 
        /// <exception cref="System.ArgumentException">Bad Array</exception>
        /// <exception cref="System.ArgumentNullException">One of the values at [x, 0] is <c>null</c></exception>
        public AssocArray(object[,] arr)
        {
            if (arr == null)
            {
                throw new ArgumentNullException("The specified array is <c>null</c>", "arr");
            }

            // Right dimensions [x, 2]
            if (arr.GetLength(1) != 2)
            {
                throw new ArgumentException("The specified array's dimensions are not [x, 2]", "arr");
            }

            // Are any "keys" null?
            int length = arr.GetLength(0);
            for (int i = 0; i < length; i++)
            {
                if (arr[i, 0] == null)
                {
                    throw new ArgumentNullException("The value at index [" + i + ", 0] is null", "arr");
                }
            }

            // Check for duplicate keys
            string[] usedkeys = new string[length]; // Use length as a max
            for (int i = 0; i < length; i++)
            {
                // For each element in "data" (i), check if the key exists already (ii)
                for (int ii = 0; ii < length; i++)
                {
                    if (arr[i, 0] == usedkeys[ii])
                    {
                        throw new ArgumentException("The specified array contains duplicate keys", "arr");
                    }
                }
            }

            // Set data var
            data = arr;
        }

        /// <summary>
        /// Add a new index to the associative array
        /// </summary>
        /// <param name="key">The key of the new item</param>
        /// <param name="value">The value of the new item</param>
        /// <exception cref="System.ArgumentNullException">The specified key is <c>null</c></exception>
        /// <exception cref="System.OverflowException">The array is too big</exception>
        /// <exception cref="System.StackOverflowException">The stack is too big</exception>
        /// <returns>The associative array</returns>
        public AssocArray<T> Add(string key, T value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key", "The specified key is null");
            }

            // Does the specified key already exist?
            if (Exists(key))
            {
                string[] keys = GetKeys();
                int length = keys.Length;
                for (int i = 0; i < length; i++)
                {
                    if (keys[i] == key)
                    {
                        // Set value
                        data[i, 1] = value;
                        break;
                    }
                }
                return this;
            }
            else
            {
                try
                {
                    // Resize
                    int origlength = Length;
                    object[,] resize = new object[origlength + 1, 2];
                    for (int i = 0; i < origlength; i++)
                    {
                        resize[i, 0] = data[i, 0];
                        resize[i, 1] = data[i, 1];
                    }

                    data = resize;

                    // Add value
                    data[origlength, 0] = key;
                    data[origlength, 1] = value;

                    return this;
                }
                catch (OverflowException ex)
                {
                    throw new OverflowException("The array is too big", ex);
                }
                catch (StackOverflowException ex)
                {
                    throw new StackOverflowException("The stack is too small", ex);
                }
            }
        }

        /// <summary>
        /// Get a value of data from the associative array
        /// </summary>
        /// <param name="key">The key of the data to be returned from the associative array</param>
        /// <exception cref="System.MissingFieldException">The specified key was not found</exception>
        /// <exception cref="System.ArgumentNullException">The specified key is <c>null</c></exception>
        /// <returns>The data specified in the associative array from the specified key</returns>
        public T Get(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key", "The specified key is null");
            }

            int origlength = data.GetLength(0);
            for (int i = 0; i < origlength; i++)
            {
                if (data[i, 0].Equals(key))
                {
                    return (T)data[i, 1];
                }
            }
            
            throw new MissingFieldException("The specified key \"" + key.ToString() + "\" does not exist in the array", key);
        }

        /// <summary>
        /// Gets or sets a value of data from the associative array
        /// </summary>
        /// <param name="key">The key of the data to be set or retrieved</param>
        /// <returns>The data specified in the associative array from the specified key</returns>
        public T this[string key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                Add(key, value);
            }
        }

        /// <summary>
        /// Deletes a key from the AssocArray
        /// </summary>
        /// <param name="key">The key to delete from the AssocArray</param>
        /// <exception cref="System.ArgumentNullException">The specified key is <c>null</c></exception>
        public void Delete(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key", "The specified key is null");
            }
            if (!Exists(key))
            {
                throw new MissingFieldException("The specified key was not found", key);
            }
            // Loop through array and delete it
        }

        /// <summary>
        /// Get the array keys of this associative array
        /// </summary>
        /// <returns>Array keys of this associative array</returns>
        public string[] GetKeys()
        {
            int length = Length; // Save calculation time
            string[] ret = new string[length];
            for (int i = 0; i < length; i++)
            {
                ret[i] = data[i, 0].ToString();
            }
            return ret;
        }

        /// <summary>
        /// Returns true or false depending on whether on not the specified key exists
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <exception cref="System.ArgumentNullException">The specified key is <c>null</c></exception>
        /// <returns>true if exists; otherwise false</returns>
        public bool Exists(string key)
        {
            try
            {
                Get(key.ToString());
            }
            catch (MissingFieldException)
            {
                return false;
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException("The specified key is <c>null</c>", "key");
            }
            return true;
        }

        /// <summary>
        /// Merge an associative array into the current array
        /// </summary>
        /// <param name="piecetwo">The array to merge into this array (will end up at the end)</param>
        /// <returns>The current array</returns>
        public AssocArray<T> Merge(AssocArray<T> piecetwo)
        {
            int length = piecetwo.Length;
            string[] keys = piecetwo.GetKeys();
            for (int i = 0; i < length; i++)
            {
                this.Add(
                    keys[i],
                    piecetwo.Get(keys[i]));
            }
            return this;
        }

        /// <summary>
        /// Merges two Associative Arrays
        /// </summary>
        /// <param name="pieceone">The first array to merge (will end up at the beginning)</param>
        /// <param name="piecetwo">The second array to merge (will end up at the end)</param>
        /// <returns>A merged Associative Array with piecetwo following pieceone</returns>
        public AssocArray<T> Merge(AssocArray<T> pieceone, AssocArray<T> piecetwo)
        {
            int length = piecetwo.Length;
            string[] keys = piecetwo.GetKeys();
            for (int i = 0; i < length; i++)
            {
                pieceone.Add(
                    keys[i],
                    piecetwo.Get(keys[i]));
            }
            return pieceone;
        }

        /// <summary>
        /// Get the length of the array as an int
        /// </summary>
        public int Length
        {
            get
            {
                return data.GetLength(0);
            }
        }

        /// <summary>
        /// Get the length of the array as a long
        /// </summary>
        public long LongLength
        {
            get
            {
                return data.GetLongLength(0);
            }
        }
    }
}