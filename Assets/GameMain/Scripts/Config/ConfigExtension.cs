//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityGameFramework.Runtime;

namespace TankBattle
{
    /// <summary>
    /// Config文件加载的扩展方法
    /// </summary>
    public static class ConfigExtension
    {
        /// <summary>
        /// 加载配置。
        /// </summary>
        /// <param name="configComponent">全局配置组件名称。</param>
        /// <param name="configName">配置资源名称。</param>
        /// <param name="loadType">配置加载方式：txt | byte</param>
        /// <param name="userData">用户自定义数据</param>
        public static void LoadConfig(this ConfigComponent configComponent, string configName, LoadType loadType, object userData = null)
        {
            if (string.IsNullOrEmpty(configName))
            {
                Log.Warning("Config name is invalid.");
                return;
            }

            configComponent.LoadConfig(configName, AssetUtility.GetConfigAsset(configName, loadType), loadType, Constant.AssetPriority.ConfigAsset, userData);
        }
    }
}
