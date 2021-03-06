using ProSeqqoLib.Model.Hierarchy;
using System.Collections.Generic;

namespace ProSeqqoLib.Helper
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
                tmp += list[^1].ToString("0.##");
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

        public static string FitFor(this string tmp, int space)
        {
            if (tmp.Length < space)
            {
                for (int i = 0; i < space - tmp.Length; i++)
                {
                    tmp += " ";
                }
            }
            return tmp;
        }

        public static string ToIDListString(this List<Config> list)
        {
            string tmp = "";
            if (list is not null && list.Count > 0)
            {
                for (int i = 0; i < list.Count - 1; i++)
                {
                    if(list[i] is not null)
                        tmp += list[i].ID + ", ";
                }
                if (list[^1] is not null)
                    tmp += list[^1].ID;

            }
            return tmp;
        }

        public static string ToIDListString(this List<Motion> list)
        {
            string tmp = "";
            if (list is not null && list.Count > 0)
            {
                for (int i = 0; i < list.Count - 1; i++)
                {
                    tmp += list[i].ToString() + ", ";
                }
                tmp += list[^1].ToString();

            }
            return tmp;
        }
    }
}