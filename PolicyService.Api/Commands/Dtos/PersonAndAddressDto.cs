namespace PolicyService.Api.Commands.Dtos;

public class PersonDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string TaxId { get; set; }
}

public class AddressDto
{
    public string Country { get; set; }
    public string ZipCode { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
}

