namespace srvr.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open core.Services.UserService
open core.Records

[<ApiController>]
[<Route("user")>]
type UserController(
  logger : ILogger<UserController>,
  userService : IUserService) = 
  inherit ControllerBase()

  let _logger = logger
  let _userService = userService

  [<HttpPost>]
  [<Route("create")>]
  member this.CreateUser (user : User) = task {
    try
      match CreateUser.IsRequestValid user with
      | false ->
          return Results.BadRequest(
            "CreateUser request is invalid. " +
            $"Id : {user.Id}, " +
            $"Handle : {user.Handle}.")
      | true ->
          do! _userService.CreateUser user
          return Results.Ok(
            "User successfully created. " +
            $"Id : {user.Id}, " +
            $"Handle : {user.Handle}.")
    with ex ->
      _logger.LogError(
        "An unexpected error occurred while creating the user record.\n\t" +
        "[UserController] -> [CreateUser]:\n\t" +
        $"UserId : {user.Id}\n\t" +
        $"Handle : {user.Handle}.")
      return Results.Problem(
        title = "Internal server error occurred.",
        statusCode = 500
      )
  }

  [<HttpGet>]
  [<Route("get/{userId}")>]
  member this.GetUser (userId : int) = task {
    try
      match GetUser.IsRequestValid userId with
      | false ->
          return Results.BadRequest(
            "GetUserById request is invalid.\n\t" +
            $"UserId : {userId}")
      | true ->
          let! user = _userService.GetUserById userId
          return Results.Ok(user)
    with ex ->
      _logger.LogError(
        "An unexpected error occurred while fetching the user record.\n\t" +
        "[UserController] -> [GetUser]:\n\t" +
        $"UserId : {userId}.",
        ex)
      return Results.Problem(
        title = "Internal server error occurred.",
        statusCode = 500
      )
  }

  [<HttpPut>]
  [<Route("update")>]
  member this.UpdateUser (user : User) = task {
    try
      match UpdateUser.IsRequestValid user with
      | false ->
          return Results.BadRequest(
            "UpdateUser request is invalid.\n\t" +
            $"Id : {user.Id}\n\t" +
            $"Handle : {user.Handle}")
      | true -> 
          do! _userService.UpdateUser user
          return Results.Ok("User successfully updated.")
    with ex ->
      _logger.LogError(
        "An unexpected error occurred while updating the user record.\n\t" +
        "[UserController] -> [UpdateUser]:\n\t" +
        $"UserId : {user.Id}\n\t" +
        $"Handle : {user.Handle}.",
        ex)
      return Results.Problem(
        title = "Internal server error occurred.",
        statusCode = 500
      )
  }

  [<HttpDelete>]
  [<Route("delete/{userId}")>]
  member this.DeleteUser (userId : int) = task {
    try
      match DeleteUser.IsRequestValid userId with
      | false ->
          return Results.BadRequest(
            "DeleteUserById request is invalid.\n\t" +
            $"UserId : {userId}")
      | true ->  
          do! _userService.DeleteUserById userId
          return Results.Ok("User successfully deleted.")
    with ex ->
      _logger.LogError(
        "An unexpected error occurred while deleting the user record.\n\t" +
        "[UserController] -> [DeleteUser]:\n\t" +
        $"UserId : {userId}." +
        ex.Message) 
      return Results.Problem(
        title = "Internal server error occurred.",
        detail = ex.Message,
        statusCode = 500
      )
  }
