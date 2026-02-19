using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MoySklad.Api.Client;
using MoySklad.Api.Utils;

var directory = Directory.GetCurrentDirectory();

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile("appsettings.Development.json", optional: true)
    .Build();

var token = configuration["MoySklad:Token"]
            ?? throw new InvalidOperationException("MoySklad: Token not configured");

var debug = configuration.GetValue<bool>("MoySklad:Debug", false);

var userTimeZoneId = configuration["MoySklad:UserTimeZoneId"]
                     ?? throw new InvalidOperationException("MoySklad: UserTimeZoneId not configured");

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .AddConfiguration(configuration.GetSection("Logging"))
        .AddConsole();
});

var logger = loggerFactory.CreateLogger<ApiClient>();

var config = new MoySkladConfig
{
    Token = token,
    Debug = debug,
    UserTimeZone = TimeZoneInfo.FindSystemTimeZoneById(userTimeZoneId)
};

var client = new MoySkladClient(config, logger);

// PBH32-C6

//var product = await client.Products.FindByIdAsync("051c47d5-e15f-11f0-0a80-071100054e0d");
// var productFolders = await client.ProductFolders.FindAllAsync();
// var products = await client.Products.FindAllAsync();
// var productsList = await client.Products.FetchAllAsync();
var product = await client.Products.FindByIdAsync("056873cc-e15f-11f0-0a80-071100054e1c");
// var assortment = await client.Assortments.FindByExternalCode("cfSV44DWhfhVe1twllW9g2");
// var assortments = await client.Assortments.FetchAllAsync();
var customerOrders = await client.CustomerOrders.FindAllAsync();
var demands = await client.Demands.FindAllAsync();
var demandPositons = await client.Demands.GetPositionsAsync(demands?.Rows[0].Id);

Console.WriteLine(product);