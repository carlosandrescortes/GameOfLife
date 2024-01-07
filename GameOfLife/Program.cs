using GameOfLife.Repositories;
using GameOfLife.Services;
using Newtonsoft.Json;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IRepository, JsonRepository>();
builder.Services.AddSingleton<IBoardServices, BoardServices>();

builder.Logging.ClearProviders();
builder.Host.UseSerilog((context, config) =>
{
	config
		.MinimumLevel.Information()
		.WriteTo.Console()
		.WriteTo.File("log.txt", rollingInterval: RollingInterval.Day);
});

var app = builder.Build();

var boardServices = app.Services.CreateScope().ServiceProvider.GetRequiredService<IBoardServices>();

app.MapPost("/board", async (HttpContext context) =>
{
	try
	{
		var json = await new StreamReader(context.Request.Body).ReadToEndAsync();
		var cells = JsonConvert.DeserializeObject<int[,]>(json);

		return (await boardServices.AddBoardAsync(cells)).ToString();
	}
	catch (Exception ex)
	{
		Log.Error(ex, $"Error in POST /board");
		return ex.Message;
	}
});

app.MapGet("/board/{id}", async (int id) =>
{
	try
	{
		return await boardServices.GetNextStateAsync(id, 1);
	}
	catch (Exception ex)
	{
		Log.Error(ex, $"Error in GET /board/{id}");
		return ex.Message;
	}
});

app.MapGet("/board/{id}/{x}", async (int id, int x) =>
{
	try
	{
		return await boardServices.GetNextStateAsync(id, x);
	}
	catch (Exception ex)
	{
		Log.Error(ex, $"Error in GET /board/{id}/{x}");
		return ex.Message;
	}
});

app.MapGet("/board/{id}/final/{x}", async (int id, int x) =>
{
	try
	{
		return await boardServices.GetFinalStateAsync(id, x);
	}
	catch (Exception ex)
	{
		Log.Error(ex, $"Error in GET /board/{id}/final/{x}");
		return ex.Message;
	}
});

app.Run();

public partial class Program
{ }