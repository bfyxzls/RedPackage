using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    /// <summary>
    /// 红包领域对象
    /// </summary>
    class RedPackage
    {
        public RedPackage()
        {
            FixMoney = new List<double>();
            FixMoneyLevel = FixMoney.Count;
            RealContainer = new List<double>();
            SendMoney = new List<double>();
            RandomRealContainer = new Queue<double>();
        }
        /// <summary>
        /// 已发出金额
        /// </summary>
        public List<double> SendMoney { get; set; }
        /// <summary>
        /// 固定金额
        /// </summary>
        public List<double> FixMoney { get; set; }
        /// <summary>
        /// 真实红包容器
        /// </summary>
        public List<double> RealContainer { get; set; }
        /// <summary>
        /// 真实红包容器
        /// </summary>
        public Queue<double> RandomRealContainer { get; set; }
        /// <summary>
        /// 红包总余额
        /// </summary>
        public double Money { get; set; }
        /// <summary>
        /// 红包总数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 固定金额比例，1到100数值，即1%到100%
        /// </summary>
        public int FixMoneyLevel { get; set; }
        /// <summary>
        /// 生成红包，为RealContainer赋值
        /// </summary>
        public void GenerateRedPackage()
        {
            //求固定红包的概率
            int fixCount = (int)Math.Floor(Count * FixMoneyLevel / 100.0);
            //求固定红包的数量，如果固定红包没那么多，会把名额留给随机红包
            if (fixCount > FixMoney.Count)
                fixCount = FixMoney.Count;

            //生成固定金额的红包
            for (int i = 0; i < fixCount; i++)
            {
                if (Money > 0)
                {
                    Random rd = new Random();
                    int randomIndex = rd.Next(0, FixMoney.Count - 1);
                    double val = FixMoney[randomIndex];
                    if (Money - val >= 0)
                    {
                        this.RealContainer.Add(val);
                        FixMoney.Remove(val);
                        Money -= val;
                        SendMoney.Add(val);
                    }
                    else
                    {
                        fixCount--;
                    }
                }
            }
            //求随机红包的数量
            int randomCount = Count - fixCount;
            Console.WriteLine("需要产生随机红包的金额:{0}", Money);
            //产生随机红包
            randNum(randomCount);
            //为红包池进行随机排序 
            RealContainer=RealContainer.OrderBy(x => Guid.NewGuid()).ToList();
            //添加到随机队列
            foreach (var item in RealContainer)
                RandomRealContainer.Enqueue(item);
        }

       /// <summary>
       /// 抢红包
       /// </summary>
       /// <param name="username"></param>
       /// <param name="container"></param>
       /// <returns></returns>
        public double receiver(string username)
        {
            double val = RandomRealContainer.Dequeue();
            Console.WriteLine("这个用户:{0}，领取了一个红包:{1}",username,val);
            return val;
        }
        /// <summary>
        /// 拆分数值生成若干个和等于该数值随机值
        /// </summary>
        /// <param name="num"></param>
        private void randNum(int num)
        {
            double totalAmount = this.Money;
            List<double> list = new List<double>();
            double minAmount = 0.01;
            Random r = new Random();
            for (int i = 0; i < num; i++)
            {
                double money = minAmount;
                if (i == num - 1)
                {
                    //最后一次
                    money = totalAmount;
                }
                else
                {
                    double safeAmount = (totalAmount - (num - i) * minAmount) / (num - i);
                    money = NextDouble(r, minAmount * 100, safeAmount * 100) / 100;
                    money = Math.Round(money, 2, MidpointRounding.AwayFromZero);
                    totalAmount = totalAmount - money;
                    totalAmount = Math.Round(totalAmount, 2, MidpointRounding.AwayFromZero);
                }
                list.Add(money);
                this.RealContainer.Add(money);
                SendMoney.Add(money);

            }


        }
        /// <summary>
        /// 产生随机数
        /// </summary>
        /// <param name="random"></param>
        /// <param name="miniDouble"></param>
        /// <param name="maxiDouble"></param>
        /// <returns></returns>
        private double NextDouble(Random random, double miniDouble, double maxiDouble)
        {
            if (random != null)
            {
                return random.NextDouble() * (maxiDouble - miniDouble) + miniDouble;
            }
            else
            {
                return 0.0d;
            }
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            RedPackage redPackage = new RedPackage();
            redPackage.FixMoney = new List<double> { 10, 30 };
            redPackage.Money = 50;//100元
            redPackage.Count = 10;
            redPackage.FixMoneyLevel = 50;//50%的固定
            redPackage.GenerateRedPackage();
            Console.WriteLine("开始产生红包");
            foreach(var item in redPackage.RealContainer)
            {
                Console.Write(item+",");
            }
            Console.WriteLine("开始模拟领红包");
            redPackage.receiver("张三");
            redPackage.receiver("郴四");
            redPackage.receiver("mike");

            Console.ReadKey();
        }


    }
}
