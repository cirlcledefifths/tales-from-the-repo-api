namespace TalesFromRepo.Lambda.Models.Requests
{
    public class CreateUserRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }

    public class UpdateUserRequest
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}