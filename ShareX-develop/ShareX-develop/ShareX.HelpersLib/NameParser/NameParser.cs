#region License Information (GPL v3)

/*
    ShareX - A program that allows you to take screenshots and share any file type
    Copyright (c) 2007-2025 ShareX Team

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v3)

using ShareX.HelpersLib.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ShareX.HelpersLib
{
    public enum NameParserType
    {
        Default,
        Text, // Allows new line
        FileName,
        FilePath,
        URL // URL path encodes
    }

    public class NameParser
    {
        public NameParserType Type { get; private set; }
        public int MaxNameLength { get; set; }
        public int MaxTitleLength { get; set; }
        public int AutoIncrementNumber { get; set; } // %i, %ia, %ib, %iAa, %ix
        public int ImageWidth { get; set; } // %width
        public int ImageHeight { get; set; } // %height
        public string WindowText { get; set; } // %t
        public string ProcessName { get; set; } // %pn
        public TimeZoneInfo CustomTimeZone { get; set; }

        // NEW: Mouse coordinates
        public int MouseX { get; set; } // %mx
        public int MouseY { get; set; } // %my

        // If we're trying to preview via TaskSettings or not
        // Used so that %rf throws "File not found" exceptions and brings up a popup on upload
        // But only returns an error message when previewing to avoid popup spam
        public bool IsPreviewMode { get; set; } = false;

        protected NameParser()
        {
        }

        public NameParser(NameParserType nameParserType)
        {
            Type = nameParserType;
        }

        public static string Parse(NameParserType nameParserType, string pattern)
        {
            return new NameParser(nameParserType).Parse(pattern);
        }

        public string Parse(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return "";
            }

            StringBuilder sb = new StringBuilder(pattern);

            if (WindowText != null)
            {
                string windowText = SanitizeInput(WindowText);

                if (MaxTitleLength > 0)
                {
                    windowText = windowText.Truncate(MaxTitleLength);
                }

                sb.Replace(CodeMenuEntryFilename.t.ToPrefixString(), windowText);
            }

            if (ProcessName != null)
            {
                string processName = SanitizeInput(ProcessName);

                sb.Replace(CodeMenuEntryFilename.pn.ToPrefixString(), processName);
            }

            string width = "", height = "";

            if (ImageWidth > 0)
            {
                width = ImageWidth.ToString();
            }

            if (ImageHeight > 0)
            {
                height = ImageHeight.ToString();
            }

            sb.Replace("%width", width);
            sb.Replace("%height", height);

            // NEW: Mouse coordinates
            sb.Replace("%mx", MouseX.ToString());
            sb.Replace("%my", MouseY.ToString());

            // ... (rest of your existing replacements and logic goes here) ...

            // You may have additional code for other variables below this point.

            return sb.ToString();
        }

        private string SanitizeInput(string input)
        {
            // Replace any invalid characters or logic as needed for your use case
            return input ?? string.Empty;
        }
    }
}
