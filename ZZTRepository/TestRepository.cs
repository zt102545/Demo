using System;
using System.Collections.Generic;
using System.Text;
using ZZTIRepository;

namespace ZZTRepository
{
    public class TestRepository : ITestRepository
    {
        public int Sum(int i, int j)
        {
            return i + j;
        }
    }
}
