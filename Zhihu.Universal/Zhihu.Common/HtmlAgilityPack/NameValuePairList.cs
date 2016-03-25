using System;
using System.Collections.Generic;

namespace Zhihu.Common.HtmlAgilityPack
{
    public class NameValuePairList
    {
        #region Fields

        internal readonly String Text;
        private List<KeyValuePair<String, String>> _allPairs;
        private Dictionary<String, List<KeyValuePair<String, String>>> _pairsWithName;

        #endregion

        #region Constructors

        internal NameValuePairList() :
            this(null)
        {
        }

        internal NameValuePairList(String text)
        {
            Text = text;
            _allPairs = new List<KeyValuePair<String, String>>();
            _pairsWithName = new Dictionary<String, List<KeyValuePair<String, String>>>();

            Parse(text);
        }

        #endregion

        #region Internal Methods

        internal static String GetNameValuePairsValue(String text, String name)
        {
            NameValuePairList l = new NameValuePairList(text);
            return l.GetNameValuePairValue(name);
        }

        internal List<KeyValuePair<String, String>> GetNameValuePairs(String name)
        {
            if (name == null)
                return _allPairs;
            return _pairsWithName.ContainsKey(name) ? _pairsWithName[name] : new List<KeyValuePair<String, String>>();
        }

        internal String GetNameValuePairValue(String name)
        {
            if (name == null)
                throw new ArgumentNullException();
            List<KeyValuePair<String, String>> al = GetNameValuePairs(name);
            if (al.Count == 0)
                return String.Empty;

            // return first item
            return al[0].Value.Trim();
        }

        #endregion

        #region Private Methods

        private void Parse(String text)
        {
            _allPairs.Clear();
            _pairsWithName.Clear();
            if (text == null)
                return;

            String[] p = text.Split(';');
            foreach (String pv in p)
            {
                if (pv.Length == 0)
                    continue;
                String[] onep = pv.Split(new[] { '=' }, 2);
                if (onep.Length == 0)
                    continue;
                KeyValuePair<String, String> nvp = new KeyValuePair<String, String>(onep[0].Trim().ToLower(),
                                                                                    onep.Length < 2 ? "" : onep[1]);

                _allPairs.Add(nvp);

                // index by name
                List<KeyValuePair<String, String>> al;
                if (!_pairsWithName.ContainsKey(nvp.Key))
                {
                    al = new List<KeyValuePair<String, String>>();
                    _pairsWithName[nvp.Key] = al;
                }
                else
                    al = _pairsWithName[nvp.Key];

                al.Add(nvp);
            }
        }

        #endregion
    }
}
