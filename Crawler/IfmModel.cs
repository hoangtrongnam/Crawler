using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Crawler
{
    class IfmModel
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public List<TechnicalDetails> TechnicalDetails { get; set; }
    }
    class TechnicalDetails
    {
        public string Id { get; set; }
        public string ItemId { get; set; }
        public string DataName { get; set; }
        public List<DataDescription> DataDescriptions { get; set; }
    }
    class DataDescription
    {
        public string Id { get; set; }
        public string TechnicalDetailId { get; set; }
        public int flag { get; set; }
        public string DataDescriptionDetail { get; set; }
    }
}
