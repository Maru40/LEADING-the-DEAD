using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaruUtility
{

    public class MyRandom
    {
        static System.Random sm_random = new System.Random(System.DateTime.Now.Millisecond);

        /// <summary>
        /// 何割の確率化変数で受け取る。
        /// 0～100までの間の数値を引き数で受け取る。
        /// </summary>
        /// <param name="prob">確率</param>
        /// <returns>確率でtrueかfalseを返す</returns>
        public static bool RandomProbability(float probability)
        {
            var random = sm_random.Next(0, 100);
            return random <= probability ? true : false;
        }

        /// <summary>
        /// randomな値を返す
        /// </summary>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <returns>randomな値</returns>
        public static int RandomValue(int min, int max)
        {
            return sm_random.Next(min, max);
        }

        /// <summary>
        /// リストの中からrandomに一つ返す。
        /// </summary>
        /// <typeparam name="T">Listの型</typeparam>
        /// <param name="tList">リスト</param>
        /// <returns>randomな要素</returns>
        public static T RandomList<T>(List<T> tList)
            where T : class
        {
            var index = RandomValue(0, tList.Count - 1);
            return tList[index];
		}
    }
}



