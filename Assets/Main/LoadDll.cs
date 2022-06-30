using System;
using System.Collections.Generic;
using System.Linq;
using BM;
using ET;
using UnityEngine;

public class LoadDll : MonoBehaviour
{
    public static AssetBundle AssemblyAssetBundle { get; private set; }

    private System.Reflection.Assembly gameAss;
    
    private void Start()
    {
        Initialize().Coroutine();
    }

    private void Update()
    {
        AssetComponent.Update();
    }

    private async ETTask Initialize()
    {
        await CheckHotfix();
        await LoadGameDll();
        RunMain();
    }

    private async ETTask CheckHotfix()
    {
        //重新配置热更路径(开发方便用, 打包移动端需要注释注释)
        //AssetComponentConfig.HotfixPath = Application.dataPath + "/../HotfixBundles/";
        
        AssetComponentConfig.DefaultBundlePackageName = "AllBundle";
        Dictionary<string, bool> updatePackageBundle = new Dictionary<string, bool>()
        {
            {AssetComponentConfig.DefaultBundlePackageName, false},
            {"Scripts", false},
        };
        UpdateBundleDataInfo updateBundleDataInfo = await AssetComponent.CheckAllBundlePackageUpdate(updatePackageBundle);
        if (updateBundleDataInfo.NeedUpdate)
        {
            Debug.LogError("需要更新, 大小: " + updateBundleDataInfo.NeedUpdateSize);
            await AssetComponent.DownLoadUpdate(updateBundleDataInfo);
        }
        await AssetComponent.Initialize(AssetComponentConfig.DefaultBundlePackageName);
        await AssetComponent.Initialize("Scripts");
    }
    
    private async ETTask LoadGameDll()
    {
        if((AssetComponentConfig.AssetLoadMode == AssetLoadMode.Develop))
        {
            gameAss = AppDomain.CurrentDomain.GetAssemblies().First(assembly => assembly.GetName().Name == "Hotfix");
        }
        else
        {
            TextAsset hotfixDll = await AssetComponent.LoadAsync<TextAsset>(
                BPath.Assets_HuatuoBuildCache_AssetBundleSourceData_StandaloneWindows64_HotFix__dll__bytes, "Scripts");
            gameAss = System.Reflection.Assembly.Load(hotfixDll.bytes);
        }
    }

    private void RunMain()
    {
        if (gameAss == null)
        {
            Debug.LogError("dll未加载");
            return;
        }
        var appType = gameAss.GetType("App");
        var mainMethod = appType.GetMethod("Main");
        mainMethod.Invoke(null, null);

        // 如果是Update之类的函数，推荐先转成Delegate再调用，如
        //var updateMethod = appType.GetMethod("Update");
        //var updateDel = System.Delegate.CreateDelegate(typeof(Action<float>), null, updateMethod);
        //updateDel(deltaTime);
    }
}