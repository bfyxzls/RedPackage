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
            FixMoney = new List<double>() { 10, 8, 5, 20 };
            FixMoneyLevel = FixMoney.Count;
            RealContainer = new List<double>();
            SendMoney = new List<double>();
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
            int fixCount = (int)Math.Floor(Count * FixMoneyLevel / 100.0);
            if (fixCount > FixMoney.Count)
                fixCount = FixMoney.Count;

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
            int randomCount = Count - fixCount;
            Console.WriteLine("radom money balance:{0}", Money);
            randNum(randomCount);

            RealContainer=RealContainer.OrderBy(x => Guid.NewGuid()).ToList();
        }

        //拆分数值生成若干个和等于该数值随机值
        public void randNum(int num)
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

        protected static double NextDouble(Random random, double miniDouble, double maxiDouble)
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

            redPackage.Money = 100;//100元
            redPackage.Count = 10;
            redPackage.FixMoneyLevel = 50;//50%的固定
            redPackage.GenerateRedPackage();
            double sum = 0;
            for (int i = 0; i < redPackage.RealContainer.Count; i++)
            {
                sum += redPackage.RealContainer[i];
                Console.WriteLine("第:{0}个,本次领取的金额:{1},已经领取的总金额:{2}", i, redPackage.RealContainer[i], sum);
            }
            Console.ReadKey();
        }


    }
}
