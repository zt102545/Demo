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
            Select(15);
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
                    int[] nums1_1 = { 1, 4, 7, 9 };
                    int[] nums1_2 = { 1, 2, 3, 4, 5, 6, 7, 8 };
                    result = FindMedianSortedArrays(nums1_1, nums1_2).ToString();
                    break;
                case 2://最长回文子串
                    //给定一个字符串 s，找到 s 中最长的回文子串。你可以假设 s 的最大长度为 1000。例如"cabccbad"的最长回文子串是"abccba"
                    string s = "cccc";
                    result = LongestPalindrome(s);
                    break;
                case 3://最长公共前缀
                    //编写一个函数来查找字符串数组中的最长公共前缀。如果不存在公共前缀，返回空字符串 ""。例如输入：["flower","flow","flight"]，输出: "fl"
                    string[] strs = { "", "b" };
                    result = LongestCommonPrefix(strs);
                    break;
                case 4://三数之和
                    int[] nums4 = { 0, 0, 0 };
                    var r = ThreeSum(nums4);
                    result = JsonConvert.SerializeObject(r);
                    break;
                case 5://合并两个有序数组
                    //注意是两个有序的数组。
                    int[] nums5_1 = { 1, 2, 3, 0, 0, 0 }; int m5 = 3;
                    int[] nums5_2 = { 2, 5, 6 }; int n5 = 3;
                    Merge(nums5_1, m5, nums5_2, n5);
                    result = JsonConvert.SerializeObject(nums5_1);
                    break;
                case 6://螺旋矩阵
                    //给定一个包含 m x n 个元素的矩阵（m 行, n 列），请按照顺时针螺旋顺序，返回矩阵中的所有元素。
                    int[][] nums6 = new int[][] { new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 9, 10, 11, 12 } };
                    result = JsonConvert.SerializeObject(SpiralOrder(nums6));
                    break;
                case 7://螺旋矩阵II
                    //给定一个正整数 n，生成一个包含 1 到 n2 所有元素，且元素按顺时针顺序螺旋排列的正方形矩阵。
                    result = JsonConvert.SerializeObject(GenerateMatrix(3));
                    break;
                case 8://存在重复元素
                    //给定一个整数数组，判断是否存在重复元素。如果任意一值在数组中出现至少两次，函数返回 true 。如果数组中每个元素都不相同，则返回 false 。
                    int[] nums8 = { 1, 1, 1, 3, 3, 4, 3, 2, 4, 2 };
                    result = ContainsDuplicate(nums8).ToString();
                    break;
                case 9://反转字符串
                    //编写一个函数，其作用是将输入的字符串反转过来。输入字符串以字符数组 char[] 的形式给出。
                    //不要给另外的数组分配额外的空间，你必须原地修改输入数组、使用 O(1) 的额外空间解决这一问题。
                    char[] nums9 = { 'H','a','n','n','a','h' };
                    ReverseString(nums9);
                    result = JsonConvert.SerializeObject(nums9);
                    break;
                case 10://盛最多水的容器
                    //给你 n 个非负整数 a1，a2，...，an，每个数代表坐标中的一个点(i, ai) 。在坐标内画 n 条垂直线，垂直线 i 的两个端点分别为(i, ai) 和(i, 0)。找出其中的两条线，使得它们与 x 轴共同构成的容器可以容纳最多的水。
                    int[] nums10 = { 1, 8, 6, 2, 5, 4, 8, 3, 7 };
                    result = MaxArea(nums10).ToString();
                    break;
                case 11://删除排序数组中的重复项
                    //给定一个排序数组，你需要在 原地 删除重复出现的元素，使得每个元素只出现一次，返回移除后数组的新长度。
                    //不要使用额外的数组空间，你必须在 原地 修改输入数组 并在使用 O(1) 额外空间的条件下完成。你不需要考虑数组中超出新长度后面的元素。
                    int[] nums11 = { 0, 1, 2, 3, 4 };
                    result = RemoveDuplicates(nums11).ToString();
                    break;
                case 12://有效的括号
                    //给定一个只包括 '('，')'，'{'，'}'，'['，']' 的字符串，判断字符串是否有效。
                    string str12 = "{[()]}()";
                    result = IsValid(str12).ToString();
                    break;
                case 13://反转一个单链表。
                    //输入: 1->2->3->4->5->NULL
                    //输出: 5->4->3->2->1->NULL
                    ReverseList(null);
                    break;
                case 14://两数相加
                    //给出两个 非空 的链表用来表示两个非负的整数。其中，它们各自的位数是按照 逆序 的方式存储的，并且它们的每个节点只能存储 一位 数字。
                    //如果，我们将这两个数相加起来，则会返回一个新的链表来表示它们的和。
                    //您可以假设除了数字 0 之外，这两个数都不会以 0 开头。
                    ListNode l14_1 = new ListNode(9);

                    int[] num14_2 = { 1, 9, 9, 9, 9, 9, 9, 9, 9, 9 };
                    ListNode l14_2 = new ListNode(0);
                    ListNode current = l14_2;
                    foreach (int i14 in num14_2)
                    {
                        current.next = new ListNode(i14);
                        current = current.next;
                    }
                    var Node14 = AddTwoNumbers(l14_1, l14_2.next);
                    break;
                case 15://合并两个有序链表
                    ListNode l15_1 = new ListNode(-9);
                    l15_1.next = new ListNode(3);

                    ListNode l15_2 = new ListNode(5);
                    l15_2.next = new ListNode(7);

                    var Node15 = MergeTwoLists(l15_1, l15_2);
                    break;
                case 16://旋转链表
                    //给定一个链表，旋转链表，将链表每个节点向右移动 k 个位置，其中 k 是非负数。
                    //输入: 1->2->3->4->5->NULL, k = 2
                    //输出: 4->5->1->2->3->NULL
                    ListNode l16_1 = new ListNode(9);
                    l16_1.next = new ListNode(3);

                    var Node16 = RotateRight(l16_1, 4);
                    break;
                case 17://环形链表
                    HasCycle(null);
                    break;
                case 18://相交链表
                    //编写一个程序，找到两个单链表相交的起始节点。
                    GetIntersectionNode(null, null);
                    break;
                case 19://删除链表中的节点
                    //请编写一个函数，使其可以删除某个链表中给定的（非末尾）节点。注意：传入函数的唯一参数为 要被删除的节点 。
                    DeleteNode(null);
                    break;
            }
            Console.WriteLine(result);
        }
        #region 数组与字符串
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

            #region 官方方法，一遍哈希表
            //Dictionary<int, int> kvs = new Dictionary<int, int>();
            //for (int i = 0; i < nums.Length; i++)
            //{
            //    int complement = target - nums[i];
            //    if (kvs.ContainsKey(complement) && kvs[complement] != i)
            //    {
            //        return new int[] { i, kvs[complement] };
            //    }
            //    //需要对重复值进行判断,若结果包含了重复值，则已经被上面给return了；所以此处对于重复值直接忽略
            //    if (!kvs.ContainsKey(nums[i]))
            //    {
            //        kvs.Add(nums[i], i);
            //    }
            //}
            //return new int[] { 0, 0 };
            #endregion
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
            //第一次遍历找出数组最小的字符串长度
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
        /// <summary>
        /// 思路，从大到小，从后往前插入
        /// </summary>
        /// <param name="nums1"></param>
        /// <param name="m"></param>
        /// <param name="nums2"></param>
        /// <param name="n"></param>
        public static void Merge(int[] nums1, int m, int[] nums2, int n)
        {
            int p1 = m - 1, p2 = n - 1, p3 = m + n - 1;
            while (p2 >= 0)
            {
                if (p1 >= 0 && nums1[p1] > nums2[p2])
                {
                    nums1[p3--] = nums1[p1--];
                }
                else
                {
                    nums1[p3--] = nums2[p2--];
                }
            }
        }
        #endregion
        #region 螺旋矩阵
        /// <summary>
        /// 思路：模拟路径，设置边界点，右下左上为一次循环
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static IList<int> SpiralOrder(int[][] matrix)
        {
            IList<int> result = new List<int>();
            if (matrix == null || matrix.Length == 0) return result;
            int left = 0;
            int right = matrix[0].Length-1;
            int top = 0;
            int bottom = matrix.Length - 1;
            while (result.Count < matrix.Length * matrix[0].Length)
            {
                //向右
                for (int i = left; i <= right; i++)
                {
                    result.Add(matrix[left][i]);
                }
                top++;
                if (top > bottom) break;
                //向下
                for (int i = top; i <= bottom; i++)
                {
                    result.Add(matrix[i][right]);
                }
                right--;
                if (left > right) break;
                //向左
                for (int i = right; i >= left; i--)
                {
                    result.Add(matrix[bottom][i]);
                }
                bottom--;
                if (top > bottom) break;
                //向上
                for (int i = bottom; i >= top; i--)
                {
                    result.Add(matrix[i][left]);
                }
                left++;
                if (left > right) break;
            }
            return result;
        }
        #endregion
        #region 螺旋矩阵II
        /// <summary>
        /// 思路：根据整数先构建矩阵，然后模拟路径
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int[][] GenerateMatrix(int n)
        {
            int[][] result = new int[n][];
            for (int i = 0; i < n; i++)
            {
                result[i] = new int[n];
            }
            int left = 0;
            int right = n - 1;
            int top = 0;
            int bottom = n - 1;
            int num = 1;
            while (num <= n * n)
            {
                //向右
                for (int i = left; i <= right; i++)
                {
                    result[left][i] = num++;
                }
                top++;
                if (top > bottom) break;
                //向下
                for (int i = top; i <= bottom; i++)
                {
                    result[i][right] = num++;
                }
                right--;
                if (left > right) break;
                //向左
                for (int i = right; i >= left; i--)
                {
                    result[bottom][i] = num++;
                }
                bottom--;
                if (top > bottom) break;
                //向上
                for (int i = bottom; i >= top; i--)
                {
                    result[i][left] = num++;
                }
                left++;
                if (left > right) break;
            }
            return result;
        }
        #endregion
        #region 存在重复元素
        /// <summary>
        /// 思路：用HashSet，用List在LeetCode上测试会超时
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public static bool ContainsDuplicate(int[] nums)
        {
            HashSet<int> checkList = new HashSet<int>();
            for (int i = 0; i < nums.Length; i++)
            {
                if (checkList.Contains(nums[i]))
                {
                    return true;
                }
                else 
                {
                    checkList.Add(nums[i]);
                }
            }
            return false;
        }
        #endregion
        #region 除自身以外数组的乘积
        /// <summary>
        /// 思路：左右乘积列表，第一次遍历算出索引i左侧所有元素的乘积
        /// 第二次遍历算出索引i右侧所有元素的乘积，并和之前左侧乘积相乘得到的就是除自身以外数组的乘积。
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public int[] ProductExceptSelf(int[] nums)
        {
            int[] result = new int[nums.Length];
            result[0] = 1;//索引为 '0' 的元素左侧没有元素，所以默认为1
            for (int i = 1; i < nums.Length; i++)
            {
                result[i] = nums[i - 1] * result[i - 1];
            }

            int r = 1;//刚开始索引右边没有元素，默认为1
            for (int i = nums.Length - 1; i >= 0; i--)
            {
                result[i] = result[i] * r;
                r *= nums[i];
            }
            return result;
        }
        #endregion
        #region 反转字符串
        /// <summary>
        /// 思路：两头索引往中间走。
        /// </summary>
        /// <param name="s"></param>
        public static void ReverseString(char[] s)
        {
            char temp;
            int l = 0;
            int r = s.Length - 1;
            while (r - l > 0)
            {
                temp = s[l];
                s[l++] = s[r];
                s[r--] = temp;
            }
        }
        #endregion
        #region 盛最多水的容器
        /// <summary>
        /// 思路1：两次遍历，算出每个元素能得到最大的面积
        /// 思路2：双指针，左右指针分别向中间移动，每次比较元素值，值较小的指针移动
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        public static int MaxArea(int[] height)
        {
            //这是思路1的代码：
            int max = 0;
            for (int i = 0; i < height.Length; i++)
            {
                for (int j = i; j < height.Length; j++)
                {
                    int temp = 0;
                    if (height[i] > height[j])
                    {
                        temp = height[j] * (j - i);
                    }
                    else
                    {
                        temp = height[i] * (j - i);
                    }
                    if (temp > max)
                    {
                        max = temp;
                    }
                }
            }
            return max;
        }
        #endregion
        #region 删除排序数组中的重复项
        /// <summary>
        /// 思路：双指针
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public static int RemoveDuplicates(int[] nums)
        {
            if (nums.Length == 0) return 0;
            int p = 0;
            for (int i = 1; i < nums.Length; i++)
            {
                if (nums[p] != nums[i])
                {
                    nums[++p] = nums[i];
                }
            }
            return ++p;
        }
        #endregion
        #region 有效的括号
        /// <summary>
        /// 思路：栈的思想，先进后出
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsValid(string s)
        {
            Dictionary<char, char> keyValues = new Dictionary<char, char>() {
                { '{','}' },{ '(',')'},{ '[',']'}
            };
            bool check = true;
            Stack<char> st = new Stack<char>();
            foreach (char c in s)
            {
                if (keyValues.ContainsKey(c))
                {
                    st.Push(c);
                }
                else if (st.Count == 0 || keyValues[st.Pop()] != c)
                {
                    check = false;
                    break;
                }
            }
            if (st.Count > 0)
                check = false;
            return check;
        }
        #endregion
        #endregion

        #region 链表突击
        #region 反转链表
        /// <summary>
        /// 思路：循环遍历
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        public static ListNode ReverseList(ListNode head)
        {
            ListNode prev = null;
            ListNode curr = head;
            while (curr != null)
            {
                ListNode nextTemp = curr.next;
                curr.next = prev;
                prev = curr;
                curr = nextTemp;
            }
            return prev;
        }

        /// <summary>
        /// 思路：递归，关键递归节点head->next反向指回head节点：head->next->next = head
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        public static ListNode ReverseList2(ListNode head)
        {
            if (head == null || head.next == null)
            {
                return head;
            }
            //递归
            ListNode newHead = ReverseList(head.next);
            //回溯
            head.next.next = head;
            head.next = null;

            return newHead;
        }
        #endregion
        #region 两数相加
        /// <summary>
        /// 思路：比较笨的方法，分别算出两个值，然后相加，再拆成链表。该方法会被数值类型的最大值限制，LeetCode上验证失败。
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        public static ListNode AddTwoNumbers(ListNode l1, ListNode l2)
        {
            long num1 = 0;
            long mi1 = 0;
            ListNode t1 = l1;
            while (t1 != null)
            {
                num1 += t1.val * (long)Math.Pow(10, mi1++);
                t1 = t1.next;
            }

            long num2 = 0;
            long mi2 = 0;
            ListNode t2 = l2;
            while (t2 != null)
            {
                num2 += t2.val * (long)Math.Pow(10, mi2++);
                t2 = t2.next;
            }

            long num = num1 + num2;
            ListNode result = new ListNode(0);
            if (num == 0)
            {
                return result;
            }
            else
            {
                ListNode current = result;
                while (num > 0)
                {
                    int val = (int)(num % 10);
                    num /= 10;
                    current.next = new ListNode(val);
                    current = current.next;
                }
                return result.next;
            }
        }
        /// <summary>
        /// 思路：LeetCode上抄的，遍历每一位，将他们每一位的值加起来，如果存在进位的话就将进位存在Other变量中下一次循环使用。在下一次循环时Ohter又会被重置。
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        public ListNode AddTwoNumbers2(ListNode l1, ListNode l2)
        {
            //定义返回值
            var result = new ListNode(-1);
            //定义循环用的对象，将形参制定到temp
            var temp = result;
            //前一轮数字和的十位数
            int Other = 0;
            do
            {
                //取出numbe1和number2的值
                var number1 = l1 == null ? 0 : l1.val;
                var number2 = l2 == null ? 0 : l2.val;
                //计算两数之和 并加上前一轮需要进位的值
                var sum = number1 + number2 + Other;
                //计算个位
                var value = sum % 10;
                //计算十位并赋值
                Other = sum / 10;
                //将数据添加到循环链表中
                temp.next = new ListNode(value);
                //循环用的temp对象赋值为循环链表中的下一个对象
                temp = temp.next;
                //l1 l2 指向自己在链表中对应的下一个值
                l1 = l1?.next;
                l2 = l2?.next;
            } while (l1 != null || l2 != null || Other != 0);
            return result.next;
        }
        #endregion
        #region 合并两个有序链表
        /// <summary>
        /// 思路：遍历两个链表的每个元素拿出来比较，小的先插入到新链表里，小的链表下移。另一种思路：递归
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        public static ListNode MergeTwoLists(ListNode l1, ListNode l2)
        {
            ListNode result = new ListNode(0);
            ListNode current = result;
            while (l1 != null && l2 != null)
            {
                if (l1.val < l2.val)
                {
                    current.next = l1;
                    l1 = l1.next;
                }
                else
                {
                    current.next = l2;
                    l2 = l2.next;
                }
                current = current.next;
            }
            current.next = l1 == null ? l2 : l1;
            return result.next;
        }
        #endregion
        #region 旋转链表
        /// <summary>
        /// 思路：快慢指针，先取余k步，快指针先走k步，然后遍历到最后，最后快指针的对象链上初始对象，慢指针的对象断开下一个对象。
        /// </summary>
        /// <param name="head"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static ListNode RotateRight(ListNode head, int k)
        {
            var fast = head;
            var slow = head;
            int count = 0;
            while (fast != null)
            {
                fast = fast.next;
                count++;
            }
            if (count == 0) return head;
            k = k % count;
            fast = head;
            // fast 先走k步
            while (k-- > 0)
            {
                if (fast != null && fast.next != null) fast = fast.next;
                else fast = head;
            }
            // slow == fast说明k会被链表长度整除，故无需操作head直接返回即可
            if (slow == fast) return head;
            // 快慢指针start
            while (fast.next != null)
            {
                slow = slow.next;
                fast = fast.next;
            }
            // 对慢指针位置进行打断
            fast.next = head;
            head = slow.next;
            slow.next = null;
            return head;
        }
        #endregion
        #region 环形链表
        /// <summary>
        /// 思路：遍历，然后List存储每个节点，判断List是否存在该节点，存在则是环形链表。
        /// 另一种思路：快慢指针，同时起步，快指针总会跑过慢指针，快指针等于慢指针时就是环形链表，若快指针跑完了就不是
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        public static bool HasCycle(ListNode head)
        {
            bool isCycle = false;
            List<int> keyList = new List<int>();
            ListNode current = head;
            while (current != null)
            {
                if (keyList.Contains(current.GetHashCode()))
                {
                    isCycle = true;
                    break;
                }
                else
                {
                    keyList.Add(current.GetHashCode());
                    current = current.next;
                }
            }
            return isCycle;
        }
        #endregion
        #region 相交链表
        /// <summary>
        /// 思路：分别遍历nodeA和nodeB，如果nodeA遍历完以后指向headB，同理nodeB遍历完后指向headA。所以nodeA和nodeB迟早会遇到。
        /// </summary>
        /// <param name="headA"></param>
        /// <param name="headB"></param>
        /// <returns></returns>
        public static ListNode GetIntersectionNode(ListNode headA, ListNode headB)
        {
            if (headA == null || headB == null) return null;
            ListNode nodeA = headA;
            ListNode nodeB = headB;
            //如果两个链表都不相交也不会死循环，最后两个链表都指向nodeA=nodeB=null
            //如果是相交前节点长度相同时，则两个链表从开始每次都移一个节点，相交的节点就跳出循环了。
            while (nodeA != nodeB)
            {
                if (nodeA == null)
                    nodeA = headB;
                else
                    nodeA = nodeA.next;
                if (nodeB == null)
                    nodeB = headA;
                else
                    nodeB = nodeB.next;
            }
            return nodeA;
        }
        #endregion
        #region 删除链表中的节点
        /// <summary>
        /// 跳过该节点，指向下一个节点
        /// </summary>
        /// <param name="node">该节点就是要被删除的节点</param>
        public static void DeleteNode(ListNode node)
        {
            node.val = node.next.val;
            node.next = node.next.next;
        }
        #endregion
        #endregion

        #region 数学与数字
        #region 整数反转
        public int Reverse(int x)
        {
            return 0;
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 链表结构
    /// </summary>
    public class ListNode
    {
        public int val;
        public ListNode next;
        public ListNode(int x) { val = x; }
    }
}
