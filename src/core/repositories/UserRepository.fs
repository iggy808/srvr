namespace core.Repositories

open core.Records
open Dapper.FSharp.SQLite
open Microsoft.Extensions.Logging
open System.Threading.Tasks
open System
open System.Data
open System.Data.SQLite

type UserRepository(db : IDbConnection, logger : ILogger<UserRepository>) = 

    let _logger = logger
    let _db = db

    interface IUserRepository with

      // User -> Task
      member this.CreateUser user = task {
        _logger.LogInformation(
          "User being inserted into database:\n\t" +
          $"Id: {user.Id},\n\t" +
          $"Handle: {user.Handle}")

        insert {
          into table<User>
          value user
        } |> _db.InsertAsync |> ignore
      }

      // Guid -> Task<Option<User>>
      member this.GetUserById userId = task {
        _logger.LogInformation(
          $"User with Id : {userId} being fetched.")

        let user = (select {
          for user in table<User> do
          where (user.Id = userId)
        } |> _db.SelectAsync<User>).Result

        return user |> Seq.cast<User> |> Seq.toArray |> Array.tryExactlyOne
      }

      // User -> Task
      member this.UpdateUser updatedUser = task {
        _logger.LogInformation(
          $"User with Id : `{updatedUser.Id}` being updated.")

        update {
          for u in table<User> do
          set updatedUser
          where (u.Id = updatedUser.Id)
        } |> _db.UpdateAsync |> ignore
      }

      // Guid -> Task
      member this.DeleteUserById userId = task {
        _logger.LogInformation(
          $"User with Id : {userId} being deleted.")

        delete {
          for user in table<User> do
          where (user.Id = userId)
        } |> _db.DeleteAsync |> ignore
      }
