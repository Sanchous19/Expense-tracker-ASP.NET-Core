namespace Tracker.ViewModels
{
    public class UserRolesViewModel
    {
        public UserRolesViewModel(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public UserRolesViewModel(string id, string name, bool isAdmin, bool isModerator)
            : this(id, name)
        {
            IsAdmin = isAdmin;
            IsModerator = isModerator;
        }

        public UserRolesViewModel(string id, string name, bool isBlockedUser)
            : this(id, name)
        {
            IsBlockedUser = isBlockedUser;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsModerator { get; set; }
        public bool IsBlockedUser { get; set; }
    }
}