namespace TheOrchidArchade.Utils
{
    using Microsoft.AspNetCore.DataProtection;

    public class EncryptionHelper
    {
        private readonly IDataProtector _protector;

        public EncryptionHelper(IDataProtectionProvider dataProtectionProvider)
        {
            _protector = dataProtectionProvider.CreateProtector("CreditCardNumberProtector");
        }

        public string Encrypt(string plainText)
        {
            return _protector.Protect(plainText);
        }

        public string Decrypt(string encryptedText)
        {
            return _protector.Unprotect(encryptedText);
        }
    }

}
