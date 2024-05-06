namespace MoviesApi.Identity
{
    public class RegisterModel
    {
        [MaxLength(100)]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Skill { get; set; }
        public string? Language { get; set; }
        public string? Title { get; set; }


        // Property for the selected role
        public string SelectedRole { get; set; }

    }
}
