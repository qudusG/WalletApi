using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };
var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.QueueDeclare("interests");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var connectionString = "Server=(localdb)\\mssqllocaldb;Database=WalletDb;Trusted_Connection=True;MultipleActiveResultSets=true";
    var json = JsonConvert.DeserializeObject<PendingTransaction>(message);
    var result = 0;
    var parameters = new SqlParameter[]
    {
        new SqlParameter("@walletId", json.WalletId)
    };
    using (var connection = new SqlConnection(connectionString))
    {
        using var cmd = new SqlCommand("DebitAccount", connection);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddRange(parameters);
        connection.Open();
        result = cmd.ExecuteNonQuery();
        
    }

    Console.WriteLine(message);
};

channel.BasicConsume(queue: "interests", autoAck: true, consumer: consumer);
Console.ReadKey();

public class PendingTransaction
{
    public long WalletId { get; set; }
}