
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;

namespace Model
{
    [Table("user")]
    public  class User
    {
        [Key]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("password")]
        public string Password { get; set; }
        public User() { }
        public User(string name, string password)
        {
            Password = password;
            Name = name;
        }
    }

    [Table("article")]
    public record class article
    {
        public article() { }
        [Key]
        public int Id { get; set; }
        [Column("title")]
        public string title { get; set; }
        [Column("content")]
        public string content { get; set; }
        [Column("create_DateTime")]
        public string createDateTime { get; set; }
        [Column("classify")]
        public string classify { get; set; }
        [Column("page_view")]
        public int pageView { get; set; }
        [Column("likes")]
        public int likes { get; set; }

        public article(string name, string title, string content, string createDateTime, string classify, int pageView, int likes)
        {
            this.title = title;
            this.content = content;
            this.createDateTime = createDateTime;
            this.classify = classify;
            this.pageView = pageView;
            this.likes = likes;

            Console.WriteLine("构造方法执行");
        }
    }
}
