using System;
namespace SmartLab
{
    public static class Utility
    {
        public static int FindMatch(this byte[] data, byte[] pattern, int startIndex)
        {
            if (pattern.Length > data.Length)
                return -1;
            for (int i = startIndex; i < data.Length - pattern.Length + 1; i++)
            {
                int k = 0;
                for (int j = 0; j < pattern.Length; j++)
                {
                    if (pattern[j] != data[i + j])
                        break;
                    else k++;
                }
                if (k == pattern.Length)
                    return i;
            }
            return -1;
        }

        public static bool CompairBytes(this byte[] a, byte[] b)
        {
            if (a.Length == b.Length)
            {
                int i = 0;
                for (i = 0; i < a.Length; i++)
                    if (a[i] != b[i])
                        break;
                if (i == a.Length)
                    return true;
                else return false;
            }
            else return false;
        }

        public static byte[] CombineArray(this byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Array.Copy(first, 0, ret, 0, first.Length);
            Array.Copy(second, 0, ret, first.Length, second.Length);
            return ret;
        }

        public static byte[] ExtractRangeFromArray(this byte[] source, int offset, int length)
        {
            byte[] cache = new byte[length];
            Array.Copy(source, offset, cache, 0, cache.Length);
            return cache;
        }
    }
}

