namespace EmployeeApi.Models
{
    public record Employee
    {
        public Guid Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }

        public Employee(Guid id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }


    }
}
