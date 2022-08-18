﻿namespace ServiceBusDemo.Consumer.Events;

public class CustomerUpdatedEvent
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
}