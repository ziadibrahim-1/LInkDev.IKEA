namespace LinkDev.IKEA.PL.Models.Roles
{
    public class RoleViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public IEnumerable<UserRoleViewModel> Users { get; set; } = new List<UserRoleViewModel>();

        public RoleViewModel()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
