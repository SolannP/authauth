using AuthAuthDomainModel;
using Newtonsoft.Json;
using System;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;

namespace AuthAuthDomaineService;
public class Account : IEquatable<Account>
{
    protected AccountDTO _authentificationData = new();
    
    public string Contact
    {
        get => _authentificationData.Contact;
    }
    public Account(string login, string password) : this(login, login, password) { }
    public Account(string? contact,string login, string password)
    {
        if (string.IsNullOrWhiteSpace(contact)) contact = login;
        var utf8 = new UTF8Encoding();
        byte[] byteLogin, bytePassword;
        byteLogin = utf8.GetBytes(login);
        string saltedPassword = login + password;
        bytePassword = utf8.GetBytes(saltedPassword);
        using (HashAlgorithm sha = SHA512.Create())
        {
            sha.ComputeHash(byteLogin);
            _authentificationData.Login = sha.Hash ?? throw new InvalidOperationException("Compute hash is null");
            sha.ComputeHash(bytePassword);
            _authentificationData.Password = sha.Hash ?? throw new InvalidOperationException("Compute hash is null");
        }
        _authentificationData.Contact = contact;
    }

    public bool Equals(Account? other)
    {
        if (other is null) return false;
        return other._authentificationData.Login.SequenceEqual(_authentificationData.Login)
            && other._authentificationData.Password.SequenceEqual(_authentificationData.Password)
            && other._authentificationData.Contact.SequenceEqual(_authentificationData.Contact);
    }
    public bool EqualsLogin(Account? other)
    {
        if (other is null) return false;
        return other._authentificationData.Login.SequenceEqual(_authentificationData.Login);
    }
    public bool EqualsContact(Account? other)
    {
        if (other is null) return false;
        return other._authentificationData.Contact.SequenceEqual(_authentificationData.Contact);
    }

    public static string Serialise(Account account)
    {
        return JsonConvert.SerializeObject(account._authentificationData);
    }

    public static Account Deserialize(string serializedData)
    {
        var authentificationData = JsonConvert.DeserializeObject<AccountDTO>(serializedData);
        Account account = new Account("", "");
        account._authentificationData.Login = authentificationData.Login;
        account._authentificationData.Contact = authentificationData.Contact;
        account._authentificationData.Password = authentificationData.Password;
        return account;
    }
}
