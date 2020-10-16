using System;
using System.Linq;
using System.Collections.Generic;
namespace Task_6.Testing
{
    public static class Utilities
    {
        public static bool IsAscSort<T>(Queue<T> posts)
            where T : class, IComparable
        {
            if (posts.Count == 0)
                throw new Exception($"The {posts.GetType()} do not have inside element.");
            for(int i = 0; i < posts.Count - 1; i++)
            {
                T firstPost = posts.Dequeue();
                if (firstPost.CompareTo(posts.Peek()) != -1)
                    return false;                
            }
            return true;
        }
        public static string GetRandomString(int length)
        {
            var randum = new Random();
            return new String(Enumerable.Range(0, length).Select(n => (Char)(randum.Next(41, 122))).ToArray());
        }
    }
}
