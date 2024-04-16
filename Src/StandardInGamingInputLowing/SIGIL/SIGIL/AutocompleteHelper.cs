using System.Text.RegularExpressions;
using AutocompleteMenuNS;

namespace SIGIL
{
    /// <summary>
    /// This item appears when any part of snippet text is typed
    /// </summary>
    class DeclarationSnippet : SnippetAutocompleteItem
    {
        public static string RegexSpecSymbolsPattern = @"[\^\$\[\]\(\)\.\\\*\+\|\?\{\}]";

        public DeclarationSnippet(string snippet)
            : base(snippet)
        {
        }

        public override CompareResult Compare(string fragmentText)
        {
            var pattern = Regex.Replace(fragmentText, RegexSpecSymbolsPattern, "\\$0");
            if (Regex.IsMatch(Text, "\\b" + pattern, RegexOptions.IgnoreCase))
                return CompareResult.Visible;
            return CompareResult.Hidden;
        }
    }

    /// <summary>
    /// Divides numbers and words: "123AND456" -> "123 AND 456"
    /// Or "i=2" -> "i = 2"
    /// </summary>
    class InsertSpaceSnippet : AutocompleteItem
    {
        string pattern;

        public InsertSpaceSnippet(string pattern)
            : base("")
        {
            this.pattern = pattern;
        }

        public InsertSpaceSnippet()
            : this(@"^(\d+)([a-zA-Z_]+)(\d*)$")
        {
        }

        public override CompareResult Compare(string fragmentText)
        {
            if (Regex.IsMatch(fragmentText, pattern))
            {
                Text = InsertSpaces(fragmentText);
                if (Text != fragmentText)
                    return CompareResult.Visible;
            }
            return CompareResult.Hidden;
        }

        public string InsertSpaces(string fragment)
        {
            var m = Regex.Match(fragment, pattern);
            if (m.Groups[1].Value == "" && m.Groups[3].Value == "")
                return fragment;
            return (m.Groups[1].Value + " " + m.Groups[2].Value + " " + m.Groups[3].Value).Trim();
        }

        public override string ToolTipTitle
        {
            get
            {
                return Text;
            }
        }
    }

    /// <summary>
    /// Inerts line break after '}'
    /// </summary>
    class InsertEnterSnippet : AutocompleteItem
    {
        int enterPlace = 0;

        public InsertEnterSnippet()
            : base("[Line break]")
        {
        }

        public override CompareResult Compare(string fragmentText)
        {
            var tb = Parent.TargetControlWrapper;

            var text = tb.Text;
            for (int i = Parent.Fragment.Start - 1; i >= 0; i--)
            {
                if (text[i] == '\n')
                    break;
                if (text[i] == '}')
                {
                    enterPlace = i;
                    return CompareResult.Visible;
                }
            }

            return CompareResult.Hidden;
        }

        public override string GetTextForReplace()
        {
            var tb = Parent.TargetControlWrapper;

            //insert line break
            tb.SelectionStart = enterPlace + 1;
            tb.SelectedText = "\n";
            Parent.Fragment.Start += 1;
            Parent.Fragment.End += 1;
            return Parent.Fragment.Text;
        }

        public override string ToolTipTitle
        {
            get
            {
                return "Insert line break after '}'";
            }
        }
    }
}