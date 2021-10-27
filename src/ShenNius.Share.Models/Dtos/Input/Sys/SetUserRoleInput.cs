namespace ShenNius.Share.Models.Dtos.Input.Sys
{
    public class SetUserRoleInput
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public bool Status { get; set; } = true;
    }
}
