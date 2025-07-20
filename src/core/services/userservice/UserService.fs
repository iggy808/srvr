namespace core.Services.UserService

open System.Threading.Tasks
open Microsoft.Extensions.Logging
open core.Records
open core.Repositories

type UserService(logger : ILogger<UserService>, userRepository : IUserRepository) =
    let _logger = logger
    let _userRepository = userRepository

    interface IUserService with

      // User -> Task
      member this.CreateUser user = task {
        do! _userRepository.CreateUser user
      }

      // int -> Task<Option<User>>
      member this.GetUserById userId = task { 
        let! user = _userRepository.GetUserById userId
        return user
      }

      // User -> Task
      member this.UpdateUser userDelta = task {
        let! existingUser = _userRepository.GetUserById userDelta.Id
        match existingUser with
        | None ->
            _logger.LogInformation(
              $"User with Id : {userDelta.Id} not found.")
        | Some user ->
            let updatedUser = UpdateUser.MapUserDeltaToExistingUser userDelta user
            do! _userRepository.UpdateUser updatedUser
      }

      // int -> Task
      member this.DeleteUserById userId = task {
        do! _userRepository.DeleteUserById userId
      }
