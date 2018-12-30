using GraphQL.Privacy.Sample.Models.Abstractions;

namespace GraphQL.Privacy.Sample.Models
{
    public class Album : ISupportIDCursor, ICanBeHidden
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public bool IsHidden { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
    }
}
