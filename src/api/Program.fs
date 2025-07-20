namespace srvr
#nowarn "20"
open System
open System.Collections.Generic
open System.Data
open System.Data.SQLite
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

open core.Repositories
open core.Services.UserService


module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)

        builder.Services.AddControllers()

        builder.Services.AddScoped<IUserService, UserService>()
        builder.Services.AddScoped<IUserRepository, UserRepository>()
        builder.Services.AddScoped<IDbConnection>(fun _ ->
            let connection = new SQLiteConnection("Data Source=C:\\srvr\\data\\srvr.db")
            connection.Open()
            connection :> IDbConnection
        )

        let app = builder.Build()

        app.UseHttpsRedirection()

        app.UseAuthorization()
        app.MapControllers()

        app.Run()

        exitCode
