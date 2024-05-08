namespace ConsoleApp
{
    public struct Customer
    {
        public int Id;
        public string Name;
        public string Passport;
        public string Nationality;
        public readonly override string ToString()
        {
            return $"{Id} {Name} {Passport} {Nationality}";
        }
        public Customer(int id, string name, string passport, string nationality)
        {
            Id = id;
            Name = name;
            Passport = passport;
            Nationality = nationality;
        }
    }
}

