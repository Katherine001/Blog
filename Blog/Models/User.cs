using System;

namespace Blog.Models
{
    public class User
    {
        public int u_id { get; set; }
        public string u_name { get; set; }
        public string u_pwd { get; set; }
        public int u_rid { get; set; }
        public DateTime u_birth { get; set; }
        public char u_sex { get; set; }
        public string u_rname { get; set; }

        public User()
        {
        }
    }
}