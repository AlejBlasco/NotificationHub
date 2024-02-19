namespace NotificationHub.API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer()
                .AddSwaggerGen();

            builder.Services.AddAPILayer();
            builder.Services.AddApplicationLayer();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGroup("/api").WithOpenApi();
            if (app.Environment.IsDevelopment()) _ = app.UseSwagger().UseSwaggerUI();
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
