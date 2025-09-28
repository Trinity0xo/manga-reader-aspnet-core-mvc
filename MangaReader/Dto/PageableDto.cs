using MangaReader.Utils.Constants;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MangaReader.Dto
{
    public class PageableDto
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 15;
        public string? Search { get; set; }
    }
}
