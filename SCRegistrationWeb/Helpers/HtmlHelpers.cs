using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCRegistrationWeb.Helpers
{
    public static class HtmlHelpers
    {
        public static string Truncate(this HtmlHelper helper, string input, int length, bool dots = true)
        {
            if (input.Length <= length)
            {
                return input;
            } 
            else 
            {
                if (dots)
                {
                    return input.Substring(0, length) + "...";
                }
                else
                {
                    return input.Substring(0, length);
                }
            }

        }
    }
}