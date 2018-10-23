using System;

namespace algocs_paint
{
    public class KeyDuplicateException : Exception
    {
        public KeyDuplicateException()
        {
            Console.WriteLine("Key already in tree");
        }
    }
}
