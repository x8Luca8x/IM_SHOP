using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMAPI
{
    public static class Utils
    {
        // Checks if two string are similar
        public static bool IsSimilar(string s1, string s2)
        {
            if (s1 is null || s2 is null)
                return false;

            s1 = s1.ToLower();
            s2 = s2.ToLower();

            if (s1 == s2)
                return true;

            if (s1.Length > s2.Length)
            {
                string temp = s1;
                s1 = s2;
                s2 = temp;
            }

            int n = s1.Length;
            int m = s2.Length;

            int[,] dp = new int[n + 1, m + 1];

            for (int i = 0; i <= n; i++)
                dp[i, 0] = i;

            for (int j = 0; j <= m; j++)
                dp[0, j] = j;

            for (int i = 1; i <= n; i++)
            {
                char c1 = s1[i - 1];
                for (int j = 1; j <= m; j++)
                {
                    char c2 = s2[j - 1];

                    if (c1 == c2)
                        dp[i, j] = dp[i - 1, j - 1];
                    else
                    {
                        int replace = dp[i - 1, j - 1];
                        int insert = dp[i, j - 1];
                        int delete = dp[i - 1, j];

                        dp[i, j] = 1 + Math.Min(replace, Math.Min(insert, delete));
                    }
                }
            }

            return dp[n, m] <= 2;
        }
    }
}
