using System;
using System.Collections.Generic;
using System.Drawing;

namespace LAB04.Bai04
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string DetailUrl { get; set; } = string.Empty;
        public string PosterUrl { get; set; } = string.Empty;
        public string Genre { get; set; } = "Đang cập nhật";
        public string Duration { get; set; } = "Đang cập nhật";
        public decimal StandardPrice { get; set; } = 65000m;
        public List<int> Rooms { get; set; } = new List<int> { 1, 2, 3 };
    }

    public class TicketOrder
    {
        public string CustomerName { get; set; } = string.Empty;
        public string MovieTitle { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public List<string> Seats { get; set; } = new List<string>();
        public decimal TotalAmount { get; set; }
        public DateTime BookingTime { get; set; } = DateTime.Now;
    }

    public readonly struct CrawlProgress
    {
        public int Percent { get; }
        public string Status { get; }

        public CrawlProgress(int percent, string status)
        {
            Percent = Math.Clamp(percent, 0, 100);
            Status = status;
        }
    }

    public class ScrapeResult
    {
        public List<Movie> Movies { get; set; } = new List<Movie>();
        public Dictionary<int, Image> Posters { get; set; } = new Dictionary<int, Image>();
        public bool UsedFallback { get; set; }
        public string Message { get; set; } = string.Empty;
        public string JsonFilePath { get; set; } = string.Empty;
    }
}