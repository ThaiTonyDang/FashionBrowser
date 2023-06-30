using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Utilities
{
	public struct Mode
	{
		public const string MODE = "MODE";
		public const string USING_MODAL_CONFIRM = "USING MODAL CONFIRM";
		public const string USING_LABEL_CONFIRM = "USING LABEL CONFIRM";
		public const string LABEL_CONFIRM_SUCCESS = "SUCCESS LABEL CONFIRM";
		public const string LABEL_CONFIRM_FAIL = "FAIL LABEL CONFIRM";
		public const string MODAL_CONFIRM_SUCCESS = "SUCCESS MODAL CONFIRM";
		public const string MODAL_CONFIRM_FAIL = "FAIL MODAL CONFIRM";
		public const string MODAL_CONFIRM = "MODAL CONFIRM";
		public const string LABEL_CONFIRM_CHECK = "LABEL CONFIRM CHECK";
    }

	public struct OPERATOR
	{
		public const string SUBTRACT = "SUB";
		public const string ADDITION = "ADD";
	}

    public struct HTTP
    {
        public const string SLUG = "resource";
    }

    public struct SHIPPING_FEE
    {
        public const decimal SHIPPING = 10000;
    }

    public struct CATEGORY
    {
        public const string MEN_FASHION = "MEN";
        public const string WOMEN_FASHION = "WOMEN";
        public const string KID_FASHION = "KID"; 
    }
}