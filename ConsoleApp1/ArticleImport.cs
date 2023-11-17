using Npoi.Mapper.Attributes;

namespace ConsoleApp1
{
    internal class ArticleImport
    {
        [Column("Name")]
        public string Name { get; set; }

        [Column("Price")]
        public string Price { get; set; }

        [Column("OnSale")]
        public string OnSale { get; set; }

        [Column("Status")]
        public string Status { get; set; }

        [Column("Category")]
        public string Category { get; set; }

        [Column("EAN")]
        public string EAN { get; set; }

        [Column("Stock")]
        public string Stock { get; set; }
    }
}
