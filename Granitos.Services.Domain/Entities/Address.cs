namespace Granitos.Services.Domain.Entities;

public sealed class Address
{
    public Address(string street, string country, string state, string city, string zipCode, string? references,
        int interior, int exterior)
    {
        Street = street;
        Country = country;
        State = state;
        City = city;
        ZipCode = zipCode;
        References = references;
        Interior = interior;
        Exterior = exterior;
    }

    public string Street { get; private set; }
    public string Country { get; private set; }
    public string State { get; private set; }
    public string City { get; private set; }
    public string ZipCode { get; private set; }
    public string? References { get; private set; }
    public int Interior { get; private set; }
    public int Exterior { get; private set; }
}