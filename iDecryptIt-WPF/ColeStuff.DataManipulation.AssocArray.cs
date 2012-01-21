using System;

namespace ColeStuff.DataManipulation
{
    public class AssocArray<T>
    {
        private T[,] data = new T[0, 2] { };
        
        /// <summary>
        /// Constructor function
        /// </summary>
        public AssocArray()
        {
        }

        /// <summary>
        /// Add a new index to the associative array
        /// </summary>
        /// 
        /// <param name="key">The key of the new item</param>
        /// <param name="value">The value of the new item</param>
        /// 
        /// <exception cref="System.ArgumentNullException">The specified key is null</exception>
        public void Add(T key, T value)
        {
            if (key == null)
            {
                // null value ok, just not null key
                throw new ArgumentNullException("key", "The specified key is null");
            }

            // Does the specified key already exist?
            if (Exists(key))
            {
                throw new ArgumentException("The specified key already exists", "key");
            }
            try
            {
                // Resize
                int origlength = data.Length;
                T[,] output = new T[origlength + 1, 2];
                for (int i = 0; i < origlength; i++)
                {
                    output[i, 0] = data[i, 0];
                    output[i, 1] = data[i, 1];
                }

                // Add value
                output[origlength, 0] = key;
                output[origlength, 1] = value;
                data = output;
            }
            catch (Exception ex2)
            {
                throw new Exception(ex2.Message, ex2);
            }
        }

        /// <summary>
        /// Get a value of data from the associative array
        /// </summary>
        /// 
        /// <param name="key">The key of the data to be returned from the associative array</param>
        /// 
        /// <exception cref="System.MissingFieldException">The specified key was not found</exception>
        /// <exception cref="System.ArgumentNullException">The specified key is null or empty</exception>
        /// 
        /// <returns>The data specified in from the specified key in the associative array</returns>
        public T Get(string key)
        {
            if (key == null || key == "")
            {
                throw new ArgumentNullException("key", "The specified key is null or empty");
            }

            int origlength = data.GetLength(0);
            for (int i = 0; i < origlength; i++)
            {
                if (data[i, 0].Equals(key))
                {
                    return data[i, 1];
                }
            }
            
            throw new MissingFieldException("The specified key \"" + key.ToString() + "\" does not exist in the array", key);
        }

        /// <summary>
        /// Returns true or false depending on whether on not the specified key exists
        /// </summary>
        /// 
        /// <param name="key">The key to check</param>
        /// 
        /// <returns>true if exists; otherwise false</returns>
        public bool Exists(T key)
        {
            try
            {
                Get(key.ToString());
            }
            catch (MissingFieldException ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Get the length of the array
        /// </summary>
        public int Length
        {
            get
            {
                return data.GetLength(0);
            }
        }
    }
}