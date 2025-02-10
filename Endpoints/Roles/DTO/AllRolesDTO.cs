namespace SSO_Backend.Endpoints.Roles.DTO
{
    public class AllRolesDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsServicePrincipal { get; set; }
    }
}
