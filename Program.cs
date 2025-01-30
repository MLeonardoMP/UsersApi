var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();//OpenAPI native support .NET 9

builder.Logging.ClearProviders();//Clear default logging providers
builder.Logging.AddConsole();//Add console logging provider

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();//Map OpenAPI in development environment
}

// app.UseHttpsRedirection();//Redirect HTTP to HTTPS for security

//Middleware to log all requests
app.Use(async (context, next) =>
{
    Console.WriteLine($"[LOG] Request: {context.Request.Method} {context.Request.Path}");
    await next.Invoke();
});
//Middleware to check if the request has an Authorization header
app.Use(async (context, next) =>
{
    if (!context.Request.Headers.ContainsKey("Authorization"))
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Unauthorized");
        return;
    }
    // ...very basic check to illustrate...
    await next.Invoke();
});


var users = new List<User>();//List to store users
var nextId = 1;//Variable to store the next user id

//Routes
//POST /users - Create a new user
app.MapPost("/users", (User user) =>
{
    //Check if the user data is valid
    if (string.IsNullOrWhiteSpace(user.Name) || user.Age <= 0 ||
        string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Role))
    {
        return Results.BadRequest("Invalid user data");
    }
    var newUser = user with { Id = nextId++ };
    users.Add(newUser);
    return Results.Ok(newUser);
});
//GET /users - Get all users
app.MapGet("/users", () => Results.Ok(users));
//GET /users/{id} - Get a user by id
app.MapGet("/users/{id}", (int id) =>
{   
    var user = users.FirstOrDefault(u => u.Id == id);
    return user is null ? Results.NotFound() : Results.Ok(user);
});
//PUT /users/{id} - Update a user by id
app.MapPut("/users/{id}", (int id, User updated) =>
{
    var index = users.FindIndex(u => u.Id == id);
    if (index < 0) return Results.NotFound();
    if (string.IsNullOrWhiteSpace(updated.Name) || updated.Age <= 0 ||
        string.IsNullOrWhiteSpace(updated.Email) || string.IsNullOrWhiteSpace(updated.Role))
    {
        return Results.BadRequest("Invalid user data");
    }
    users[index] = updated with { Id = id };
    return Results.Ok(users[index]);
});
//DELETE /users/{id} - Delete a user by id
app.MapDelete("/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound();
    users.Remove(user);
    return Results.Ok(user);
});

app.Run();


record User(int Id, string Name, int Age, string Email, string Role); // Im using record instead of class to simplify the code and make it more readable
