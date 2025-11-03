namespace GiraffeChinook
#nowarn "20"
open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpsPolicy
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging

open Microsoft.EntityFrameworkCore
open Giraffe
open GiraffeChinook.Data
open GiraffeChinook.Domain
open GiraffeChinook.Handlers.Artist

module Program =
    let exitCode = 0

    let withDb (f) : HttpHandler = 
        fun next ctx -> 
            let db = ctx.GetService<ChinookContext>()
            f db next ctx


    let webApp : HttpHandler =
        choose [
            route "/" >=> text "Hello Christoffer! You are awsome"
            route "/artists" >=> withDb getAllHttpHandler
            // routef "artists/%i" >=> withDb getByIdHttpHandler
            routef "/artists/%i" (fun id -> withDb (getByIdHttpHandler id))
        ]

    let configureApp (app: IApplicationBuilder) = 
        app.UseGiraffe webApp

    let configureServices (services : IServiceCollection) =
        services.AddGiraffe() |> ignore

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)
        configureServices builder.Services

        builder.Services.AddDbContext<ChinookContext>(fun options ->
            options.UseSqlServer(builder.Configuration.GetConnectionString("LocalSql")) |> ignore
        )

        builder.Services.AddControllers()

        builder.Services.AddEndpointsApiExplorer()    // Needed for Swagger
        builder.Services.AddSwaggerGen()  

        let app = builder.Build()

        if app.Environment.IsDevelopment() then
            app.UseSwagger() |> ignore
            app.UseSwaggerUI() |> ignore // <-- Enables the Swagger UI

        app.UseHttpsRedirection()

        app.UseAuthorization()
        app.MapControllers()

        configureApp app

        app.Run()

        exitCode
