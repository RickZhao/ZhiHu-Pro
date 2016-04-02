using System;
using System.Diagnostics;
using Zhihu.Common.Helper;

namespace Zhihu.Helper
{
    public class LayoutHelper
    {
        private Boolean _lowerVotingButtonVisiable;

        public String LowerVotingButtonVisiblity
        {
            get {
                Boolean.TryParse(LocalSettingUtility.Instance.Read<String>("lowerVotingButtonVisiable"), out _lowerVotingButtonVisiable);
                return _lowerVotingButtonVisiable ? "Visible" : "Collapsed";
            }
            set
            {
                if (value == "Visible")
                    _lowerVotingButtonVisiable = true;
                else if (value == "Collapsed")
                    _lowerVotingButtonVisiable = false;
                else
                    Debug.WriteLine("invalid value for LowerVotingButtonVisiblity, it can only be 'Visible' or 'Collapsed'.");

                LocalSettingUtility.Instance.Add("lowerVotingButtonVisiable", _lowerVotingButtonVisiable.ToString());                    
            }
        }
    }
}