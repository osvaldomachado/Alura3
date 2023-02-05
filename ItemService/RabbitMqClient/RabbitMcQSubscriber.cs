using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ItemService.EventProcessor;

namespace ItemService.RabbitMqClient
{
    public class RabbitMcQSubscriber : BackgroundService
    {
        private IConfiguration _configuration;
        private string _nomeDaFila;
        private IConnection _connection; 
        private IModel _channel;
        private IProcessaEvento _processaEvento;

    public RabbitMcQSubscriber(IConfiguration configuration)
        {
            _configuration = configuration;

            _connection = new ConnectionFactory()
                                { 
                                    HostName = _configuration["RabbitMqHost"], 
                                    Port = Int32.Parse(_configuration["RabbitMqPort"]) 
                                }.CreateConnection();

            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange:"trigger", type: ExchangeType.Fanout);

            _nomeDaFila = _channel.QueueDeclare().QueueName;

            _channel.QueueBind(queue: _nomeDaFila, exchange:"trigger", routingKey:"");

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            EventingBasicConsumer? consumidor = new EventingBasicConsumer(_channel);

            consumidor.Received += (ModuleHandle, ea) =>
            {
                    ReadOnlyMemory<byte> body = ea.Body;
                    
                    string? mensagem = Encoding.UTF8.GetString(body.ToArray());

                    _processaEvento.Processa(mensagem);
            };
            
            _channel.BasicConsume(queue: _nomeDaFila, autoAck:true, consumer: consumidor);

            return Task.CompletedTask;
        }     


    }
}