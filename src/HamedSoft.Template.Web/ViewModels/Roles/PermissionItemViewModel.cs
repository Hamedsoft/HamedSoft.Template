namespace HamedSoft.Template.Web.ViewModels.Roles
{
    public sealed class PermissionItemViewModel
    {
        public Guid PermissionId { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsAssigned { get; set; }
    }
}
