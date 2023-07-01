namespace PostlyApp.Services
{
    public interface IAccountService
    {
        public Task<bool> GetStatus();
        public Task<bool?> Login(string username, string password);
        public void Logout();
    }
}
