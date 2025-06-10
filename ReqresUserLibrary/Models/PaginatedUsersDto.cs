namespace ReqresUserLibrary.Models
{
    public class PaginatedUsersDto
    {
        public int Page { get; set; }
        public int Total_Pages { get; set; }
        public List<UserDto> Data { get; set; } = new();
    }
}
