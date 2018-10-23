using System;

namespace algocs_paint
{
    public class KeySizeException : Exception
    {
        public KeySizeException()
        {
            Console.WriteLine("Key size mismatch");
        }
    }
}
