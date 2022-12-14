using Application.Interfaces;
using Application.Mappings;
using Application.Middlewares;
using Application.Services;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Midas.Services;
using Midas.Services.Family;
using Midas.Services.FileStorage;
using Midas.Services.User;
using NLog;
using NLog.Web;
using WebAPI.Extensions;

namespace WebAPI;

public class Startup
{
    private readonly WebApplicationBuilder _builder;
    private readonly Logger _logger;

    public Startup(string[] args)
    {
        _builder = WebApplication.CreateBuilder(args);
        _logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

        _logger.Debug("The Message API was started");
    }

    public Startup SetBuilderOptions()
    {
        _builder.Services.AddControllers().AddNewtonsoftJson();
        _builder.Services.AddEndpointsApiExplorer();
        _builder.Services.AddSwaggerGen();

        return this;
    }

    public Startup SetOpenCors()
    {
        _builder.Services.AddCors(options =>
        {
            options.AddPolicy("Open", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
        });

        _logger.Debug("The CORS open policy was successfully added");

        return this;
    }

    public Startup SetDbContext()
    {
        var transactionConnString = _builder.Configuration.GetConnectionString("TransactionConnection");
        
        _builder.Services.AddDbContext<TransactionDbContext>(options =>
        {
            options.UseSqlServer(transactionConnString).EnableSensitiveDataLogging();
        });
        
        _logger.Debug("SQL connection was successfully added");

        return this;
    }

    public Startup SetMapperConfig()
    {
        var mapperConfig = AutoMapperConfig.Initialize();

        _builder.Services.AddSingleton(mapperConfig);
        _logger.Debug("The mapping config was successfully added");

        return this;
    }

    public Startup AddInternalServices()
    {
        _builder.Services.AddScoped<ITransactionService, TransactionService>();
        _builder.Services.AddScoped<IInvoiceService, InvoiceService>();
        _builder.Services.AddScoped<IDictionaryService, DictionaryService>();
        _logger.Debug("Internal services were successfully added");

        return this;
    }

    public Startup AddInternalRepositories()
    {
        _builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
        _builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        _builder.Services.AddScoped<IDictionaryRepository, DictionaryRepository>();
        _logger.Debug("Internal repositories were successfully added");

        return this;
    }

    public Startup AddLoggerConfig()
    {
        _builder.Logging.ClearProviders();
        _builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
        _builder.Host.UseNLog();

        _logger.Debug("Logger options were successfully added");

        return this;
    }

    public Startup SetExternalServiceClients()
    {
        var userServiceAddress = _builder.Configuration["ServiceAddresses:User"];
        var familyServiceAddress = _builder.Configuration["ServiceAddresses:Family"];
        var fileServiceAddress = _builder.Configuration["ServiceAddresses:File"];
        
        var userHttpClientDelegate = (Action<HttpClient>)(client => client.BaseAddress = new Uri(userServiceAddress));
        var familyHttpClientDelegate = (Action<HttpClient>)(client => client.BaseAddress = new Uri(familyServiceAddress));
        var fileHttpClientDelegate = (Action<HttpClient>)(client => client.BaseAddress = new Uri(fileServiceAddress));
        
        _builder.Services.AddHeaderPropagation(o => o.Headers.Add("Authorization"));
        _builder.Services.AddHttpClient<IUserClient, UserClient>(userHttpClientDelegate)
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
                };
            })
            .AddHeaderPropagation();
        _builder.Services.AddHttpClient<IFamilyClient, FamilyClient>(familyHttpClientDelegate)
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
                };
            })
            .AddHeaderPropagation();
        _builder.Services.AddHttpClient<IFileStorageClient, FileStorageClient>(fileHttpClientDelegate)
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
                };
            })
            .AddHeaderPropagation();
        
        return this;
    }
    
    public void Run()
    {
        var app = _builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseCors("Open");
        app.UseHeaderPropagation();
        //app.MigrateDatabase();
        app.UseHttpsRedirection();
        app.UseMiddleware<AuthorizationMiddleware>();
        app.UseAuthentication();
        app.MapControllers();
        app.Run();
        
        _logger.Debug("Application has been successfully ran");
    }
}