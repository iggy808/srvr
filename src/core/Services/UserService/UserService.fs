namespace Core.Services.UserService
open Core.AsyncTools
open Core.Repositories
open Core.Validation.UserService
open System.Threading.Tasks
open Microsoft.Extensions.Logging

type UserService(logger : ILogger<UserService>, userRepository : IUserRepository) =
    let _logger = logger
    let _userRepository = userRepository

    interface IUserService with

      // User -> Task
      member this.CreateUser user = task {
        match CreateUserValidation.IsRequestValid user with
        | false ->
            _logger.LogInformation(
              "CreateUser request is invalid.\n\t" +
              $"Id : {user.Id}\n\t" +
              $"Handle : {user.Handle}")
        | true ->
            TaskTools.await (_userRepository.CreateUser user) 
      }

      // Guid -> Task<Option<User>>
      member this.GetUserById userId = task {
        match GetUserByIdValidation.IsRequestValid userId with
          | false ->
              _logger.LogInformation(
                "GetUserById request is invalid.\n\t" +
                $"UserId : {userId}")
              return None
          | true ->
              let! user = _userRepository.GetUserById userId
              return user
      }

      // User -> Task
      member this.UpdateUser userDelta = task {
        match UpdateUser.Validation.IsRequestValid userDelta with
          | false ->
              _logger.LogInformation(
                "UpdateUser request is invalid.\n\t" +
                $"Id : {userDelta.Id}\n\t" +
                $"Handle : {userDelta.Handle}")
          | true ->
              let! existingUser = _userRepository.GetUserById userDelta.Id
              match existingUser with
                | None ->
                    _logger.LogInformation(
                      $"User with Id : {userDelta.Id} not found.")
                | Some user ->
                   let updatedUser = UpdateUser.Logic.MapUserDeltaToExistingUser userDelta user
                   TaskTools.await (_userRepository.UpdateUser updatedUser)
      }

      // Guid -> Task
      member this.DeleteUserById userId = task {
        match DeleteUserByIdValidation.IsRequestValid userId with
          | false ->
              _logger.LogInformation(
                "DeleteUserById request is invalid.\n\t" +
                $"UserId : {userId}") 
          | true ->
              TaskTools.await (_userRepository.DeleteUserById userId)
      }
