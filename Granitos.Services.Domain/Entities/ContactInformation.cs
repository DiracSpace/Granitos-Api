namespace Granitos.Services.Domain.Entities;

public sealed class ContactInformation
{
    public ContactInformation(string email, string phoneNumber, string facebookUrl, string instagramUrl)
    {
        Email = email;
        PhoneNumber = phoneNumber;
        FacebookUrl = facebookUrl;
        InstagramUrl = instagramUrl;
    }

    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public string FacebookUrl { get; private set; }
    public string InstagramUrl { get; private set; }
}