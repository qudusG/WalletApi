using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Services.Background
{
    public class InterestService : BackgroundService
    {
        private readonly IConfiguration _config;
        private bool hasRan = false;
        public InterestService(IServiceScopeFactory factory,
            IConfiguration configuration)
        {
            _config = configuration;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (DateTime.UtcNow.Hour == 1)
                {
                    hasRan = false;
                }
                if (DateTime.UtcNow.Hour == 0 && !hasRan)
                {
                    using var connection = new SqlConnection(_config.GetConnectionString("DbContext"));
                    using var command = new SqlCommand("InterestsAdjustment", connection); //Procedure is commented below
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        await command.ExecuteNonQueryAsync();
                        connection.Close();
                        hasRan = true;
                    }
                    catch (Exception ex)
                    {
                        command.Dispose();
                        connection.Dispose();
                        connection.Close();
                        await Task.Delay(TimeSpan.FromSeconds(15));
                    }
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
        
    }
}
/*Declare @batchSize INT = 10000,  
@startPosition INT = 1,
@endPosition INT,
@incrementalPosition INT = 10000  
  
SELECT @endPosition = MAX(Id)   
FROM dbo.Wallets


WHILE @endPosition >= @startPosition
BEGIN
    UPDATE dbo.Wallets
    SET Balance = Balance + (10 / 365) * Balance
    WHERE Id >= @startPosition
    AND   Id <= @incrementalPosition

    SET @startPosition = @startPosition + @batchSize
    SET @incrementalPosition = @startPosition + @batchSize - 1
END */