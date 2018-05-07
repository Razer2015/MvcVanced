using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace MvcVanced.Helpers
{
    public static class Extensions
    {
        public static string ParseLinks(this string s) {
            var arr = s.Split(' ');
            var sb = new StringBuilder();

            for (int i = 0; i < arr.Length; i++) {
                var item = arr[i];
                if (item.Equals("build", StringComparison.OrdinalIgnoreCase)) {
                    sb.Append(item);

                    if (arr.Length > (i + 1) && arr[i + 1].StartsWith("#")) {
                        sb.Append(' ');
                        var tag = arr[i + 1];
                        sb.Append($"<a href=\"/Home/Changelogs?changelog=BUILD{tag.TrimEnd(',')}\">{tag}</a>");
                        i++;
                    }
                    //<a href="/Home/Changelogs?changelog=BUILD#19.0">Build 19.0</a>
                }
                else if (item.Equals("theme", StringComparison.OrdinalIgnoreCase)) {
                    sb.Append(item);
                    sb.Append(' ');

                    if (arr.Length > (i + 1) && arr[i + 1].StartsWith("#")) {
                        sb.Append(' ');
                        var tag = arr[i + 1];
                        sb.Append($"<a href=\"/Home/Changelogs?changelog=THEME{tag.TrimEnd(',')}\">{tag}</a>");
                        i++;
                    }
                }
                else {
                    sb.Append(item);
                }
                sb.Append(' ');
            }
            return sb.ToString();
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
    (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source) {
                if (seenKeys.Add(keySelector(element))) {
                    yield return element;
                }
            }
        }
    }
}