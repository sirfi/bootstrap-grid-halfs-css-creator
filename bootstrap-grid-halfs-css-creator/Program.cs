using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace bootstrap_grid_halfs_css_creator
{
    class Program
    {
        static void Main(string[] args)
        {
            File.AppendAllText("bootstrap-grid-halfs.css",GetBootstrapHalfs());
            Console.WriteLine("Its Created");
        }
        public static string GetBootstrapHalfs(int gridColumnCount = 12, int gridGutterWidth = 30,int minScreen = 480)
        {
            var css = "";
            var screens = new Dictionary<string, int> { { "xs", 480 }, { "sm", 768 }, { "md", 992 }, { "lg", 1200 } };
            var types = new[] { "", "pull", "push", "offset" };
            Func<string, string, string, string> className = (screen, col, type) => ".col-" + screen + (string.IsNullOrEmpty(type) ? "" : "-" + type) + "-" + col + "-half";
            Func<string, string, string> classNames = (screen, type) => Enumerable.Range(0, gridColumnCount).Select(x => x.ToString()).Aggregate("", (current, i) => current + (string.IsNullOrEmpty(current) ? "" : ",") + (className(screen, i, type)));
            Func<string, string> classNamesForAllScreen = (type) => screens.Keys.Aggregate("", (current, screenName) => current + (string.IsNullOrEmpty(current) ? "" : ",") + (classNames(screenName, type)));
            Func<string, string> classNamesForAllTypes = (screen) => types.Aggregate("", (current, type) => current + (string.IsNullOrEmpty(current) ? "" : ",") + (classNames(screen, type)));
            Func<string> classNamesForAllScreenAndAllTypes = () => types.Aggregate("", (current, type) => current + (string.IsNullOrEmpty(current) ? "" : ",") + (classNamesForAllScreen(type)));
            css += classNamesForAllScreen("") + "{" + "position: relative;min-height: 1px;padding-right: " + gridGutterWidth / 2 + "px;padding-left: " + gridGutterWidth / 2 + "px;" + "}";
            foreach (var screen in screens)
            {
                if (screen.Value > minScreen)
                    css += "@media (min-width: " + screen.Value + "px) {";

                css += classNames(screen.Key, "") + "{float: left;}";

                for (int i = 0; i < gridColumnCount; i++)
                {
                    css += className(screen.Key, i.ToString(), "") + "{width: " + (((i + 0.5) / gridColumnCount) * 100).ToString().Replace(',', '.') + "%;}";
                    css += className(screen.Key, i.ToString(), "pull") + "{right: " + (((i + 0.5) / gridColumnCount) * 100).ToString().Replace(',', '.') + "%;}";
                    css += className(screen.Key, i.ToString(), "push") + "{right: " + (((i + 0.5) / gridColumnCount) * 100).ToString().Replace(',', '.') + "%;}";
                    css += className(screen.Key, i.ToString(), "offset") + "{margin-left: " + (((i + 0.5) / gridColumnCount) * 100).ToString().Replace(',', '.') + "%;}";
                }

                if (screen.Value > minScreen)
                    css += "}";
            }
            return css;
        }
    }
}
