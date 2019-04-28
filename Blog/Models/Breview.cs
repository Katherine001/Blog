using System;

namespace Blog.Models
{
    public class Breview
    {
        public int p_id { get; set; }
        public string p_content { get; set; }
        public int p_uid { get; set; }
        public int p_aid { get; set; }
        public DateTime p_time { get; set; }

        public Breview()
        {
        }
    }
}