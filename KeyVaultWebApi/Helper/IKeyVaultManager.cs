namespace KeyVaultWebApi.Helper
{
    public interface IKeyVaultManager
    {
        public Task<string> GetSecret(string secretKey);
    }
}
