﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huatuo.Generators
{
    public class FileRegionReplace
    {
        private readonly string _tplFile;

        private readonly Dictionary<string, string> _regionReplaceContents = new Dictionary<string, string>();

        public FileRegionReplace(string tplFile)
        {
            _tplFile = tplFile;
        }

        public void Replace(string regionName, string regionContent)
        {
            _regionReplaceContents.Add(regionName, regionContent);
        }

        public void Commit(string outputFile)
        {
            string originContent = File.ReadAllText(_tplFile, Encoding.UTF8);

            string resultContent = originContent;

            foreach (var c in _regionReplaceContents)
            {
                resultContent = TemplateUtil.ReplaceRegion(resultContent, c.Key, c.Value);
            }
            File.WriteAllText(outputFile, resultContent, Encoding.UTF8);
        }
    }
}
