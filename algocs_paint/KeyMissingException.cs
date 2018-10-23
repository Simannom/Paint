using System;

namespace algocs_paint
{
    public class KeyMissingException : Exception
    {
        public KeyMissingException()
        {
            Console.WriteLine("Key not found");
        }
    }
}
