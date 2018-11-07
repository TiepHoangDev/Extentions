using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace LibraryCommon
{
    public static class StringHelper
    {
        public static string ToAscii(this string unicode)
        {
            if (string.IsNullOrWhiteSpace(unicode)) return "";
            unicode = unicode.ToLower();
            unicode = Regex.Replace(unicode, "[-\\s+/]+", "-");
            unicode = Regex.Replace(unicode, "[áàảãạăắằẳẵặâấầẩẫậ]", "a");
            unicode = Regex.Replace(unicode, "[óòỏõọôồốổỗộơớờởỡợ]", "o");
            unicode = Regex.Replace(unicode, "[éèẻẽẹêếềểễệ]", "e");
            unicode = Regex.Replace(unicode, "[íìỉĩịIÍĨÌỈĨ]", "i");
            unicode = Regex.Replace(unicode, "[úùủũụưứừửữự]", "u");
            unicode = Regex.Replace(unicode, "[ýỳỷỹỵ]", "y");
            unicode = Regex.Replace(unicode, "[đ]", "d");
            unicode = Regex.Replace(unicode, "\\W+", "-");
            return unicode;
        }


        public static String PassedTime(this DateTime? ndate)
        {
            if (ndate == null) return "";
            var date = ndate.Value;
            var passtime = DateTime.Now - date;
            if (passtime.TotalMinutes < 1) return "vừa xong";
            if (passtime.TotalMinutes < 60) return (int)passtime.TotalMinutes + " phút trước";
            if (passtime < TimeSpan.FromDays(1)) return (int)passtime.TotalHours + " giờ trước";
            if (passtime < TimeSpan.FromDays(7)) return (int)passtime.TotalDays + " ngày trước";
            return date.ToString("HH:mm dd/MM/yyyy");
        }
    }
}