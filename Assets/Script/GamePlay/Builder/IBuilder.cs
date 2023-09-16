using UnityEngine;


namespace Carrot
{
    public interface IBuilder<T>
    {

        /// <summary>
        /// 获取到游戏物体身上的脚本对象，从而去赋值
        /// </summary>
        T GetProductClass(GameObject gameObject);

        /// <summary>
        /// 使用工厂去获取具体的游戏对象
        /// </summary>
        GameObject GetProduct();

        /// <summary>
        /// 获取数据信息
        /// </summary>
        void GetData(T productClassGo);

        /// <summary>
        /// 获取特有资源与信息
        /// </summary>
        void GetOtherResource(T productClassGo);
    }
}
