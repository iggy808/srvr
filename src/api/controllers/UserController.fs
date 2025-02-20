namespace srvr.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open core.Services.UserService
open core.Records

[<ApiController>]
[<Route("[controller]")>]
type UserController(
  logger : ILogger<UserController>,
  userService : IUserService) = 
  inherit ControllerBase()

  let _logger = logger
  let _userService = userService

  [<HttpPost>]
  member this.CreateUser user = task {
    try
      match CreateUser.Validation.IsRequestValid user with
      | false ->
          return BadRequest(
            "CreateUser request is invalid.\n\t" +
            $"Id : {user.Id}\n\t" +
            $"Handle : {user.Handle}.")
      | true ->
          _userService.CreateUser user |> Async.AwaitTask
          return Ok("User successfully created.")
    with ex ->
      _logger.LogError(
        "An unexpected error occurred while creating the user record.\n\t" +
        "[UserController] -> [CreateUser]:\n\t" +
        $"UserId : {user.Id}\n\t" +
        $"Handle : {user.Handle}.")
      return Error("Internal server error occurred.") 
  }

  [<HttpGet>]
  member this.GetUser userId = task {
    try
      match GetUserById.Validation.IsRequestValid userId with
      | false ->
          return BadRequest(
            "GetUserById request is invalid.\n\t" +
            $"UserId : {userId}"))
      | true ->
          let! user = _userService.GetUserById userId
          return Ok(user)
    with ex ->
      _logger.LogError(
        "An unexpected error occurred while fetching the user record.\n\t" +
        "[UserController] -> [GetUser]:\n\t" +
        $"UserId : {userId}.",
        ex)
      return Error
  }

  [<HttpPost>]
  member this.UpdateUser user = task {
    try
      match UpdateUser.Validation.IsRequestValid user with
      | false ->
          return BadRequest(
            "UpdateUser request is invalid.\n\t" +
            $"Id : {user.Id}\n\t" +
            $"Handle : {user.Handle}")
      | true -> 
          _userService.UpdateUser user |> Async.AwaitTask
          return Ok("User successfully updated.")
    with ex ->
      _logger.LogError(
        "An unexpected error occurred while updating the user record.\n\t" +
        "[UserController] -> [UpdateUser]:\n\t" +
        $"UserId : {user.Id}\n\t" +
        $"Handle : {user.Handle}.",
        ex)
      return Error("Internal server error occurred.") 
  }

  [<HttpDelete>]
  member this.DeleteUser userId = task {
    try
      match DeleteUserById.Validation.IsRequestValid userId with
      | false ->
          return BadRequest(
            "DeleteUserById request is invalid.\n\t" +
            $"UserId : {userId}")
      | true ->  
          _userService.DeleteUserById userId |> Async.AwaitTask
          return Ok("User successfully deleted.")
    with ex ->
      _logger.LogError(
        "An unexpected error occurred while deleting the user record.\n\t" +
        "[UserController] -> [DeleteUser]:\n\t" +
        $"UserId : {userId}.",
        ex) 
      return Error("Internal server error occurred.") 
  }
