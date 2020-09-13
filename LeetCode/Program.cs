using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text.Json.Serialization;

namespace LeetCode
{
    class Program
    {
        static void Main(string[] args)
        {
            Select(4);
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
                    //给定一个字符串 s，找到 s 中最长的回文子串。你可以假设 s 的最大长度为 1000。例如"cabccbad"的最长回文子串是"abccba"
                    string s = "cccc";
                    result = LongestPalindrome(s);
                    break;
                case 3://最长公共前缀
                    //编写一个函数来查找字符串数组中的最长公共前缀。如果不存在公共前缀，返回空字符串 ""。例如输入：["flower","flow","flight"]，输出: "fl"
                    string[] strs = { "","b" };
                    result = LongestCommonPrefix(strs);
                    break;
                case 4://三数之和
                    int[] numss = { 0, 0, 0 };
                    var r = ThreeSum(numss);
                    result = JsonConvert.SerializeObject(r);
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
            int end = 2 * n - 1;
            for (int i = 0; i < end; i++)
            {
                double mid = i / 2.0;
                int p = (int)(Math.Floor(mid));
                int q = (int)(Math.Ceiling(mid));
                while (p >= 0 && q < n)
                {
                    if (s[p] != s[q]) break;
                    p--; q++;
                }
                int len = q - p - 1;
                if (len > result.Length)
                    result = s.Substring(p + 1, len);
            }
            return result;
        }
        #endregion
        #region 最长公共前缀
        public static string LongestCommonPrefix(string[] strs)
        {
            string result = "";
            int len = 0;
            for (int i = 0; i < strs.Length; i++)
            {
                if (strs[i].Length == 0)
                {
                    len = 0;
                    break;
                }
                if (len > strs[i].Length || len == 0)
                    len = strs[i].Length;
            }
            for (int i = 0; i < len; i++)
            {
                char v = strs[0][i];
                bool isEqual = true;
                foreach (string item in strs)
                {
                    if (item[i] != v)
                    {
                        isEqual = false;
                        break;
                    }
                }
                if (isEqual)
                {
                    result += v;
                }
                else
                {
                    break;
                }
            }
            return result;
        }
        #endregion
        #region 三数之和
        public static IList<IList<int>> ThreeSum(int[] nums)
        {
            IList<IList<int>> result = new List<IList<int>>();
            if (nums.Length == 3 && nums[0] == 0 && nums[1] == 0 && nums[2] == 0)
            {
                result.Add(new int[] { 0, 0, 0 });
                return result;
            }

            Array.Sort(nums);
            for (int i = 0; i < nums.Length; i++)
            {
                int v = 0 - nums[i];
                for (int j = i + 1; j < nums.Length; j++)
                {
                    int i2 = Array.IndexOf(nums, (v - nums[j]));
                    if (i2 != -1 && i2 > j && i2 != i && j != i)
                    {
                        var r = new List<int>() { nums[i], nums[j], nums[i2] };
                        result.Add(r);
                    }
                }
            }
            return result;
        }

        public static IList<IList<int>> ThreeSum2(int[] nums)
        {
            IList<IList<int>> result = new List<IList<int>>();
            int len = nums.Length;
            if (len < 3) return result;
            Array.Sort(nums);
            for (int i = 0; i < len - 2; i++)
            {
                if (nums[i] > 0) break;
                if (i > 0 && nums[i] == nums[i - 1]) continue; // 去重
                int left = i + 1;
                int right = len - 1;
                while (left < right)
                {
                    int sum = nums[i] + nums[left] + nums[right];
                    if (sum == 0)
                    {
                        result.Add(new List<int>() { nums[i], nums[left], nums[right] });
                        while (left < right && nums[left] == nums[left + 1]) left++; // 去重
                        while (left < right && nums[right] == nums[right - 1]) right--; // 去重
                        left++;
                        right--;
                    }
                    else if (sum < 0) left++;
                    else if (sum > 0) right--;
                }
            }
            return result;
        }
        #endregion
        #region 合并两个有序数组
        public static void Merge(int[] nums1, int m, int[] nums2, int n)
        {
            int[] num = new int[m + n];
            foreach (int item in nums1)
            {
                for (int i = 0; i < num.Length; i++)
                { 
                    
                }
            }    
        }
        #endregion

    }

}
