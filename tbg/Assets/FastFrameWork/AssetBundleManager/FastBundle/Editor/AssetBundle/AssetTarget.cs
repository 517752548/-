﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


    public enum FastAssetType
{
        Asset,
        Builtin
    }

    public class FastAssetTarget : System.IComparable<FastAssetTarget>
    {
        /// <summary>
        /// 目标Object
        /// </summary>
        public Object asset;
        /// <summary>
        /// 文件路径
        /// </summary>
        public FileInfo file;
        /// <summary>
        /// 相对于Assets文件夹的目录
        /// </summary>
        public string assetPath;
        /// <summary>
        /// 此文件是否已导出
        /// </summary>
        public bool isExported;
        /// <summary>
        /// 素材类型
        /// </summary>
        public FastAssetType type = FastAssetType.Asset;
        /// <summary>
        /// 导出类型
        /// </summary>
        public FastAssetBundleExportType exportType = FastAssetBundleExportType.Asset;
        /// <summary>
        /// 保存地址
        /// </summary>
        public string bundleSavePath;
        /// <summary>
        /// BundleName
        /// </summary>
        public string bundleName;
        /// <summary>
        /// 短名
        /// </summary>
        public string bundleShortName;

        public int level = -1;
        public List<FastAssetTarget> levelList;

        //目标文件是否已改变
        private bool _isFileChanged = false;
        //是否已分析过依赖
        private bool _isAnalyzed = false;
        //依赖树是否改变（用于增量打包）
        private bool _isDepTreeChanged = false;
        //上次打包的信息（用于增量打包）
        private AssetCacheInfo _cacheInfo;

        //上次打好的AB的CRC值（用于增量打包）
        private string _bundleCrc;
        //是否是新打包的
        private bool _isNewBuild;
        /// <summary>
        /// 我要依赖的项
        /// </summary>
        private HashSet<FastAssetTarget> _dependParentSet = new HashSet<FastAssetTarget>();
        /// <summary>
        /// 依赖我的项
        /// </summary>
        private HashSet<FastAssetTarget> _dependChildrenSet = new HashSet<FastAssetTarget>();

        public FastAssetTarget(Object o, FileInfo file, string assetPath)
        {
            this.asset = o;
            this.file = file;
            this.assetPath = assetPath;
            this.bundleShortName = file.Name.ToLower();
            this.bundleName = FastAssetBundleUtils.ConvertToABName(assetPath) + FastContent.BundleSuffix;
            this.bundleSavePath = Path.Combine(FastAssetBundleUtils.pathResolver.BundleSavePath, bundleName);

            _isFileChanged = true;
        }


        /// <summary>
        /// 分析引用关系
        /// </summary>
        public void Analyze()
        {
            if (_isAnalyzed) return;
            _isAnalyzed = true;

            _cacheInfo = FastAssetBundleUtils.GetCacheInfo(assetPath);
            _isFileChanged = _cacheInfo == null || !_cacheInfo.fileHash.Equals(GetHash());
            if (_cacheInfo != null)
            {
                _bundleCrc = _cacheInfo.bundleCrc;
                if (_isFileChanged)
                    Debug.Log("File was changed : " + assetPath);
            }

            Object[] deps = EditorUtility.CollectDependencies(new Object[] { asset });
            List<Object> depList = new List<Object>();
            for (int i = 0; i < deps.Length; i++)
            {
                Object o = deps[i];
                //不包含脚本对象
                //不包含LightingDataAsset对象
                if (o is MonoScript || o is LightingDataAsset)
                    continue;

                //不包含builtin对象
                string path = AssetDatabase.GetAssetPath(o);
                if (path.StartsWith("Resources"))
                    continue;

                depList.Add(o);
            }
            deps = depList.ToArray();


            var res = from s in deps
                      let obj = AssetDatabase.GetAssetPath(s)
                      select obj;
            var paths = res.Distinct().ToArray();

            for (int i = 0; i < paths.Length; i++)
            {
                if (File.Exists(paths[i]) == false)
                {
                    //Debug.Log("invalid:" + paths[i]);
                    continue;
                }
                FileInfo fi = new FileInfo(paths[i]);
            FastAssetTarget target = FastAssetBundleUtils.Load(fi);
                if (target == null)
                    continue;

                this.AddDependParent(target);

                target.Analyze();
            }
        }

        public void Merge()
        {
            if (this.NeedExportStandalone())
            {
                var children = new List<FastAssetTarget>(_dependChildrenSet);
                this.RemoveDependChildren();
                foreach (FastAssetTarget child in children)
                {
                    child.AddDependParent(this);
                }
            }
        }

        private void GetRoot(HashSet<FastAssetTarget> rootSet)
        {
            switch (this.exportType)
            {
                case FastAssetBundleExportType.Standalone:
                case FastAssetBundleExportType.Root:
                    rootSet.Add(this);
                    break;
                default:
                    foreach (FastAssetTarget item in _dependChildrenSet)
                    {
                        item.GetRoot(rootSet);
                    }
                    break;
            }
        }

        private bool beforeExportProcess;

        /// <summary>
        /// 在导出之前执行
        /// </summary>
        public void BeforeExport()
        {
            if (beforeExportProcess) return;
            beforeExportProcess = true;

            foreach (FastAssetTarget item in _dependChildrenSet)
            {
                item.BeforeExport();
            }

            if (this.exportType == FastAssetBundleExportType.Asset)
            {
                HashSet<FastAssetTarget> rootSet = new HashSet<FastAssetTarget>();
                this.GetRoot(rootSet);
                if (rootSet.Count > 1)
                    this.exportType = FastAssetBundleExportType.Standalone;
            }
        }


        public void UpdateLevel(int level, List<FastAssetTarget> lvList)
        {
            this.level = level;
            if (level == -1 && levelList != null)
                levelList.Remove(this);
            this.levelList = lvList;
        }

        /// <summary>
        /// 获取所有依赖项
        /// </summary>
        /// <param name="list"></param>
        public void GetDependencies(HashSet<FastAssetTarget> list)
        {
            var ie = _dependParentSet.GetEnumerator();
            while (ie.MoveNext())
            {
            FastAssetTarget target = ie.Current;
                if (target.needSelfExport)
                {
                    list.Add(target);
                }
                else
                {
                    target.GetDependencies(list);
                }
            }
        }

        public List<FastAssetTarget> dependencies
        {
            get { return new List<FastAssetTarget>(_dependParentSet); }
        }

        public FastAssetBundleExportType compositeType
        {
            get
            {
            FastAssetBundleExportType type = exportType;
                if (type == FastAssetBundleExportType.Root && _dependChildrenSet.Count > 0)
                    type |= FastAssetBundleExportType.Asset;
                return type;
            }
        }

        public bool isNewBuild
        {
            get { return _isNewBuild; }
        }

        public string bundleCrc
        {
            get { return _bundleCrc; }
            set
            {
                _isNewBuild = value != _bundleCrc;
                if (_isNewBuild)
                {
                    Debug.Log("Export AB : " + bundleName);
                }
                _bundleCrc = value;
            }
        }

        /// <summary>
        /// 是不是需要重编
        /// </summary>
        public bool needRebuild
        {
            get
            {
                if (_isFileChanged || _isDepTreeChanged)
                    return true;

                foreach (FastAssetTarget child in _dependChildrenSet)
                {
                    if (child.needRebuild)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// 是不是自己的原因需要重编的，有的可能是因为被依赖项的原因需要重编
        /// </summary>
        public bool needSelfRebuild
        {
            get
            {
                if (_isFileChanged || _isDepTreeChanged)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// 是不是自身的原因需要导出如指定的类型prefab等，有些情况下是因为依赖树原因需要单独导出
        /// </summary>
        public bool needSelfExport
        {
            get
            {
                if (type == FastAssetType.Builtin)
                    return false;

                bool v = exportType == FastAssetBundleExportType.Root || exportType == FastAssetBundleExportType.Standalone;

                return v;
            }
        }

        /// <summary>
        /// 是否需要导出
        /// </summary>
        public bool needExport
        {
            get
            {
                if (isExported)
                    return false;

                bool v = needSelfExport && needRebuild;

                return v;
            }
        }

        /// <summary>
        /// (作为AssetType.Asset时)是否需要单独导出
        /// </summary>
        /// <returns></returns>
        private bool NeedExportStandalone()
        {
            return _dependChildrenSet.Count > 1;
        }

        /// <summary>
        /// 增加依赖项
        /// </summary>
        /// <param name="target"></param>
        private void AddDependParent(FastAssetTarget target)
        {
            if (target == this || this.ContainsDepend(target))
                return;

            _dependParentSet.Add(target);
            target.AddDependChild(this);
            this.ClearParentDepend(target);
        }

        /// <summary>
        /// 是否已经包含了这个依赖（检查子子孙孙）
        /// </summary>
        /// <param name="target"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        private bool ContainsDepend(FastAssetTarget target, bool recursive = true)
        {
            if (_dependParentSet.Contains(target))
                return true;
            if (recursive)
            {
                var e = _dependParentSet.GetEnumerator();
                while (e.MoveNext())
                {
                    if (e.Current.ContainsDepend(target, true))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void AddDependChild(FastAssetTarget parent)
        {
            _dependChildrenSet.Add(parent);
        }

        /// <summary>
        /// 我依赖了这个项，那么依赖我的项不需要直接依赖这个项了
        /// </summary>
        private void ClearParentDepend(FastAssetTarget target = null)
        {
            IEnumerable<FastAssetTarget> cols = _dependParentSet;
            if (target != null) cols = new FastAssetTarget[] { target };
            foreach (FastAssetTarget at in cols)
            {
                var e = _dependChildrenSet.GetEnumerator();
                while (e.MoveNext())
                {
                FastAssetTarget dc = e.Current;
                    dc.RemoveDependParent(at);
                }
            }
        }

        /// <summary>
        /// 移除依赖项
        /// </summary>
        /// <param name="target"></param>
        /// <param name="recursive"></param>
        private void RemoveDependParent(FastAssetTarget target, bool recursive = true)
        {
            _dependParentSet.Remove(target);
            target._dependChildrenSet.Remove(this);

            //recursive
			var dcc = new HashSet<FastAssetTarget>(_dependChildrenSet);
            var e = dcc.GetEnumerator();
            while (e.MoveNext())
            {
            FastAssetTarget dc = e.Current;
                dc.RemoveDependParent(target);
            }
        }

        private void RemoveDependChildren()
        {
            var all = new List<FastAssetTarget>(_dependChildrenSet);
            _dependChildrenSet.Clear();
            foreach (FastAssetTarget child in all)
            {
                child._dependParentSet.Remove(this);
            }
        }

        /// <summary>
        /// 依赖我的项
        /// </summary>
        public List<FastAssetTarget> dependsChildren
        {
            get { return new List<FastAssetTarget>(_dependChildrenSet); }
        }

        int System.IComparable<FastAssetTarget>.CompareTo(FastAssetTarget other)
        {
            return other._dependChildrenSet.Count.CompareTo(_dependChildrenSet.Count);
        }

        public string GetHash()
        {
            if (type == FastAssetType.Builtin)
                return "0000000000";
            else
                return FastAssetBundleUtils.GetFileHash(file.FullName);
        }
        public void WriteCache(StreamWriter sw)
        {
            sw.WriteLine(this.assetPath);
            sw.WriteLine(GetHash());
            sw.WriteLine(this._bundleCrc);
            HashSet<FastAssetTarget> deps = new HashSet<FastAssetTarget>();
            this.GetDependencies(deps);
            sw.WriteLine(deps.Count.ToString());
            foreach (FastAssetTarget at in deps)
            {
                sw.WriteLine(at.assetPath);
            }
        }
    }
