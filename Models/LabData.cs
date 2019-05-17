using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace pavlovLab.Models
{
    public class LabData
    {
          public Guid Id { get; set; } = Guid.Empty;
                public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Sport { get; set; }
        public byte Kolvo  { get; set; }

        public BaseModelValidationResult Validate()
        {
            var validationResult = new BaseModelValidationResult();

            if (string.IsNullOrWhiteSpace(Name)) validationResult.Append($"Name cannot be empty");
            if (!(0 < Kolvo )) validationResult.Append($"Kolvo ne mozhet byt otricatelnoe");

            if (!string.IsNullOrEmpty(Name) && !char.IsUpper(Name.FirstOrDefault())) validationResult.Append($"Name {Name} should start from capital letter");
            if (!string.IsNullOrEmpty(City) && !char.IsUpper(City.FirstOrDefault())) validationResult.Append($"City {City} should start from capital letter");
            if (!string.IsNullOrEmpty(Country) && !char.IsUpper(Country.FirstOrDefault())) validationResult.Append($"Surname {Country} should start from capital letter");
            return validationResult;
        }

        public override string ToString()
        {
            return $"{City} {Country} from {Name}";
}

    }
}