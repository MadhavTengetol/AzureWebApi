using Azure.Security.KeyVault.Secrets;

namespace KeyVaultWebApi.Helper
{
    public class KeyVaultManager : IKeyVaultManager
    {
        private readonly SecretClient secretClient;

        public KeyVaultManager(SecretClient secretClient)
        {
            this.secretClient = secretClient;
        }

        public async Task<string> GetSecret(string secretKey)
        {
            try

            {

                KeyVaultSecret keyValueSecret = await secretClient.GetSecretAsync(secretKey);

                return keyValueSecret.Value;

            }

            catch

            {
                throw;

            }
        }
    }
}
