namespace core.Services.UserService
open core.Repositories
open System.Threading.Tasks
open Microsoft.Extensions.Logging

type UserService(logger : ILogger<UserService>, userRepository : IUserRepository) =
    let _logger = logger
    let _userRepository = userRepository

    interface IUserService with

      // User -> Task
      // avaTODO: Research using return! here, i.e., return! _userRepo etc.
      member this.CreateUser user = task {
        _userRepository.CreateUser user |> Async.AwaitTask
      }

      // Guid -> Task<Option<User>>
      // avaTODO: Research using return! here, i.e., return! _userRepo etc.
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
            // avaTODO: Research using return! here, i.e., return! _userRepo etc.
            _userRepository.UpdateUser updatedUser |> Async.AwaitTask
      }

      // Guid -> Task
      // avaTODO: Research using return! here, i.e., return! _userRepo etc.
      member this.DeleteUserById userId = task {
        _userRepository.DeleteUserById userId |> Async.AwaitTask
      }
