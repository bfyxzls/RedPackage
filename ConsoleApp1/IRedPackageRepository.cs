using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public interface IRedPackageRepository
    {
        /// <summary>
        /// 保存红包，每个红包有唯一的redId和一个红包容器
        /// </summary>
        /// <param name="redId"></param>
        /// <param name="list"></param>
        void Save(string redId, List<Double> list);
        /// <summary>
        /// 根据红包ID从它的容器里随机取出一个红包
        /// </summary>
        /// <param name="redId"></param>
        /// <returns></returns>
        Double Get(string redId);
    }
}
