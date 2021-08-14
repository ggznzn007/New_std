using System;

namespace except01
{
    class Pro
    {
        static void Main()
        {
            try
            {
                int[] arr = new int[3];
                arr[10] = 1;
            }
            catch(IndexOutOfRangeException except)
            {
                Console.WriteLine(except.Message);
            }
            finally
            {
                Console.WriteLine("The finally block always executes when try block exits.");
            }
        }
    }
}