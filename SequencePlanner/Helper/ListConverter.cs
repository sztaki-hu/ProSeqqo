using System.Collections.Generic;

namespace SequencePlanner.Helper
{
    public static class ListConverter
    {
        public static string ToListString(this double[] list)
        {
            if (list.Length > 1)
            {
                string tmp = "";
                if (list is not null && list.Length > 0)
                {
                        for (int i = 0; i < list.Length - 1; i++)
                    {
                        tmp += list[i].ToString("0.#####") + "/ ";
                    }
                    tmp += list[^1].ToString("0.#####");
                }
                return tmp;
            }
            return "";
        }
        public static string ToListString(this List<double> list)
        {
            string tmp = "";
            if (list is not null && list.Count > 0)
            {
                for (int i = 0; i < list.Count - 1; i++)
                {
                    tmp += list[i].ToString("0.##") + "; ";
                }
                tmp += list[^1];
            }
            return tmp;
        }
        public static string ToListString(this List<long> list)
        {
            string tmp = "";
            if (list is not null && list.Count > 0)
            {
                for (int i = 0; i < list.Count - 1; i++)
                {
                    tmp += list[i] + ", ";
                }
                tmp += list[^1];
            }
            return tmp;
        }
        public static string ToListString(this List<int> list)
        {
            string tmp = "";
            if (list is not null && list.Count > 0)
            {
                for (int i = 0; i < list.Count - 1; i++)
                {
                    tmp += list[i] + ", ";
                }
                tmp += list[^1];

            }
            return tmp;
        }
        
        public static string ToListString(this long[] list)
        {
            string tmp = "";
            if (list is not null && list.Length > 0)
            {
                for (int i = 0; i < list.Length - 1; i++)
                {
                    tmp += list[i] + ", ";
                }
                tmp += list[^1];
            }
            return tmp;
        }
    }
}
