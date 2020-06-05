using System;
using System.ComponentModel.Design;

namespace LeetCode
{
    class Program
    {
        static void Main(string[] args)
        {
            Select(2);
            Console.ReadKey();
        }

        public static void Select(int i)
        {
            string result = "";
            switch (i)
            {
                case 0://两数之和
                    //给定一个整数数组 nums 和一个目标值 target，请你在该数组中找出和为目标值的那 两个 整数，并返回他们的数组下标。
                    //你可以假设每种输入只会对应一个答案。但是，数组中同一个元素不能使用两遍。
                    int[] nums = { 2, 7, 11, 15 };
                    int target = 9;
                    result = TwoSum(nums, target).ToString();
                    break;
                case 1://寻找两个正序数组的中位数------不会，抄的
                    //给定两个大小为 m 和 n 的正序（从小到大）数组 nums1 和 nums2。
                    //请你找出这两个正序数组的中位数，并且要求算法的时间复杂度为 O(log(m +n))。
                    //你可以假设 nums1 和 nums2 不会同时为空。
                    int[] nums1 = { 1, 4, 7, 9 };
                    int[] nums2 = { 1, 2, 3, 4, 5, 6, 7, 8 };
                    result = FindMedianSortedArrays(nums1, nums2).ToString();
                    break;
                case 2://最长回文子串
                    string s = "ccc";
                    result=LongestPalindrome(s);
                    break;
            }
            Console.WriteLine(result);
        }

        #region 两数之和
        public static int[] TwoSum(int[] nums, int target)
        {
            for (int i = 0; i < nums.Length; i++)
            {
                int i2 = Array.IndexOf(nums, (target - nums[i]));
                if (i2 != -1 && i != i2)
                {
                    int[] r = { i, i2 };
                    return r;
                }
            }
            return null;
        }
        #endregion
        #region 寻找两个正序数组的中位数
        public static double FindMedianSortedArrays(int[] nums1, int[] nums2)
        {
            int n = nums1.Length;
            int m = nums2.Length;
            int len = n + m;
            int kPre = (len + 1) / 2;
            int k = (len + 2) / 2;
            if (len % 2 == 0)
                return (GetKth(nums1, 0, n - 1, nums2, 0, m - 1, kPre) + GetKth(nums1, 0, n - 1, nums2, 0, m - 1, k)) * 0.5;
            else
                return GetKth(nums1, 0, n - 1, nums2, 0, m - 1, k);
        }

        private static int GetKth(int[] nums1, int start1, int end1, int[] nums2, int start2, int end2, int k)
        {
            int len1 = end1 - start1 + 1;
            int len2 = end2 - start2 + 1;
            //让 len1 的长度小于 len2，这样就能保证如果有数组空了，一定是 len1 
            if (len1 > len2) return GetKth(nums2, start2, end2, nums1, start1, end1, k);
            if (len1 == 0) return nums2[start2 + k - 1];
            if (k == 1) return Math.Min(nums1[start1], nums2[start2]);
            int i = start1 + Math.Min(len1, k / 2) - 1;
            int j = start2 + Math.Min(len2, k / 2) - 1;
            if (nums1[i] > nums2[j])
                return GetKth(nums1, start1, end1, nums2, j + 1, end2, k - (j - start2 + 1));
            else
                return GetKth(nums1, i + 1, end1, nums2, start2, end2, k - (i - start1 + 1));
        }
        #endregion
        #region 最长回文子串
        public static string LongestPalindrome(string s)
        {
            string result = "";
            int n = s.Length;
            //遍历所有的长度
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"{i}:");
                int l = 0;
                int r = 0;
                if (i - 1 >= 0 && i + 1 < n && s[i - 1] == s[i + 1])
                {
                    l = i - 1;
                    r = i + 1;
                }
                else if(i - 1 >= 0 && s[i] == s[i - 1])
                {
                    l = i - 1;
                    r = i;
                }
                else if (i + 1 < n && s[i] == s[i + 1])
                {
                    l = i;
                    r = i + 1;
                }

                while (l >= 0 && r < n)
                {
                    if (s[l] != s[r])
                        break;
                    l--;
                    r++;
                }
                int len = r - l - 1;
                if (len > result.Length)
                {
                    result = s.Substring(l + 1, len);
                    Console.WriteLine(result);
                }
            }
            return result;
        }
        #endregion
    }
}
