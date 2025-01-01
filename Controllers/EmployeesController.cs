using Confluent.Kafka;
using EmployeeApi.Database;
using EmployeeApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EmployeeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController(EmployeeDbContext dbContext, ILogger<EmployeesController> logger) : ControllerBase
    {
        private readonly ILogger<EmployeesController> _logger = logger;
        private readonly EmployeeDbContext _dbContext = dbContext;

        [HttpGet]
        public async Task <IEnumerable<Employee>> GetEmployees()
        {
            _logger.LogInformation("Requesting all employees");
            return await _dbContext.Employees.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(string firstName, string lastName)
        {
            var employee = new Employee(Guid.NewGuid(), firstName, lastName);
            _dbContext.Add(employee);
            await _dbContext.SaveChangesAsync();
            // START - move to another class
            // this code does not belong here
            var message = new Message<string, string>()
            {
                Key = employee.Id.ToString(),
                Value = JsonSerializer.Serialize(employee)
            };

            var producerConfig = new ProducerConfig()
            {
                BootstrapServers = "172.20.0.3:9094",
                Acks = Acks.All
            };

            var producer = new ProducerBuilder<string, string>(producerConfig).Build();
            await producer.ProduceAsync("employeeTopic", message); // move to configuration
            // // broker 1 broker 2 (ISR)
            // // END - move to another class

            return CreatedAtAction(nameof(CreateEmployee), new { id = employee.Id }, employee);
        }
    }
}

