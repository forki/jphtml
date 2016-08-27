﻿using System;
using System.Xml;
using System.Diagnostics;

namespace jphtml.Core.Dic
{
    public class JmdicReader
    {
        readonly XmlDocument _document;
        readonly Jmdictionary _dictionary;

        public JmdicReader(string path)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Console.WriteLine($"Loading {path}");
            _document = new XmlDocument();
            _document.Load(path);
            Console.WriteLine($"Indexing {path}");
            _dictionary = CreateDictionary();
            sw.Stop();
            Console.WriteLine($"done in {sw.ElapsedMilliseconds}ms");
        }

        public string Lookup(string kanji)
        {
            return _dictionary.LookupTranslation(kanji);
        }

        Jmdictionary CreateDictionary()
        {
            var index = new Jmdictionary();
            using (var nodes = _document.SelectNodes($"/JMdict/entry/k_ele/keb[text()]"))
            {
                foreach (XmlElement n in nodes)
                {
                    var gloss = n.ParentNode.ParentNode.SelectSingleNode("sense/gloss");
                    if (gloss != null)
                    {
                        index.Append(n.InnerXml, gloss.InnerXml);
                    }
                }
            }
            return index;
        }
    }
}
