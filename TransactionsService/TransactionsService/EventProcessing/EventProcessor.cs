using AutoMapper;
using System.Text.Json;
using TransactionsService.Data;
using TransactionsService.Dtos;
using TransactionsService.Models;

namespace TransactionsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.CustomerPublished:
                    addCustomer(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notifcationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notifcationMessage);

            switch (eventType.Event)
            {
                case "Customer_Published":
                    Console.WriteLine("--> Customer Published Event Detected");
                    return EventType.CustomerPublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }

        private void addCustomer(string customerPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ITransactionRepository>();

                var customerPublishedDto = JsonSerializer.Deserialize<CustomerPublishedDto>(customerPublishedMessage);

                try
                {
                    var customer = _mapper.Map<Customer>(customerPublishedDto);
                    if (!repo.ExternalCustomerExists(customer.ExternalId))
                    {
                        repo.CreateCustomer(customer);
                        repo.SaveChanges();
                        Console.WriteLine("--> Customer added!");
                    }
                    else
                    {
                        Console.WriteLine("--> Customer already exists...");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Customer to DB {ex.Message}");
                }
            }
        }
    }

    enum EventType
    {
        CustomerPublished,
        Undetermined
    }
}
