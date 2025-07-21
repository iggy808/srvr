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

open core.repositories
open core.features


module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)

        builder.Services.AddControllers()

        builder.Services.AddScoped<IUserRepository, UserRepository>()
        builder.Services.AddScoped<IDbConnection>(fun _ ->
            let connection = new SQLiteConnection("Data Source=C:\\srvr\\data\\srvr.db")
            connection.Open()
            connection :> IDbConnection
        )

        builder.Services.AddScoped<User.Features>(fun serviceProvider ->            
            // Partially apply user repository to features
            let userRepository = serviceProvider.GetRequiredService<IUserRepository>()

            {
                User.Features.GetUser = User.getUser userRepository
                User.Features.CreateUser = User.createUser userRepository
                User.Features.UpdateUser = User.updateUser userRepository
                User.Features.DeleteUser = User.deleteUser userRepository
            }
        )


        let app = builder.Build()

        app.UseHttpsRedirection()

        app.UseAuthorization()
        app.MapControllers()

        app.Run()

        exitCode
